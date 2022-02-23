using voting_data_access.Data;
using voting_data_access.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using System;

namespace voting_data_access.Repositories.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private VotingDbContext _context;
        private bool _disposed;

        public IVoteRepository Vote { get; private set; }
        public IVotingArticleRepository VotingArticle { get; private set; }
        public IVotingSubArticleRepository VotingSubArticle { get; private set; }
        public IVotingGroupsRepository VotingGroups { get; private set; }
        public IVotingRolesRepository VotingRoles { get; private set; }
        public IVotingSessionRepository VotingSession { get; private set; }
        public IVotingUsersRepository VotingUsers { get; private set; }

        public UnitOfWork(VotingDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            Vote = new VoteRepository(_context);
            VotingArticle = new VotingArticleRepository(_context);
            VotingSubArticle = new VotingSubArticleRepository(_context);
            VotingGroups = new VotingGroupsRepository(_context);
            VotingRoles = new VotingRolesRepository(_context);
            VotingSession = new VotingSessionRepository(_context);
            VotingUsers = new VotingUsersRepository(_context);
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!_disposed)
            {
                if(disposing && _context != null)
                {
                    _context.Dispose();
                    _context = null;
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}