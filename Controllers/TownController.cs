using DeigCrud.Infrastructure;
using DeigCrud.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DeigCrud.Controllers
{
    [Authorize(Roles = "Admin, List")]
    public class TownController : Controller
    {
      
        const string UPDATE = "U";
        const string CREATE = "C";
        const string DELETE = "D";

        int districtnumber = -1;      
        static string msg = "";
        int TownId = 0;

        [HttpGet]
        public IActionResult Index()
        {
            var list = TempData["id"];
            if (list != null)
            {
                TownId = Convert.ToInt32(list);
            }

            // Deig town distrct model
            var dtdmodel = new DTownDistrictViewModel()
            {
                TownModel = DropDownHelpers.PopulateTowns(),
                DistrictModel = DropDownHelpers.PopulateDistricts(),

                DistrictTownModel = TownHelpers.PopulateDistrictsTown(TownId, districtnumber)
            };

            if (TownId > 0)
            {
                switch (TempData["sender"])
                {
                    case "U":
                        ViewBag.Result = $"Town Id:  {TownId} has been updated.";
                        break;
                    case "C":
                        ViewBag.Result = $"Town Id:  {TownId} has been created.";
                        break;
                    case "D":
                        ViewBag.Result = $"Poof! Town id:  {TownId} has been deleted."; ;
                        break;
                    default:
                        ViewBag.Result = $"Town Id:  { TownId} has been ???";
                        break;
                }
            }

            return View(dtdmodel);

        }

        // Post Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(int? TownIdSelect, int? DistrictSelect)
        {
            var dtdmodel = new DTownDistrictViewModel()
            {
                TownModel = DropDownHelpers.PopulateTowns(),
                DistrictModel = DropDownHelpers.PopulateDistricts(),

                DistrictTownModel = TownHelpers.PopulateDistrictsTown(TownIdSelect, DistrictSelect)
            };

            return View(dtdmodel);
        }

        // Create Get 
        [HttpGet]
        public IActionResult Create()
        {
            var dtdmodel = new DTownDistrictViewModel()
            {
                TownModel = DropDownHelpers.PopulateTowns(),
                DistrictModel = DropDownHelpers.PopulateDistricts(),

                DistrictTownModel = TownHelpers.PopulateDistrictsTown(TownId, districtnumber)
            };

            return View("Create", dtdmodel);
        }

        // Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DTownDistrictViewModel dtd)
        {
            //todo: Create constants for sp for create and update
            //todo: Call function, passing in the model, and sp name.
            //todo: redirect to index to display the new record with ListId
            //todo: z Create page -- add vars containers
            string rc = "";
            if (ModelState.IsValid)
            {
                rc = TownHelpers.UpdateDistrictTowns(dtd, TownId);
            }

            // Int or string?
            if (rc.Length > 0) { TempData["id"] = Convert.ToInt32(rc); }
            TempData["sender"] = CREATE;
            return RedirectToAction("Index");
        }

        // Update
        [HttpGet]
        public IActionResult Update(int id)
        {
            TownId = id;
            var dtd = new DTownDistrictViewModel()
            {
                DistrictModel = DropDownHelpers.PopulateDistricts(),
                TownModel = DropDownHelpers.PopulateTowns(),

                DistrictTownModel = TownHelpers.PopulateDistrictsTown(TownId, districtnumber)
            };

            ViewBag.Result = $"Update towns with the id: {TownId.ToString()}";
            TempData["id"] = TownId;
            return View("Update", dtd);   //  Update a meeting: " + id.ToString();
        }

        // Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(DTownDistrictViewModel dtdView)
        {
            int id = Convert.ToInt32(TempData["id"]);
            string rc = TownHelpers.UpdateDistrictTowns(dtdView, id);

            TempData["sender"] = UPDATE;
            TempData["id"] = id;
            return RedirectToAction("Index");
        }

        // Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {

            TempData["id"] = TownId;
            TempData["sender"] = DELETE;

            msg = TownHelpers.DeleteTown(id);
           
            return RedirectToAction("Index");

        }
    }
}
