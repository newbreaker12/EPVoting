using voting_data_access.Repositories.Implementation;
using voting_data_access.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace voting_api.Configuration
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), (typeof(Repository<>)));
            services.AddScoped<IAppointmentRepository, VoteRepository>();
            services.AddScoped<IVotingArticleRepository, VotingArticleRepository>();
            services.AddScoped<IVotingGroupsRepository, VotingGroupsRepository>();
            services.AddScoped<IVotingRolesRepository, VotingRolesRepository>();
            services.AddScoped<IVotingSessionRepository, VotingSessionRepository>();
            services.AddScoped<IVotingUsersRepository, VotingUsersRepository>();

            return services;
        }
    }
}
