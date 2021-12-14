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

        public IAppointmentRepository Appointment { get; private set; }
        public IVotingArticleRepository Consultation { get; private set; }
        public IVotingGroupsRepository Doctor { get; private set; }
        public IVotingRolesRepository Hospital { get; private set; }
        public IVotingSessionRepository Room { get; private set; }
        public IVotingUsersRepository Speciality { get; private set; }

        public UnitOfWork(VotingDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            Appointment = new VoteRepository(_context);
            Consultation = new VotingArticleRepository(_context);
            Doctor = new VotingGroupsRepository(_context);
            Hospital = new VotingRolesRepository(_context);
            Room = new VotingSessionRepository(_context);
            Speciality = new VotingUsersRepository(_context);
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