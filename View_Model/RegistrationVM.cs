using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WroseModel.View_Model
{
    public class RegistrationVM
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "First name is required")]
        [RegularExpression("^[A-Za-z]*$", ErrorMessage = "First name must be alphabet only.")]
        public string First_Name { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [RegularExpression("^[A-Za-z]*$", ErrorMessage = "Last name must be alphabet only.")]
        public string Last_Name { get; set; }
        [Required(ErrorMessage = "Email address is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid email address.")]
        public string User_Email { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid phone number.")]
        public string User_Mobile { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[A-Z]).{6,}", ErrorMessage = "Password must contain one uppercase letter, and at least 6 characters")]
        public string User_Password { get; set; }
        [Required(ErrorMessage = "Confirm password is required")]
        [RegularExpression(@"^(?=.*[A-Z]).{6,}", ErrorMessage = "Confirm password must contain one uppercase letter, and at least 6 characters")]
        [Compare("User_Password",ErrorMessage = "Password doesn't match.")]
        public string User_ConfirmPassword { get; set; }
        [Required(ErrorMessage = "New password is required")]
        [RegularExpression(@"^(?=.*[A-Z]).{6,}", ErrorMessage = "New password must contain one uppercase letter, and at least 6 characters")]
        public string User_NewPassword { get; set; }
        public byte Active { get; set; }
        public int User_Role_ID { get; set; }
        [Display(Name = "Remember Me")]
        public bool Rememberme { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "OTP is requierd")]
        public string OTP { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    }
}