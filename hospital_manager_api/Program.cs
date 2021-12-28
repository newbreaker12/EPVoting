using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using voting_bl.Service;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using voting_models.Models;

namespace voting_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            AddGroups(host);
            AddArticle(host);
            AddSession(host);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

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
                    ReadableId ="BG",
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
                    From = new DateTime(now.Year, now.Month, now.Day, 7, 0, 0),
                    To = new DateTime(now.Year, now.Month, now.Day + 1, 7, 0, 0)
                });
        }
    }

}