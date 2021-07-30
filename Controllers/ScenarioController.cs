using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WroseModel.Models;
using WroseModel.Repository;
using WroseModel.View_Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WroseModel.Controllers
{
    public class ScenarioController:Controller
    {
        private IScenario Iscenario;

        public ScenarioController()
        {
            this.Iscenario = new ScenarioRepository(new WROSEDBEntities());
        }
        public PartialViewResult AddScenarioPartail()
        {
            //List<SelectListItem> categoryList = new List<SelectListItem>();
            //List<SelectListItem> projectList = new List<SelectListItem>();
            //List<SelectListItem> wasteTechList = new List<SelectListItem>();
            try
            {
                //categoryList = this.Iscenario.GetCategoryList();
                //ViewBag.Category = categoryList;
                //projectList = this.Iscenario.GetProjectList();
                //ViewBag.Project = projectList;
                //wasteTechList = this.Iscenario.GetWasteTechList();
                //ViewBag.WasteTech = wasteTechList;
                return PartialView("_AddScenarioPartial");
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult CategoryList()
        {
            try
            {
                IEnumerable<SelectListItem> categoryList = new List<SelectListItem>();
                categoryList = this.Iscenario.GetCategoryList();
                return Json(new SelectList(categoryList, "Value", "Text", JsonRequestBehavior.AllowGet));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public JsonResult ScenarioList()
        {
            try
            {
                //int projectId = 0;
                //if (Session["projectId"] != null)
                //{
                //    projectId = Convert.ToInt32(Session["projectId"]);
                //}
                IEnumerable<SelectListItem> scenarioList = new List<SelectListItem>();
                scenarioList = this.Iscenario.GetScenarioList();
                return Json(new SelectList(scenarioList, "Value", "Text", JsonRequestBehavior.AllowGet));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public JsonResult ScenarioDetailsByScenarioID(int ScenarioID)
        {
            try
            {
                Scenario_Master scenarioMaster = new Scenario_Master();
                scenarioMaster = this.Iscenario.ScenarioDetailsByScenarioID(ScenarioID);
                string str = System.Convert.ToBase64String(scenarioMaster.Image_File, 0, scenarioMaster.Image_File.Length);
                return Json(new { Image = str, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public JsonResult WasteTechListByCategory(int CategoryID)
        {
            try
            {
                IEnumerable<SelectListItem> wasteTechlistList = new List<SelectListItem>();
                wasteTechlistList = this.Iscenario.WasteTechListByCategory(CategoryID);
                return Json(new SelectList(wasteTechlistList, "Value", "Text", JsonRequestBehavior.AllowGet));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public JsonResult SaveScenarioDiagram(string diagramData,string scenarioName)
        {
            try
            {
                Scenario_Details scenarioDetails = new Scenario_Details();
                List<Scenario_Node_details> listNodeDetail = new List<Scenario_Node_details>();
                List<Scenario_Connection_Details> listConnDetail = new List<Scenario_Connection_Details>();
                int projectId = 0;
                if (Session["projectId"] != null)
                {
                    projectId = Convert.ToInt32(Session["projectId"]);
                }
               
                if (!string.IsNullOrEmpty(diagramData) && !string.IsNullOrEmpty(scenarioName))
                {
                    JToken allnode = JToken.Parse(diagramData);
                    var nodes = allnode["nodes"].Children().ToList();
                    foreach (var innernodes in nodes)
                    {
                        Scenario_Node_details scenarionodedetails = new Scenario_Node_details();
                        var innernode = innernodes.Children().ToList();
                        scenarionodedetails.Block_ID = ((JValue)((JProperty)innernode[0]).Value).Value.ToString();
                        scenarionodedetails.Waste_Tech_ID= Convert.ToInt32(((JValue)((JProperty)innernode[1]).Value).Value);
                        scenarionodedetails.Position_X= Convert.ToInt32(((JValue)((JProperty)innernode[2]).Value).Value);
                        scenarionodedetails.Position_Y= Convert.ToInt32(((JValue)((JProperty)innernode[3]).Value).Value);
                        listNodeDetail.Add(scenarionodedetails);
                    }
                    var connectionnodes = allnode["connections"].Children().ToList();
                    foreach(var connectionnode in connectionnodes)
                    {
                        Scenario_Connection_Details scenarioconndetails = new Scenario_Connection_Details();
                        var connode = connectionnode.Children().ToList();
                        scenarioconndetails.Connection_ID = ((JValue)((JProperty)connode[0]).Value).Value.ToString();
                        scenarioconndetails.Page_Source_ID = ((JValue)((JProperty)connode[1]).Value).Value.ToString();
                        scenarioconndetails.Page_Target_ID = ((JValue)((JProperty)connode[2]).Value).Value.ToString();
                        listConnDetail.Add(scenarioconndetails);
                    }
                    var noofelements = allnode["numberOfElements"];
                    var categoryid = allnode["categoryId"];

                    scenarioDetails.Project_ID = projectId;
                    scenarioDetails.Scenario_Name = scenarioName;
                    scenarioDetails.Active = 1;
                    scenarioDetails.Created_Date = DateTime.Now;
                    scenarioDetails.IsDefault = 0;
                    scenarioDetails.Category_ID = Convert.ToInt32(((JValue)categoryid).Value);
                    scenarioDetails.Number_Of_Elements = Convert.ToInt32(((JValue)noofelements).Value);
                    scenarioDetails.Scenario_Node_details = listNodeDetail;
                    scenarioDetails.Scenario_Connection_Details = listConnDetail;
                }
                int isSaved= this.Iscenario.Create(scenarioDetails);
                if (isSaved == 1)
                {
                    TempData["SuccessMessage"] = "Scenario submitted successfully.";
                    ModelState.Clear();
                }
                else
                {
                    TempData["DuplicateMessage"] = "Scenario submission failed";
                }
                return Json(isSaved, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}