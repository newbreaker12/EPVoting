using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using System.Collections.Generic;
using voting_models.Models;
using System;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace voting_bl.Service
{
    public class EmailsService
    {
        private readonly IUnitOfWork _unitOfWork;

        private string pss = "5GL4YVgTkMbxc0HW";
        private string from = "francesco.bigi.87@gmail.com";

        public EmailsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void SendEmails(long articleId)
        {
            VotingArticle votingArticle = _unitOfWork.VotingArticle.Get(articleId);
            List<VotingUsers> users = _unitOfWork.VotingUsers.GetByGroupId(votingArticle.GroupsId);
            foreach (VotingUsers user in users)
            {
                SendEmail(user.Email, "Vote session started", "vote for " + votingArticle.Description);
            }
        }

        public void SendEmail(string recipient, string subject, string body)
        {
            string template = EmailTemplates.createTemplate;
            template = template.Replace("::SUBJECT::", subject);
            template = template.Replace("::BODY::", body);

            var smtpClient = new SmtpClient()
            {
                Host = "smtp-relay.sendinblue.com",
                EnableSsl = true,
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from, pss),
            };
            if (recipient != null && recipient != "")
            {
                smtpClient.Send(from, recipient, subject, template);
            }
        }

        public void SendEmail(string recipient, string subject, string body, byte[] document)
        {
            Attachment attachment = new Attachment(new MemoryStream(document), "to_sign.pdf", "application/pdf");
            MailMessage mailMessage = new MailMessage();
            mailMessage.To.Add(new MailAddress(recipient));
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.From = new MailAddress(from);
            mailMessage.IsBodyHtml = false;
            mailMessage.Attachments.Add(attachment);

            SmtpClient smtp = new SmtpClient()
            {
                Host = "smtp-relay.sendinblue.com",
                EnableSsl = true,
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from, pss),
            };
            smtp.Send(mailMessage);
        }
    }
}
