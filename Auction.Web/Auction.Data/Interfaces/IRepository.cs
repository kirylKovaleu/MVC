using System;
using System.Collections.Generic;

namespace Auction.Data.Interfaces
{
    public interface IRepository
    {
        IEnumerable<T> GetAll<T>() where T : class, IEntity;

        T Get<T>(Guid id) where T : class, IEntity;
        void Add<T>(T item) where T : class, IEntity;
        void Update<T>(T item) where T : class, IEntity;
        void Delete<T>(Guid id) where T : class, IEntity;
        void Delete<T>(IEnumerable<T> items) where T : class, IEntity;
        void Add<T>(IEnumerable<T> items) where T : class, IEntity;
    }
}
