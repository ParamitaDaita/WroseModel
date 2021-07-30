using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WroseModel.Models;
using WroseModel.View_Model;

namespace WroseModel.Repository
{
    public interface IRegistration
    {
        int NewRegistration(RegistrationVM objRegistration);
        string GetUserByEmail(string email);
        bool GetUserLogin(string email, string enpassword);
        User_Registration GetUserDetailByEmail(string email);
        int UpdatePassword(string useremail, string newpassword);
        
    }
}