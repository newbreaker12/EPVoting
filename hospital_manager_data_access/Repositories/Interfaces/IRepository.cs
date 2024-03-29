﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace voting_data_access.Repositories.Interfaces
{
    public interface IRepository<T> : IDisposable where T : class
    {
        // The CRUD basics methods !!!
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        T Get(long id);
        IEnumerable<T> All();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    }
}