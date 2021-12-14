using System;

namespace voting_data_access.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IVoteRepository Vote { get; }
        IVotingArticleRepository votingArticle { get; }
        IVotingGroupsRepository VotingGroups { get; }
        IVotingRolesRepository VotingRoles { get; }
        IVotingSessionRepository VotingSession { get; }
        IVotingUsersRepository VotingUsers { get; }
        int Save();
    }
}