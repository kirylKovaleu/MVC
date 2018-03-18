using Auction.Data.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Auction.Data.Implements
{
    public class JsonRepository : IRepository
    {

        private string _path;
        private static object _lock = new object();

        public JsonRepository(string path)
        {
            _path = path;
        }

        public void Add<T>(IEnumerable<T> items) where T : class, IEntity
        {
            List<T> list = GetAll<T>().ToList();
            list.AddRange(items);
            Save(list);
        }

        public void Add<T>(T item) where T : class, IEntity
        { 


            List<T> list = GetAll<T>().ToList();
            list.Add(item);
            Save(list);
        }

        public void Delete<T>(IEnumerable<T> items) where T : class, IEntity
        {
            List<T> list = GetAll<T>().ToList();
            list = list.Except(items).ToList();
            Save(list);
        }

        public void Delete<T>(Guid id) where T : class, IEntity
        {
            List<T> list = GetAll<T>().ToList();
            list.RemoveAll(x => x.Id.Equals(id));
            Save(list);
        }

        public T Get<T>(Guid id) where T : class, IEntity
        {
            return GetAll<T>().ToList().Find(x => x.Id.Equals(id));
        }

        public IEnumerable<T> GetAll<T>() where T : class, IEntity
        {
            List<T> items = new List<T>();
            lock (_lock)
            {
                var fullPath = Path.Combine(_path, string.Format("{0}.json",typeof(T).Name));
                if (File.Exists(fullPath))
                {
                    using (StreamReader sr = File.OpenText(fullPath))
                    {
                        string data = sr.ReadToEnd();
                        if (!string.IsNullOrEmpty(data))
                        {
                            items = Deserialize<List<T>>(data);
                        }
                    }
                }
            }

            return items;
        }

        private void Save<T>(IEnumerable<T> items) where T : IEntity
        {
            lock (_lock)
            {
                var list = items.ToList();
                var fullPath = Path.Combine(_path, string.Format("{0}.json", typeof(T).Name));
                using (StreamWriter sw = new StreamWriter(fullPath, false))
                {
                    sw.Write(Serialize(list));
                }
            }
        }

        public void Update<T>(T item) where T : class, IEntity
        {
            List<T> list = GetAll<T>().ToList();
            int index = list.FindIndex(x => x.Id.Equals(item.Id));
            list.RemoveAt(index);
            list.Insert(index, item);
            Save(list);
        }

        private T Deserialize<T>(string value)
        {
            T result = JsonConvert.DeserializeObject<T>(value);
            return result;
        }

        private string Serialize<T>(T value)
        {
            string result = JsonConvert.SerializeObject(value);
            return result;
        }
    }
}
