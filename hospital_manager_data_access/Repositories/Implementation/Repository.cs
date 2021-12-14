using voting_data_access.Data;
using voting_data_access.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace voting_data_access.Repositories.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly VotingDbContext Db;
        protected readonly DbSet<T> DbSet;

        public Repository(VotingDbContext db)
        {
            Db = db;
            DbSet = db.Set<T>();
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public T Get(long id)
        {
            return DbSet.Find(id);
        }

        public IEnumerable<T> All()
        {
            return DbSet.ToList();
        }

        public void Remove(T entity)
        {
            DbSet.Remove(entity);
        }

        public void Update(T entity)
        {
            DbSet.Attach(entity);
            Db.Entry(entity).State = EntityState.Modified;
        }

        public void Dispose()
        {
            Db?.Dispose();
        }
    }
}