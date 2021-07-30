using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace WroseModel.Helper
{
    public class EmailHelper
    {
        string smtpClient = ConfigurationManager.AppSettings["smtpClient"];
        string smtpPort = ConfigurationManager.AppSettings["smtpPort"];
        string fromEmailAddress = ConfigurationManager.AppSettings["fromEmailAddress"];
        string fromEmailPassword = ConfigurationManager.AppSettings["fromEmailPassword"];
        //public void SendEmailToUser(string emailId)
        //{
        //   // var GenarateUserVerificationLink = "/Register/UserVerification/" + activationCode;
        // //   var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, GenarateUserVerificationLink);

        //    var fromMail = new MailAddress("paramitabanerjee57@gmail.com", "System"); // set your email    
        //    var fromEmailpassword = "#basko#2891"; // Set your password     
        //    //var toEmail = new MailAddress(emailId);
        //    var toEmail = new MailAddress("paramita.banerjee24@gmail.com");
        //    var smtp = new SmtpClient();
        //    smtp.Host = "smtp.gmail.com";
        //    smtp.Port = 587;

        //    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    smtp.UseDefaultCredentials = false;
        //    smtp.Credentials = new NetworkCredential(fromMail.Address, fromEmailpassword);
        //    smtp.EnableSsl = true;
        //    var Message = new MailMessage(fromMail, toEmail);
        //    Message.Subject = "Registration completed sucessfully";
        //    Message.Body = "<br/> Your registration completed succesfully.";
        //    Message.IsBodyHtml = true;
        //    smtp.Send(Message);
        //}
        //string _sender = "";
        //string _password = "";
        //public EmailHelper(string sender, string password)
        //{
        //    _sender = sender;
        //    _password = password;
        //}

        public void SendMail(string recipient,string firstname)
        {
            SmtpClient client = new SmtpClient(smtpClient);

            client.Port = Convert.ToInt32(smtpPort);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            System.Net.NetworkCredential credentials =
            new System.Net.NetworkCredential(fromEmailAddress, fromEmailPassword);
            client.EnableSsl = true;
            client.Credentials = credentials;

            try
            {
                var mail = new MailMessage(fromEmailAddress.Trim(), recipient.Trim());
                mail.Subject = "Registration successful"; 
                mail.Body = "Dear "+firstname+",<br/><br/>Congratulation!<br/><br/>You have registered succesfully.<br/><br/><br/><br/>Thank you.<br/>Test mail.";
                mail.IsBodyHtml = true;
                client.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }
        public void ForgetPasswordEmailToUser(string useremail, string activationCode)
        {
            SmtpClient client = new SmtpClient(smtpClient);
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            System.Net.NetworkCredential credentials =
            new System.Net.NetworkCredential(fromEmailAddress, fromEmailPassword);
            client.EnableSsl = true;
            client.Credentials = credentials;

            var generateVerificationLink = "/UserRegistration/ChangePassword/" + activationCode;
            var link = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, generateVerificationLink);
            try
            {
                var mail = new MailMessage(fromEmailAddress.Trim(), useremail.Trim());
                mail.Subject = "Reset Password Url";
                mail.Body = "<br/>Dear Member," +
                    "<br/>As per your request, we have sent you the password reset link.Click on the following link to reset your password: " +
                    "<br/><br/><a href="+link+">"+link+"</a>";
                mail.IsBodyHtml = true;
                client.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }
    }
}