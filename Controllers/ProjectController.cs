using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using WroseModel.Models;
using WroseModel.Repository;
using WroseModel.View_Model;
using OfficeOpenXml;

namespace WroseModel.Controllers
{
    public class ProjectController : Controller
    {
        private IProject Iproject;
        private IRegistration Iregistration;

        public ProjectController()
        {
            this.Iproject = new ProjectRepository(new WROSEDBEntities());
            this.Iregistration = new RegistrationRepository(new WROSEDBEntities());
        }
        public ActionResult DashBoard()
        {
            string name = null;
            string duplicatemsg = null;
            Session["projectId"] = null;
            if (TempData.ContainsKey("SuccessMessage"))
                name = TempData["SuccessMessage"] as string;

            TempData.Keep("SuccessMessage");

            if (TempData.ContainsKey("DuplicateMessage"))
                duplicatemsg = TempData["DuplicateMessage"] as string;

            TempData.Keep("DuplicateMessage");
            List<Project_Details> projectList = new List<Project_Details>();
            Project_Details projectDetails = new Project_Details();
            string useremail = string.Empty;
            int userID = 0;
            try
            {
                if (Session["email"] == null)
                {
                    return RedirectToAction("Login", "UserRegistration");
                }
                else
                {
                    useremail = Session["email"].ToString();
                    userID = this.Iproject.GetUserId(useremail);
                    projectList = this.Iproject.GetProjectListByUser(userID);
                    ViewBag.Projects = projectList;
                    if (projectList.Count > 0)
                    {
                        Session["projectId"] = projectList.FirstOrDefault().ID;
                        projectDetails = this.Iproject.GetProjectById(projectList.FirstOrDefault().ID);
                        ViewBag.ProjectDetails = projectDetails;
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View("~/Error/NotFound");
            }

        }
        [HttpPost]
        public JsonResult SaveWasteMat(List<Waste_Mat_Details> wasteMatsDetails)
        {
            int isSaved = 0;
            try
            {
                int projectId = Convert.ToInt32(Session["projectId"]);
                foreach (var wasteMat in wasteMatsDetails)
                {
                    wasteMat.Project_ID = projectId;
                    wasteMat.Created_Date = DateTime.Now;
                    wasteMat.Active = 1;
                }
                isSaved = this.Iproject.SaveWasteMatDetails(wasteMatsDetails);
                if (isSaved == 1)
                {
                    TempData["SuccessMessage"] = "Waste materials submitted successfully.";
                    ModelState.Clear();
                }
                else
                {
                    TempData["DuplicateMessage"] = "Waste materials submission failed";
                }
                return Json(isSaved, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult NewProjectPartail()
        {
            List<SelectListItem> provienceList = new List<SelectListItem>();
            try
            {
                provienceList = this.Iproject.GetProvienceList();
                provienceList.Insert(0, new SelectListItem { Text = "--Select Provience--", Value = "0" });
                ViewBag.Provience = provienceList;
                return PartialView("_NewProjectPartial");
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetMunicipalityList(int ProvienceId)
        {
            try
            {
                IEnumerable<SelectListItem> municipalityList = new List<SelectListItem>();
                municipalityList = this.Iproject.GetMunicipalityList(ProvienceId);
                ViewBag.Municipality = municipalityList;
                return Json(new SelectList(municipalityList, "Value", "Text", JsonRequestBehavior.AllowGet));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        // GET: Project
        public ActionResult Index()
        {
            return View();
        }

        // GET: Project/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, Project_Details objProjectDetails)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int id)
        {
            string useremail = string.Empty;
            int userID = 0;
            List<Project_Details> projectList = new List<Project_Details>();
            Project_Details projectDetails = new Project_Details();
            try
            {

                useremail = Session["email"].ToString();
                userID = this.Iproject.GetUserId(useremail);
                projectList = this.Iproject.GetProjectListByUser(userID);
                ViewBag.Projects = projectList;
                projectDetails = this.Iproject.GetProjectById(id);
                ViewBag.ProjectDetails = projectDetails;
                return View("DashBoard");
            }
            catch (Exception ex)
            {
                return View("~/Error/NotFound");
            }
        }

        // POST: Project/Edit/5
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

        // GET: Project/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Project/Delete/5
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
        [HttpPost]
        public ActionResult NewProject(ProjectVM objProject)
        {
            int isSaved = 0;
            string useremail = string.Empty;
            string projectName = string.Empty;
            try
            {
                if (objProject.ID > 0)
                {
                    objProject.Active = 1;
                    objProject.Modified_Date = DateTime.Now;
                    isSaved = this.Iproject.EditProject(objProject);
                    if (isSaved == 1)
                    {
                        TempData["SuccessMessage"] = "New project submitted successfully.";
                        ModelState.Clear();
                    }
                    else
                    {
                        TempData["DuplicateMessage"] = "Project submission failed";
                    }
                }
                else
                {
                    if (Session["email"] != null)
                    {
                        List<Project_Details> projectList = new List<Project_Details>();
                        useremail = Session["email"].ToString();
                        objProject.User_Reg_ID = this.Iproject.GetUserId(useremail);
                        projectList = this.Iproject.GetProjectListByUser(objProject.User_Reg_ID);
                        if (projectList.Count > 0)
                        {
                            projectName = projectList.First(x => x.Project_Name == objProject.Project_Name).Project_Name;
                        }
                        if (!string.IsNullOrEmpty(projectName))
                        {
                            TempData["DuplicateMessage"] = "Project name '" + objProject.Project_Name + "'  already exists.Please try different project name.";
                        }

                        else
                        {
                            objProject.Active = 1;
                            objProject.Created_Date = DateTime.Now;
                            isSaved = this.Iproject.Create(objProject);
                            if (isSaved == 1)
                            {
                                TempData["SuccessMessage"] = "New project submitted successfully.";
                                ModelState.Clear();
                            }
                            else
                            {
                                TempData["DuplicateMessage"] = "Project submission failed";
                            }
                        }

                    }
                }

                return RedirectToAction("DashBoard");
            }
            catch (Exception ex)
            {
                return View("~/Error/NotFound");
            }
        }
        public ActionResult Upload(FormCollection formCollection)
        {
            int isSaved = 0;
            string wasteMatName = string.Empty;
            int wasteMatId = 0;
            decimal wasteMatPer = 0;
            try
            {
                int projectId = Convert.ToInt32(Session["projectId"]);
                if (Request != null)
                {
                    HttpPostedFileBase file = Request.Files["UploadedFile"];
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                        var wastematList = new List<Waste_Mat_Details>();
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;
                            for (int rowIterator = 2; rowIterator < noOfRow; rowIterator++)
                            {
                                var wasteMat = new Waste_Mat_Details();
                                wasteMatName = workSheet.Cells[rowIterator, 1].Value.ToString();
                                wasteMatId = Iproject.GetWasteMatIdByName(wasteMatName);
                                //need to check projectid if null then need to insert wastematname to master table first.
                                if (wasteMatId == 0)
                                {
                                    var wasteMatMaster = new Waste_Mat_Master();
                                    wasteMatMaster.Waste_Mat_Name = wasteMatName;
                                    wasteMatMaster.Active = 1;
                                    wasteMatMaster.Created_Date = DateTime.Now;
                                    int wasteMatsaved = Iproject.InsertWastMatMaster(wasteMatMaster);
                                    if (wasteMatsaved == 1)
                                    {
                                        wasteMatId = Iproject.GetWasteMatIdByName(wasteMatName);
                                    }
                                }
                                wasteMat.Waste_Mat_Amt = Convert.ToDecimal(workSheet.Cells[rowIterator, 2].Value.ToString());
                                wasteMat.Total_Waste_Amt = Convert.ToDecimal(workSheet.Cells[noOfRow, 2].Value.ToString());
                                wasteMatPer = Math.Round((wasteMat.Waste_Mat_Amt / wasteMat.Total_Waste_Amt) * 100, 2);
                                wasteMat.Waste_mat_Percentage = wasteMatPer;
                                wasteMat.Waste_Mat_ID = wasteMatId;
                                wasteMat.Project_ID = projectId;
                                wasteMat.Active = 1;
                                wasteMat.Created_Date = DateTime.Now;
                                wastematList.Add(wasteMat);
                            }
                        }
                        isSaved = this.Iproject.SaveWasteMatDetails(wastematList);
                        if (isSaved == 1)
                        {
                            TempData["SuccessMessage"] = "Waste materials submitted successfully.";
                            ModelState.Clear();
                        }
                        else
                        {
                            TempData["DuplicateMessage"] = "Waste materials submission failed";
                        }
                    }
                }
                return RedirectToAction("DashBoard");
            }
            catch (Exception ex)
            {
                return View("~/Error/NotFound");
            }


        }

        [HttpPost]
        public ContentResult ProjectDetails(int id)
        {
            List<SelectListItem> provienceList = new List<SelectListItem>();
            Project_Details projectDetails = new Project_Details();
            ProjectVM objProject = new ProjectVM();
            try
            {
                projectDetails = this.Iproject.GetProjectById(id);
                provienceList = this.Iproject.GetProvienceList();
                provienceList.Insert(0, new SelectListItem { Text = "--Select Provience--", Value = "0" });
                ViewBag.Provience = provienceList;
                objProject.Key_Assumption = projectDetails.Key_Assumption;
                objProject.Local_Auth_Study = projectDetails.Local_Auth_Study;
                objProject.Client_Name = projectDetails.Client_Name;
                objProject.Project_Name = projectDetails.Project_Name;
                objProject.Project_Desc = projectDetails.Project_Desc;
                objProject.Study_Limitation = projectDetails.Study_Limitation;
                objProject.Study_Year = projectDetails.Study_Year;
                objProject.Author_company = projectDetails.Author_company;
                objProject.Author_Name = projectDetails.Author_Name;
                objProject.Stusy_Purpose = projectDetails.Stusy_Purpose;
                objProject.ID = projectDetails.ID;
                objProject.User_Reg_ID = projectDetails.User_Reg_ID;
                objProject.Created_Date = projectDetails.Created_Date;
                objProject.Modified_Date = projectDetails.Modified_Date;
                string viewContent = ConvertViewToString("_NewProjectPartial", objProject);
                return Content(viewContent);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string ConvertViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (StringWriter writer = new StringWriter())
            {
                ViewEngineResult vResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext vContext = new ViewContext(this.ControllerContext, vResult.View, ViewData, new TempDataDictionary(), writer);
                vResult.View.Render(vContext, writer);
                return writer.ToString();
            }
        }
        [HttpPost]
        public ActionResult GetProjectById(int projectId)
        {
            try
            {
                Project_Details projectDetails = new Project_Details();
                projectDetails = this.Iproject.GetProjectById(projectId);
                return View();
            }
            catch (Exception ex)
            {
                return View("~/Error/NotFound");
            }

        }
    }
}
