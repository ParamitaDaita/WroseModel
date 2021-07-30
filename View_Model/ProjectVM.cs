using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WroseModel.View_Model
{
    public class ProjectVM
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Project name is required")]
        public string Project_Name { get; set; }
        [Required(ErrorMessage = "Study year is required")]
        public int Study_Year { get; set; }
        [Required(ErrorMessage = "Client name is required")]
        public string Client_Name { get; set; }
        [Required(ErrorMessage = "Author name is required")]
        public string Author_Name { get; set; }
        [Required(ErrorMessage = "Author company is required")]
        public string Author_company { get; set; }
        [Required(ErrorMessage = "Local authority study is required")]
        public string Local_Auth_Study { get; set; }
        [Required(ErrorMessage = "Project description is required")]
        public string Project_Desc { get; set; }
        [Required(ErrorMessage = "Purpose of study is required")]
        public string Stusy_Purpose { get; set; }
        [Required(ErrorMessage = "Key assumption is required")]
        public string Key_Assumption { get; set; }
        [Required(ErrorMessage = "Limitation of study is required")]
        public string Study_Limitation { get; set; }
        public int User_Reg_ID { get; set; }
        public byte Active { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public List<SelectListItem> Proviences { get; set; }
        public int ProvienceID { get; set; }
        public string Provience_Name { get; set; }
        public int MunicipalityID { get; set; }
        public string Municipality_Name { get; set; }
    }
}