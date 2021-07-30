using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WroseModel.Models;

namespace WroseModel.Repository
{
    public class ScenarioRepository:IScenario
    {
        private WROSEDBEntities DBcontext;
        private Models.Scenario_Details scenario_Details;
        public ScenarioRepository(WROSEDBEntities objprojectcontext)
        {
            this.DBcontext = objprojectcontext;
            this.scenario_Details = new Scenario_Details();
        }
        public IEnumerable<SelectListItem> GetCategoryList()
        {
            IEnumerable<SelectListItem> CatregoryList;
            try
            {
                CatregoryList = new SelectList(DBcontext.Category_Master.Where(x => x.Active == 1).ToList(), "ID", "Category_Name").ToList();
                return CatregoryList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> GetProjectList()
        {
            List<SelectListItem> ProjectList;
            try
            {
                ProjectList = new SelectList(DBcontext.Project_Details.Where(x => x.Active == 1).ToList(), "ID", "Project_Name").ToList();
                return ProjectList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> GetWasteTechList()
        {
            List<SelectListItem> wasteTechList;
            try
            {
                wasteTechList = new SelectList(DBcontext.Waste_tech_Master.Where(x => x.Active == 1).ToList(), "ID", "Waste_Tech_Name").ToList();
                return wasteTechList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<SelectListItem> WasteTechListByCategory(int CategoryID)
        {
            IEnumerable<SelectListItem> wasteTechList;
            try
            {
                wasteTechList = new SelectList(DBcontext.Waste_tech_Master.Where(y=>y.Category_Master_ID==CategoryID).Where(x => x.Active == 1).ToList(), "ID", "Waste_Tech_Name").ToList();
                return wasteTechList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Create(Scenario_Details objScenario)
        {
            int isSaved = 0;
            try
            {
                DBcontext.Scenario_Details.Add(objScenario);
                DBcontext.SaveChanges();
                return isSaved = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<SelectListItem> GetScenarioList()
        {
            IEnumerable<SelectListItem> scenarioList;
            try
            {
                scenarioList = new SelectList(DBcontext.Scenario_Master.ToList(), "ID", "Scenario_Name").ToList();
                return scenarioList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Scenario_Master ScenarioDetailsByScenarioID(int ScenarioID)
        {
            Scenario_Master scenarioMaster;
            try
            {
                scenarioMaster = new Scenario_Master();
                scenarioMaster = DBcontext.Scenario_Master.Where(n => n.ID == ScenarioID).FirstOrDefault();
                return scenarioMaster;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}