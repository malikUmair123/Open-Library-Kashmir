using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Open_Library_Kashmir.CustomHelpers
{
    public static class CustomHelpers
    {
        public static IHtmlString Image(this HtmlHelper htmlHelper, string src, string alt)
        {
            TagBuilder imgTag = new TagBuilder("img");
            if (!string.IsNullOrEmpty(src))
            {
                imgTag.Attributes.Add("src", VirtualPathUtility.ToAbsolute(src));

            }
            else
            {
                imgTag.Attributes.Add("src", VirtualPathUtility.ToAbsolute("~/Content/Images/book_cover_na.jpeg"));

            }
            imgTag.Attributes.Add("alt", alt);
            return new MvcHtmlString(imgTag.ToString(TagRenderMode.SelfClosing));

        }

        public static bool EmailSend(string SenderEmail, string Subject, string Message, bool IsBodyHtml = false)
        {
            bool status = false;
            try
            {
                string HostAddress = ConfigurationManager.AppSettings["Host"].ToString();
                string FormEmailId = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string Password = ConfigurationManager.AppSettings["Password"].ToString();
                string Port = ConfigurationManager.AppSettings["Port"].ToString();
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(FormEmailId);
                mailMessage.Subject = Subject;
                mailMessage.Body = Message;
                mailMessage.IsBodyHtml = IsBodyHtml;
                mailMessage.To.Add(new MailAddress(SenderEmail));
                SmtpClient smtp = new SmtpClient
                {
                    Host = HostAddress,
                    EnableSsl = true
                };
                NetworkCredential networkCredential = new NetworkCredential
                {
                    UserName = mailMessage.From.Address,
                    Password = Password
                };
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = networkCredential;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Port = Convert.ToInt32(Port);
                smtp.Send(mailMessage);
                status = true;
                return status;
            }
            catch (Exception e)
            {
                return status;
            }
        }

    }
}