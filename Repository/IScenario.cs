using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WroseModel.Models;

namespace WroseModel.Repository
{
    public interface IScenario
    {
        IEnumerable<SelectListItem> GetCategoryList();
        List<SelectListItem> GetProjectList();
        List<SelectListItem> GetWasteTechList();
        IEnumerable<SelectListItem> WasteTechListByCategory(int CategoryID);
        int Create(Scenario_Details objScenario);
        IEnumerable<SelectListItem> GetScenarioList();
        Scenario_Master ScenarioDetailsByScenarioID(int ScenarioID);
    }
}
