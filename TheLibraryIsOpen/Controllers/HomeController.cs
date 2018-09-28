﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheLibraryIsOpen.Models;
using TheLibraryIsOpen.Models.DBModels;
using TheLibraryIsOpen.Controllers.StorageManagement;
using TheLibraryIsOpen.Database;


namespace TheLibraryIsOpen.Controllers
{
	public class HomeController : Controller
	{
	    
        public IActionResult Index()
		{
			return View();
		}

		public IActionResult About()
		{
			ViewData["Message"] = "Your application description page.";

			return View();
		}

		public IActionResult Contact()
		{
			ViewData["Message"] = "Your contact page.";

			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

        public async Task<ActionResult> ListOfClients()
        {
            ClientStore cs = new ClientStore(new Db());
            string clientEmail = User.Identity.Name;
            //Client client = await cs.FindByNameAsync(clientEmail);

            bool isAdmin = await cs.IsItAdminAsync(clientEmail);
            if (isAdmin)
            {
                return View();
            }
            else
            {
                return Unauthorized();
            }
        }

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
