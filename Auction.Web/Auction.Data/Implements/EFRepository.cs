using Auction.Data.EF;
using Auction.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Auction.Data.Implements
{
    public class EFRepository : IRepository
    {
        AuctionContext db;

        public EFRepository(string path)
        {
            db = new AuctionContext(path);
        }
        public void Add<T>(IEnumerable<T> items) where T : class, IEntity
        {
            db.Set<T>().AddRange(items);
        }

        public void Add<T>(T item) where T : class, IEntity
        {
            db.Set<T>().Add(item);
            db.SaveChanges();
        }

        public void Delete<T>(IEnumerable<T> items) where T : class, IEntity
        {
            db.Set<T>().RemoveRange(items);
            db.SaveChanges();
        }

        public void Delete<T>(Guid id) where T : class, IEntity
        {
            var item = db.Set<T>().Find(id);
            db.Set<T>().Remove(item);
            db.SaveChanges();
        }

        public T Get<T>(Guid id) where T : class, IEntity
        {
            return db.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll<T>() where T : class, IEntity
        {
            return db.Set<T>();
        }

        public void Update<T>(T item) where T : class, IEntity
        {
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
