using DeigCrud.Infrastructure;
using DeigCrud.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DeigCrud.Controllers
{
    [Authorize(Roles = "Admin, List")]
    public class OnlineController : Controller
    {
        const string SPUPATE = "spUpdateOnlineList";
        const string SPCREATE = "spCreateOnlineList";
        const string SPDELETE = "spDeleteOnlineList";
        const string SPGETONLINE = "spGetOnlineList";
        const string UPDATE = "U";
        const string CREATE = "C";
        const string DELETE = "D";

        // ZoomId and TempData["id"] is used to pass the id from controller function to controller function
        // But not the helper functions
        int ZoomId = 0;
        int dayId = 0;
        int timeId = 0;
        static string msg = "";
        int districtnumber = 0;
        string strUrl = "";


#nullable enable
        [HttpGet]
        public IActionResult Index()
        {

            ZoomId = Convert.ToInt32(TempData["id"]);
            if (ZoomId == 0)
            {
                ZoomId = 0;
            }

            var doViewmodel = new DoViewModel()
            {
                DOWModel = DropDownHelpers.PopulateDOW(),
                TimeModel = DropDownHelpers.PopulateTime(),
                DistrictModel = DropDownHelpers.PopulateDistricts(),
                OnlineListModel = (IEnumerable<OnlineMeetingsModel>)OnlineHelpers.PopulateOnlineList(ZoomId, dayId, timeId, districtnumber)
            };

            // For displaying rssults
            if (ZoomId > 0)
            {
                switch (TempData["sender"])
                {
                    case "U":
                        ViewBag.Result = $"Zoom Id:  {ZoomId} has been updated.";
                        break;
                    case "C":
                        ViewBag.Result = $"Zoom Id:  {ZoomId} has been created.";
                        break;
                    case "D":
                        ViewBag.Result = $"Zoom Id:  { ZoomId} has been deleted.";
                        break;
                    default:
                        ViewBag.Result = $"Zoom Id:  { ZoomId} has been ???";
                        break;
                }
                //ViewBag.Result = $"Meeting id:  {listId} has been updated.";
            }

            return View(doViewmodel);
        }

        // Index Post
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public IActionResult Index(int? ZoomId, int? DOWSelect, int? TimeSelect, int? DistrictSelect)
        {

            var doViewmodel = new DoViewModel()
            {
                DOWModel = DropDownHelpers.PopulateDOW(),
                TimeModel = DropDownHelpers.PopulateTime(),
                DistrictModel = DropDownHelpers.PopulateDistricts(),
                OnlineListModel = (IEnumerable<OnlineMeetingsModel>)OnlineHelpers.PopulateOnlineList(ZoomId, DOWSelect, TimeSelect, DistrictSelect)
            };

            return View(doViewmodel);
        }

        // Get Display
        [HttpGet]
        public IActionResult Display()
        {

            ZoomId = Convert.ToInt32(TempData["id"]);
            if (ZoomId == 0)
            {
                ZoomId = 0;
            }

            var doViewmodel = new DoViewModel()
            {
                DOWModel = DropDownHelpers.PopulateDOW(),
                TimeModel = DropDownHelpers.PopulateTime(),
                DistrictModel = DropDownHelpers.PopulateDistricts(),
                OnlineListModel = (IEnumerable<OnlineMeetingsModel>)OnlineHelpers.PopulateOnlineList(ZoomId, dayId, timeId, districtnumber)
            };

            return View(doViewmodel);
        }

        [HttpPost]
        public IActionResult Display(int? ZoomId, int? DOWSelect, int? TimeSelect)
        {

            var doViewmodel = new DoViewModel()
            {
                DOWModel = DropDownHelpers.PopulateDOW(),
                TimeModel = DropDownHelpers.PopulateTime(),
                DistrictModel = DropDownHelpers.PopulateDistricts(),
                OnlineListModel = (IEnumerable<OnlineMeetingsModel>)OnlineHelpers.PopulateOnlineList(ZoomId, DOWSelect, TimeSelect, districtnumber)
            };

            return View(doViewmodel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var doViewmodel = new DoViewModel()
            {
                DOWModel = DropDownHelpers.PopulateDOW(),
                TimeModel = DropDownHelpers.PopulateTime(),
                DistrictModel = DropDownHelpers.PopulateDistricts(),
                OnlineListModel = (IEnumerable<OnlineMeetingsModel>)OnlineHelpers.PopulateOnlineList(ZoomId, dayId, timeId, districtnumber)
            };
            return View("Create", doViewmodel);
        }

        [HttpPost]
        public IActionResult Create(DoViewModel dol)
        {
            //todo: Create constants for sp for create and update
            //todo: Call function, passing in the model, and sp name.
            //todo: redirect to index to display the new record with ListId
            //todo: z Create page -- add vars containers

            string rc = "err";
            rc = OnlineHelpers.UpdateOnlineList(dol, ZoomId, SPCREATE);

            // Int or string?           
            TempData["id"] = Convert.ToInt32(rc);
            TempData["sender"] = CREATE;
            return RedirectToAction("Index");
        }

        // [ValidateAntiForgeryToken]
        public IActionResult Update(int id)
        {
            ZoomId = id;
            var domodel = new DoViewModel()
            {
                DOWModel = DropDownHelpers.PopulateDOW(),
                TimeModel = DropDownHelpers.PopulateTime(),
                DistrictModel = DropDownHelpers.PopulateDistricts(),
                OnlineListModel = OnlineHelpers.PopulateOnlineList(ZoomId, dayId, timeId, districtnumber)
            };

            ViewBag.Result = $"To update online meeting id: {ZoomId.ToString()}";
            TempData["id"] = ZoomId;
            return View("Update", domodel);   //  Update a meeting: " + id.ToString();
        }

        // Post update
        [HttpPost]
        public IActionResult Update(DoViewModel dol)
        {
            int id = Convert.ToInt32(TempData["id"]);
            string rc = OnlineHelpers.UpdateOnlineList(dol, id, SPUPATE);

            ViewBag.Result = $"Updated online meeting id: {id.ToString()}";
            TempData["id"] = id;
            TempData["sender"] = UPDATE;
            return RedirectToAction("Index");
        }

        // Get delete
        public IActionResult Delete(int id)
        {
            ZoomId = id;
            var domodel = new DoViewModel()
            {
                DOWModel = DropDownHelpers.PopulateDOW(),
                TimeModel = DropDownHelpers.PopulateTime(),
                OnlineListModel = OnlineHelpers.PopulateOnlineList(ZoomId, dayId, timeId, districtnumber)
            };

            TempData["id"] = ZoomId;
            ViewBag.Message = $"ZoomId: {ZoomId.ToString()} is staged for delete.";
            TempData["sender"] = UPDATE;
            return View("Delete", domodel);   //  Update a meeting: " + id.ToString();
        }

        // Post delete
        [HttpPost]
        public IActionResult Delete()
        {
            int ZoomId = Convert.ToInt32(TempData["id"]);
            if (ZoomId > 0)
            {
                //ZoomId = Convert.ToInt32(strId);
                string rc = OnlineHelpers.DeleteFunction(ZoomId);
                TempData["id"] = ZoomId;
                TempData["sender"] = DELETE;
            }
            else
            {
                // ViewBag error msg
                ZoomId = 0;
                return View();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("[controller]/Cancel")]
        public IActionResult Cancel()
        {

            ViewBag.Message = "Cancel delete request: ";
            TempData["id"] = "0";

            return RedirectToAction("Index");

        }       
    }
}
