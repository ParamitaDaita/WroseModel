using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WroseModel.Models;
using WroseModel.View_Model;

namespace WroseModel.Repository
{
    public class RegistrationRepository : IRegistration
    {
        private WROSEDBEntities DBcontext;
        private Models.User_Registration user_Registration;
        public RegistrationRepository(WROSEDBEntities objregistrationcontext)
        {
            this.DBcontext = objregistrationcontext;
            this.user_Registration = new User_Registration();
        }
        public int NewRegistration(RegistrationVM objRegistration)
        {
            int isSaved = 0;
            try
            {
                this.user_Registration.First_Name = objRegistration.First_Name;
                this.user_Registration.Last_Name = objRegistration.Last_Name;
                this.user_Registration.User_Email = objRegistration.User_Email;
                this.user_Registration.User_Mobile = objRegistration.User_Mobile;
                this.user_Registration.User_Password = objRegistration.User_Password;
                this.user_Registration.Active=objRegistration.Active;
                this.user_Registration.User_Role_ID=objRegistration.User_Role_ID;
                this.user_Registration.Created_Date=objRegistration.Created_Date;
                DBcontext.User_Registration.Add(user_Registration);
                DBcontext.SaveChanges();
                return isSaved = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int UpdatePassword(string useremail,string newpassword)
        {
            int isUpdated = 0;
            try
            {
                User_Registration userRegistration = (from x in DBcontext.User_Registration
                              where x.User_Email == useremail
                                                      select x).First();
                userRegistration.User_Password= newpassword;
                userRegistration.Modified_Date = DateTime.Now;
                DBcontext.SaveChanges();
                return isUpdated = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetUserByEmail(string email)
        {
            string useremail = string.Empty;
            try {
                var user = DBcontext.User_Registration
                   .Where(b => b.User_Email == email)
                   .FirstOrDefault();
                if (user != null)
                {
                    useremail = user.User_Email.ToString();
                }
                return useremail;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public User_Registration GetUserDetailByEmail(string email)
        {
            try
            {
                var user = DBcontext.User_Registration
                   .Where(b => b.User_Email == email)
                   .FirstOrDefault();
                
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool GetUserLogin(string email,string enpassword)
        {
            bool isValid = false;
            try
            {
                var user = DBcontext.User_Registration
                   .Where(b => b.User_Email == email && b.User_Password== enpassword)
                   .FirstOrDefault();
                if (user != null)
                {
                    isValid = true;
                }
                return isValid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    DBcontext.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}