using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace CatchMe.Helpers
{
  
         public class Emailer
    {
        #region members

        public string SmtpHost { get; set; }
        public MailAddress SenderMail { get; set; }
        // private MailAddress RecipientMail { get; set; }
        private List<MailAddress> RecipientMails { get; set; }
        private List<MailAddress> CcRecipientMails { get; set; }
        private List<MailAddress> BccRecipientMails { get; set; }

        private List<Attachment> MailAttachments { get; set; }



        //private List<Attachment> Attachments { get; set; }
        private Attachment Attachment { get; set; }

        public string Body { get; set; }

        #endregion

        #region constructors

       



        public Emailer()
        {
            SmtpHost = ConfigurationHelper.GetSmtpHostDns();
            //SenderMail = new MailAddress(@"FinancialHealthCheck@mcb.local");

            RecipientMails = new List<MailAddress>();
            CcRecipientMails = new List<MailAddress>();
            BccRecipientMails = new List<MailAddress>();
            MailAttachments = new List<Attachment>();



        }







        #endregion


        #region methods

        public void SetRecipients(string recipients)
        {
            try
            {
                var mails = recipients.Split(';');
                foreach (var email in mails)
                {
                    if (email != "")
                    {
                        AddRecipient(email);
                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public void AddRecipient(string email)
        {

            RecipientMails.Add(new MailAddress(email));

        }

        public void AddAttachment(Attachment attachment)
        {
            MailAttachments.Add(attachment);
        }



        public void AddCcRecipient(string email)
        {

            CcRecipientMails.Add(new MailAddress(email));

        }

        public void AddBccRecipient(string email)
        {

            BccRecipientMails.Add(new MailAddress(email));

        }

        public void SendMail(String Subject, String Body, bool isHtml)
        {
            try
            {
                using (SmtpClient client = new SmtpClient(SmtpHost))
                {
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //MailMessage message = new MailMessage(SenderMail, RecipientMails.FirstOrDefault());

                    MailMessage message = new MailMessage();
                    foreach (var email in RecipientMails)
                    {
                        message.To.Add(email);
                    }

                    foreach (var email in CcRecipientMails)
                    {
                        message.CC.Add(email);
                    }

                    foreach (var email in BccRecipientMails)
                    {
                        message.Bcc.Add(email);
                    }


                    foreach (var attachment in MailAttachments)
                    {
                        message.Attachments.Add(attachment);
                    }


                    message.From = SenderMail;



                    message.Body = Body;
                    message.IsBodyHtml = isHtml;
                    message.Subject = Subject;



                    if (Helpers.ConfigurationHelper.SendEmail())
                        client.Send(message);
                }
            }
            catch (Exception e)
            {

                throw e;
            }

        }



        #endregion

    }
}
    
