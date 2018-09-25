using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using TheLibraryIsOpen.Controllers.StorageManagement;
using TheLibraryIsOpen.Database;
using TheLibraryIsOpen.Models.Authentication;
using TheLibraryIsOpen.Models.DBModels;

namespace TheLibraryIsOpen.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly Db _db; //gonna change this to the actual thing we're gonna use. (Catalog, UoW, etc.)
        private readonly ClientManager _cm;

        public AuthenticationController(Db db, ClientManager cm) //gonna change this to the actual thing we're gonna use (Catalog, UoW, etc.)
        {
            _db = db;
            _cm = cm;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            Client c = await _cm.FindAsync(model.Email, model.Password);
            if (c != null)
            {
                await SignInAsync(c, true);
            }
            // TODO: Add insert logic here

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                throw new Exception();
            var x = await _cm.CreateAsync(model.ToClient());
            if (!x.Succeeded)
            {
                throw new Exception(x.Errors.First().Description);
            }
            else
            {
                var client = await _cm.FindByEmailAsync(model.Email);
                await SignInAsync(client, true);
            }
            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            Client c = await _cm.FindByIdAsync(id.ToString());
            EditViewModel edit = new EditViewModel
            {
                Email = c.EmailAddress,
                FName = c.FirstName,
                LName = c.LastName,
                PhoneNumber = c.PhoneNo,
                IsAdmin = c.IsAdmin
            };
            return View(edit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, EditViewModel edit)
        {
            try
            {
                Client c = await _cm.FindByIdAsync(id.ToString());
                c.EmailAddress = edit.Email;
                c.FirstName = edit.FName;
                c.LastName = edit.LName;
                c.PhoneNo = edit.PhoneNumber;
                c.UserName = edit.Email;
                c.IsAdmin = edit.IsAdmin;
                await _cm.UpdateAsync(c);
                return RedirectToAction(nameof(Edit), id);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private async Task SignInAsync(Client user, bool isPersistent)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.EmailAddress),
                    new Claim(ClaimTypes.Email, user.EmailAddress),
                    new Claim("Id", user.Id),

                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. Required when setting the 
                // ExpireTimeSpan option of CookieAuthenticationOptions 
                // set with AddCookie. Also required when setting 
                // ExpiresUtc.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
}