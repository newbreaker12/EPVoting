using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using voting_bl.Service;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;

namespace voting_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            //AddGroups(host);
            //AddArticle(host);
            //AddSubArticle(host);
            //AddSession(host);
            //AddRoles(host);
            //AddUser(host);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });



        private static void AddUser(IHost host)
        {
            var scope = host.Services.CreateScope();
            DateTime now = DateTime.Now;
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            VotingUsersService _userService = new VotingUsersService(unitOfWork);
            _userService.SaveUsers(
                new VotingUsers
                {
                    Email = "psr007700@students.ephec.be",
                    FirstName = "Francesco",
                    LastName = "Bigi",
                    Password = "pss",
                    IsMEP = true,
                    RoleId = 1,
                    GroupId = 1

                });
            _userService.SaveUsers(
                new VotingUsers
                {
                    Email = "admin",
                    FirstName = "admin",
                    LastName = "admin",
                    Password = "pss",
                    IsMEP = true,
                    RoleId = 2

                });
        }

        private static void AddRoles(IHost host)
        {
            var scope = host.Services.CreateScope();
            DateTime now = DateTime.Now;
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            VotingRolesService _roleService = new VotingRolesService(unitOfWork);
            _roleService.SaveRoles(
                new VotingRoles
                {
                    Name = "MEP",
                    Description = "Member of European Parliament"

                });
            _roleService.SaveRoles(
                new VotingRoles
                {
                    Name = "ADMIN",
                    Description = "Administrator of the application"

                });
            _roleService.SaveRoles(
                new VotingRoles
                {
                    Name = "PG",
                    Description = "Political Groups"

                });
        }

        private static void AddGroups(IHost host)
        {
            var scope = host.Services.CreateScope();
            DateTime now = DateTime.Now;
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            VotingGroupsService _groupsService = new VotingGroupsService(unitOfWork);
            _groupsService.SaveGroups(
                new VotingGroups
                {
                    Name = "BUDGET",
                    ReadableId = "BG",
                    CreatedAt = new DateTime(now.Year, now.Month, 1, 7, 0, 0)

                });
        }

        private static void AddArticle(IHost host)
        {
            var scope = host.Services.CreateScope();
            DateTime now = DateTime.Now;
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            VotingArticleService _articleService = new VotingArticleService(unitOfWork);
            _articleService.SaveArticle(
                new VotingArticle
                {
                    GroupsId = 1,
                    Name = "BG",
                    Description = "Article n.7",
                    CreatedAt = new DateTime(now.Year, now.Month, 1, 7, 0, 0)
                });
            _articleService.SaveArticle(
                new VotingArticle
                {
                    GroupsId = 1,
                    Name = "CT",
                    Description = "Article n.52237",
                    CreatedAt = new DateTime(now.Year, now.Month, 1, 4, 0, 0)
                });
        }

        private static void AddSubArticle(IHost host)
        {
            var scope = host.Services.CreateScope();
            DateTime now = DateTime.Now;
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            VotingSubArticleService _articleService = new VotingSubArticleService(unitOfWork);
            _articleService.SaveSubArticle(
                new VotingSubArticle
                {
                    ArticleId = 1,
                    Name = "BG",
                    Description = "Article n.71",
                    CreatedAt = new DateTime(now.Year, now.Month, 1, 7, 0, 0)
                });
            _articleService.SaveSubArticle(
                new VotingSubArticle
                {
                    ArticleId = 1,
                    Name = "GUGU",
                    Description = "Article n.7",
                    CreatedAt = new DateTime(now.Year, now.Month, 1, 7, 0, 0)
                });
            _articleService.SaveSubArticle(
                new VotingSubArticle
                {
                    ArticleId = 1,
                    Name = "CT",
                    Description = "Article n.52237",
                    CreatedAt = new DateTime(now.Year, now.Month, 1, 4, 0, 0)
                });
            _articleService.SaveSubArticle(
                new VotingSubArticle
                {
                    ArticleId = 1,
                    Name = "UHU",
                    Description = "Article n.222",
                    CreatedAt = new DateTime(now.Year, now.Month, 1, 4, 0, 0)
                });
            _articleService.SaveSubArticle(
                new VotingSubArticle
                {
                    ArticleId = 1,
                    Name = "JH",
                    Description = "Article n.256",
                    CreatedAt = new DateTime(now.Year, now.Month, 1, 4, 0, 0)
                });
        }
        private static void AddSession(IHost host)
        {
            var scope = host.Services.CreateScope();
            DateTime now = DateTime.Now;
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            VotingSessionService _sessionService = new VotingSessionService(unitOfWork);
            _sessionService.SaveSession(
                new VotingSession
                {
                    ArticleId = 1,
                    Name = "BG",
                    Description = "Transfers Other sections (single votes)Draft amending budget(2021 Brexit Adjustment Reserve)(3 AMs)",
                    From = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0),
                    To = new DateTime(now.Year, now.Month, now.Day + 1, 23, 59, 0)
                });
        }
    }

}