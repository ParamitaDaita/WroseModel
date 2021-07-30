using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WroseModel.Models;
using WroseModel.Repository;
using System.Web.Script.Serialization;
namespace WroseModel.Controllers
{
    public class WasteMatController : Controller
    {
        private IProject Iproject;
        public WasteMatController()
        {
            this.Iproject = new ProjectRepository(new WROSEDBEntities());
        }
        // GET: WasteMat
        public ActionResult Index()
        {
            return View();
        }

        // GET: WasteMat/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WasteMat/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WasteMat/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: WasteMat/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WasteMat/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: WasteMat/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WasteMat/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public PartialViewResult AddWasteMatPartail()
        {
            List<SelectListItem> wasteMatList = new List<SelectListItem>();
            try
            {
                wasteMatList = this.Iproject.GetWasteMatList();
                //provienceList.Insert(0, new SelectListItem { Text = "--Select Provience--", Value = "0" });
                ViewBag.WasteMat = wasteMatList;
                return PartialView("_AddWasteMatPartial");
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
      
    }
}
