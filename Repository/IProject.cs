using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WroseModel.Models;
using WroseModel.View_Model;

namespace WroseModel.Repository
{
    public interface IProject
    {
        List<SelectListItem> GetProvienceList();
        IEnumerable<SelectListItem> GetMunicipalityList(int ProvienceId);
        int Create(ProjectVM objProject);
        int GetUserId(string useremail);
        List<Project_Details> GetProjectListByUser(int userID);
        List<SelectListItem> GetWasteMatList();
        int SaveWasteMatDetails(List<Waste_Mat_Details> wasteMatsDetails);
        int GetWasteMatIdByName(string wasteMatname);
        int InsertWastMatMaster(Waste_Mat_Master wasteMatMaster);
        Project_Details GetProjectById(int id);
        int EditProject(ProjectVM objProject);
    }
}
