using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using System.Collections.Generic;
using voting_models.Models;
using System;
using System.Net.Mail;
using System.Net;

namespace voting_bl.Service
{
    public class EmailsService
    {
        private readonly IUnitOfWork _unitOfWork;

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
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                UseDefaultCredentials = false,
                Port = 587,
                Credentials = new NetworkCredential("hospitalmanagercontact@gmail.com", "houhHOH421HOU!"),
                EnableSsl = true,
            };
            if (recipient != null && recipient != "")
            {
                smtpClient.Send("hospitalmanagercontact@gmail.com", recipient, subject, body);
            }
        }
    }
}
