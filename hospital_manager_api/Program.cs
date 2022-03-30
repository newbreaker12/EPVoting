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

            AddGroups(host);
            AddArticle(host);
            AddSubArticle(host);
            AddSession(host);
            AddRoles(host);
            AddUser(host);
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
                    Id = 1,
                    Email = "psr007700@students.ephec.be",
                    FirstName = "Francesco",
                    LastName = "Bigi",
                    Password = "pss",
                    IsMEP = true,
                    Roles = new List<UserToRole> {
                        new UserToRole
                        {
                            RoleId = 1
                        }
                    },
                    Groups = new List<UserToGroup> {
                        new UserToGroup
                        {
                            GroupId = 1
                        }
                    }

                });
            _userService.SaveUsers(
                new VotingUsers
                {
                    Id =2,
                    Email = "admin",
                    FirstName = "admin",
                    LastName = "admin",
                    Password = "pss",
                    IsMEP = true,
                    Roles = new List<UserToRole> {
                        new UserToRole
                        {
                            RoleId = 2
                        }
                    }

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
                    Id = 1,
                    Name = "MEP",
                    Description = "Member of European Parliament"

                });
            _roleService.SaveRoles(
                new VotingRoles
                {
                    Id = 2,
                    Name = "ADMIN",
                    Description = "Administrator of the application"

                });
            _roleService.SaveRoles(
                new VotingRoles
                {
                    Id = 3,
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
                    Id = 1,
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
                    Id = 1,
                    GroupsId = 1,
                    Name = "BG",
                    Description = "Article n.7",
                    CreatedAt = new DateTime(now.Year, now.Month, 1, 7, 0, 0)
                });
            _articleService.SaveArticle(
                new VotingArticle
                {
                    Id = 2,
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
                    Id = 1,
                    ArticleId = 1,
                    Name = "BG",
                    Description = "Article n.7",
                    CreatedAt = new DateTime(now.Year, now.Month, 1, 7, 0, 0)
                });
            _articleService.SaveSubArticle(
                new VotingSubArticle
                {
                    Id = 2,
                    ArticleId = 1,
                    Name = "BG",
                    Description = "Article n.7",
                    CreatedAt = new DateTime(now.Year, now.Month, 1, 7, 0, 0)
                });
            _articleService.SaveSubArticle(
                new VotingSubArticle
                {
                    Id = 3,
                    ArticleId = 1,
                    Name = "CT",
                    Description = "Article n.52237",
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
                    Id = 1,
                    ArticleId = 1,
                    Name = "BG",
                    Description = "Transfers Other sections (single votes)Draft amending budget(2021 Brexit Adjustment Reserve)(3 AMs)",
                    From = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0),
                    To = new DateTime(now.Year, now.Month, now.Day + 1, 23, 59, 0)
                });
        }
    }

}