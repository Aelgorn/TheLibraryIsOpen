﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLibraryIsOpen.Constants;
using TheLibraryIsOpen.Controllers.StorageManagement;
using TheLibraryIsOpen.db;
using TheLibraryIsOpen.Models;
using TheLibraryIsOpen.Models.Cart;
using TheLibraryIsOpen.Models.DBModels;
using TheLibraryIsOpen.Models.Return;
using static TheLibraryIsOpen.Constants.TypeConstants;

namespace TheLibraryIsOpen.Controllers
{
    public class CartController : Controller
    {

        private readonly ClientManager _cm;
        private readonly BookCatalog _bookc;
        private readonly MusicCatalog _musicc;
        private readonly MovieCatalog _moviec;
        private readonly MagazineCatalog _magazinec;
        private readonly IdentityMap _identityMap;
        private readonly ModelCopyCatalog _modelCopyCatalog;

        public CartController(ClientManager cm, BookCatalog bc, MusicCatalog muc, MovieCatalog moc, MagazineCatalog mac, IdentityMap imap, ModelCopyCatalog modelCopyCatalog)
        {
            _cm = cm;
            _bookc = bc;
            _moviec = moc;
            _musicc = muc;
            _magazinec = mac;
            _identityMap = imap;
            _modelCopyCatalog = modelCopyCatalog;
        }

        public async Task<IActionResult> Index()
        {
            Client client = await _cm.FindByEmailAsync(User.Identity.Name);
            int borrowMax = client.BorrowMax;
            int numModels = await _identityMap.CountModelCopiesOfClient(client.clientId);
            int cartCount = HttpContext.Session.GetInt32("ItemsCount") ?? 0;
            List<SessionModel> Items = HttpContext.Session.GetObject<List<SessionModel>>("Items") ?? new List<SessionModel>();
            TempData["totalBorrowed"] = cartCount + numModels;
            TempData["canBorrow"] = cartCount + numModels <= borrowMax;
            TempData["borrowMax"] = borrowMax;
            List<Task<Book>> bookTasks = new List<Task<Book>>(Items.Count);
            List<Task<Magazine>> magazineTasks = new List<Task<Magazine>>(Items.Count);
            List<Task<Movie>> movieTasks = new List<Task<Movie>>(Items.Count);
            List<Task<Music>> musicTasks = new List<Task<Music>>(Items.Count);

            foreach (SessionModel element in Items)
            {
                switch (element.ModelType)
                {
                    case TypeEnum.Book:
                        {
                            bookTasks.Add(_bookc.FindByIdAsync(element.Id.ToString()));
                            break;
                        }
                    case TypeEnum.Magazine:
                        {
                            magazineTasks.Add(_magazinec.FindByIdAsync(element.Id.ToString()));
                            break;
                        }
                    case TypeEnum.Movie:
                        {
                            movieTasks.Add(_moviec.GetMovieByIdAsync(element.Id));
                            break;
                        }
                    case TypeEnum.Music:
                        {
                            musicTasks.Add(_musicc.FindMusicByIdAsync(element.Id.ToString()));
                            break;
                        }

                }
            }

            List<CartViewModel> result = new List<CartViewModel>(Items.Count);

            result.AddRange(bookTasks.Select(t =>
            {
                t.Wait();
                Book b = t.Result;
                return new CartViewModel { ModelId = b.BookId, Title = b.Title, Type = TypeEnum.Book };
            }));

            result.AddRange(magazineTasks.Select(t =>
            {
                t.Wait();
                Magazine m = t.Result;
                return new CartViewModel { ModelId = m.MagazineId, Title = m.Title, Type = TypeEnum.Magazine };
            }));

            result.AddRange(movieTasks.Select(t =>
            {
                t.Wait();
                Movie m = t.Result;
                return new CartViewModel { ModelId = m.MovieId, Title = m.Title, Type = TypeEnum.Movie };
            }));

            result.AddRange(musicTasks.Select(t =>
            {
                t.Wait();
                Music m = t.Result;
                return new CartViewModel { ModelId = m.MusicId, Title = m.Title, Type = TypeEnum.Music };
            }));


            return View(result);
        }


        //removes item from session
        public IActionResult RemoveFromSessionModel(int modelId, TypeEnum mt)
        {
            int cartCount = HttpContext.Session.GetInt32("ItemsCount") ?? 0;

            List<SessionModel> Items = HttpContext.Session.GetObject<List<SessionModel>>("Items") ?? new List<SessionModel>();

            Items.RemoveAt(
                Items.FindIndex(item => item.Id == modelId && item.ModelType == mt)
            );

            HttpContext.Session.SetObject("Items", Items);
            cartCount = cartCount - 1;

            HttpContext.Session.SetInt32("ItemsCount", cartCount);


            return RedirectToAction(nameof(Index));
        }


        //registers modelcopies of selected items to the client
        public async Task<IActionResult> Borrow()
        {
            List<SessionModel> modelsToBorrow = HttpContext.Session.GetObject<List<SessionModel>>("Items") ?? new List<SessionModel>();

            Client client = await _cm.FindByEmailAsync(User.Identity.Name);
            List<ModelCopy> alreadyBorrowed = await _identityMap.FindModelCopiesByClient(client.clientId);

            //Borrow all available copies of selected items
            Boolean successfulReservation = await _identityMap.ReserveModelCopiesToClient(modelsToBorrow, client.clientId);

            //if not all items were borrowed, determine which ones were not borrowed and display them to the client
            if (!successfulReservation)
            {
                List<ModelCopy> nowBorrowed = await _identityMap.FindModelCopiesByClient(client.clientId);
                HashSet<ModelCopy> borrowed = nowBorrowed.Except(alreadyBorrowed).ToHashSet();
                List<SessionModel> notBorrowed = modelsToBorrow
                                                    .Select(m => new { Id = m.Id, MT = m.ModelType })
                                                    .Except(borrowed
                                                            .Select(m => new { Id = m.modelID, MT = m.modelType }))
                                                    .Select(c => new SessionModel { Id = c.Id, ModelType = c.MT })
                                                    .ToList();

                HttpContext.Session.SetObject("Items", notBorrowed);
                HttpContext.Session.SetInt32("ItemsCount", notBorrowed.Count);
                return RedirectToAction(nameof(Index));
            }

            //TODO Return to Home?
            HttpContext.Session.SetObject("Items", null);
            HttpContext.Session.SetInt32("ItemsCount", 0);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Return(List<ReturnViewModel> mtr)
        {
            // do in a contract in preconditions 
            Client client = await _cm.FindByEmailAsync(User.Identity.Name);

            List<ModelCopy> alreadyBorrowed = await _identityMap.FindModelCopiesByClient(client.clientId);
            //

            var modelsToReturn = mtr.Where(rvm => rvm.ToReturn).Select(rvm => new ModelCopy
            {
                id = rvm.ModelCopyId,
                modelType = rvm.ModelType,
                borrowedDate = rvm.BorrowDate,
                borrowerID = null,
                returnDate = rvm.ReturnDate,
                modelID = rvm.ModelId
            });
            foreach (var item in modelsToReturn)
            {
                await _modelCopyCatalog.UpdateAsync(item);
            }

            await _modelCopyCatalog.CommitAsync();

            return RedirectToAction("Index", "Return");
        }


    }
}
