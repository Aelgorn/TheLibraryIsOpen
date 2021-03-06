﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLibraryIsOpen.Constants;
using TheLibraryIsOpen.Controllers.StorageManagement;
using TheLibraryIsOpen.Models;
using TheLibraryIsOpen.Models.Search;

namespace TheLibraryIsOpen.Controllers
{
    public class TransactionController : Controller
    {
        private readonly TransactionCatalog _tc;
        private readonly SearchTransactions _st;


        public TransactionController(TransactionCatalog tc, SearchTransactions st)
        {
            _tc = tc;
            _st = st;
        }

        public async Task<IActionResult> Index()
        {
            bool fromSearch = HttpContext.Session.GetObject<bool>("fromSearch");
            HttpContext.Session.SetObject("fromSearch", false);
            var logs = HttpContext.Session.GetObject<List<PrintedLog>>("logs");
            HttpContext.Session.SetObject("logs", null);
            return View(fromSearch ? logs : await _tc.GetLogs());
        }

        [HttpPost]
        public async Task<IActionResult> SearchTransactionHistory()
        {
            var form = HttpContext.Request.Form;
            string clientID = form["clientID"];
            string copyID = form["copyID"];
            string modelType = form["modeltype"];
            string modelID = form["modelID"];
            string date1 = form["date1"];
            string date2 = form["date2"];
            string time1 = string.IsNullOrEmpty(form["time1"]) ? "" : $" {form["time1"]}";
            string time2 = string.IsNullOrEmpty(form["time2"]) ? "" : $" {form["time2"]}";
            bool exactTime = form["exacttime"] == "true";
            string transac = form["transc"];

            string dateTime1 = date1 + time1;
            string dateTime2 = date2 + time2;

            TempData["ClientID"] = clientID;
            TempData["CopyID"] = copyID;
            TempData["ModelType"] = modelType;
            TempData["ModelID"] = modelID;
            TempData["DateTime1"] = dateTime1;


            List<PrintedLog> logs = await _st.SearchLogsAsync(clientID, copyID, modelType, modelID, dateTime1, dateTime2, exactTime, transac);

            HttpContext.Session.SetObject("logs", logs);
            HttpContext.Session.SetObject("fromSearch", true);

            return RedirectToAction(nameof(Index));
        }
    }
}