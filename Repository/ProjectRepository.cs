using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WroseModel.Models;
using WroseModel.View_Model;

namespace WroseModel.Repository
{
    public class ProjectRepository : IProject
    {
        private WROSEDBEntities DBcontext;
        private Models.Project_Details project_Details;
        public ProjectRepository(WROSEDBEntities objprojectcontext)
        {
            this.DBcontext = objprojectcontext;
            this.project_Details = new Project_Details();
        }
        public List<SelectListItem> GetProvienceList()
        {
            List<SelectListItem> provienceList;
            try
            {
                provienceList = new SelectList(DBcontext.Provience_Master.Where(n=>n.Active==1).ToList(), "ID", "Provience_Name").ToList();
                return provienceList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> GetWasteMatList()
        {
            List<SelectListItem> wasteMatList;
            try
            {
                wasteMatList = new SelectList(DBcontext.Waste_Mat_Master.Where(n=>n.Active==1).OrderBy(n => n.Waste_Mat_Name).ToList(), "ID", "Waste_Mat_Name").ToList();
                return wasteMatList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Project_Details> GetProjectListByUser(int userID)
        {
            List<Project_Details> projectList;
            try
            {
                projectList = new List<Project_Details>();
                projectList = (DBcontext.Project_Details.Where(n=>n.User_Reg_ID== userID).Where(n=>n.Active==1).OrderByDescending(n=>n.ID).ToList());
                return projectList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Project_Details GetProjectById(int id)
        {
            Project_Details projectDetails;
            try
            {
                projectDetails = new Project_Details();
                projectDetails = DBcontext.Project_Details.Where(n => n.ID== id).Where(n => n.Active == 1).FirstOrDefault();
                return projectDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<SelectListItem> GetMunicipalityList(int ProvienceId)
        {
            IEnumerable<SelectListItem> municipalityList;
            try
            {
                municipalityList = new SelectList(DBcontext.Municipality_Master.Where(x => x.Provience_Master_ID == ProvienceId).Where(n=>n.Active==1).ToList(), "ID", "Municipality_Name").ToList();
                return municipalityList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Create(ProjectVM objProject)
        {
            int isSaved = 0;
            try
            {
                this.project_Details.Project_Name = objProject.Project_Name;
                this.project_Details.Project_Desc = objProject.Project_Desc;
                this.project_Details.Study_Year = objProject.Study_Year;
                this.project_Details.Author_company = objProject.Author_company;
                this.project_Details.Author_Name = objProject.Author_Name;
                this.project_Details.Client_Name = objProject.Client_Name;
                this.project_Details.Local_Auth_Study = objProject.Local_Auth_Study;
                this.project_Details.Stusy_Purpose = objProject.Stusy_Purpose;
                this.project_Details.Key_Assumption = objProject.Key_Assumption;
                this.project_Details.Study_Limitation = objProject.Study_Limitation;
                this.project_Details.User_Reg_ID = objProject.User_Reg_ID;
                this.project_Details.Active = objProject.Active;
                this.project_Details.Created_Date = objProject.Created_Date;
                DBcontext.Project_Details.Add(this.project_Details);
                DBcontext.SaveChanges();
                return isSaved = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int EditProject(ProjectVM objProject)
        {
            int isSaved = 0;
            try
            {
                this.project_Details.Project_Name = objProject.Project_Name;
                this.project_Details.Project_Desc = objProject.Project_Desc;
                this.project_Details.Study_Year = objProject.Study_Year;
                this.project_Details.Author_company = objProject.Author_company;
                this.project_Details.Author_Name = objProject.Author_Name;
                this.project_Details.Client_Name = objProject.Client_Name;
                this.project_Details.Local_Auth_Study = objProject.Local_Auth_Study;
                this.project_Details.Stusy_Purpose = objProject.Stusy_Purpose;
                this.project_Details.Key_Assumption = objProject.Key_Assumption;
                this.project_Details.Study_Limitation = objProject.Study_Limitation;
                this.project_Details.User_Reg_ID = objProject.User_Reg_ID;
                this.project_Details.Active = objProject.Active;
                this.project_Details.Modified_Date = objProject.Modified_Date;
                this.project_Details.Created_Date = objProject.Created_Date;
                this.project_Details.ID = objProject.ID;
                DBcontext.Entry(project_Details).State = System.Data.Entity.EntityState.Modified;
                DBcontext.SaveChanges();
                return isSaved = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetUserId(string useremail)
        {
            int userId = 0;
            try
            {
                userId=DBcontext.User_Registration.Where(x => x.User_Email == useremail).FirstOrDefault().ID;
                return userId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int SaveWasteMatDetails(List<Waste_Mat_Details> wasteMatsDetails)
        {
            int isSaved = 0;
            try
            {
                DBcontext.Waste_Mat_Details.AddRange(wasteMatsDetails);
                DBcontext.SaveChanges();
                return isSaved = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertWastMatMaster(Waste_Mat_Master wasteMatMaster)
        {
            int isSaved = 0;
            try
            {
                DBcontext.Waste_Mat_Master.Add(wasteMatMaster);
                DBcontext.SaveChanges();
                return isSaved = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetWasteMatIdByName(string wasteMatname)
        {
            int wasteMatId = 0;
            try
            {
                var wastemat = DBcontext.Waste_Mat_Master
                   .Where(b => b.Waste_Mat_Name == wasteMatname)
                   .FirstOrDefault();
                if (wastemat != null)
                {
                    wasteMatId = wastemat.ID;
                }
                return wasteMatId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}