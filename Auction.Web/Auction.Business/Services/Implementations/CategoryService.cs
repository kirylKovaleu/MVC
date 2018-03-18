using Auction.Business.Entities;
using Auction.Business.Services.Interfaces;
using Auction.Data.Interfaces;
using Autofac;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Business.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private string _path;
        IRepository _repository;
        private readonly IComponentContext _context;

        public CategoryService(string type, string path, IComponentContext context)
        {
            if (type == "JSON")
            {
                _context = context;
                _repository = _context.ResolveNamed<IRepository>(type, new NamedParameter("path", path));
            }
            else
            {
                _context = context;
                _repository = _context.ResolveNamed<IRepository>(type, new NamedParameter("path", path));
            }
        }

        public void addCategory(Category category)
        {
            category.Id = Guid.NewGuid();
            _repository.Add(Mapper.Map<Data.Entities.Category>(category));
        }

        public Category category(Guid id)
        {
            var c = _repository.Get<Data.Entities.Category>(id);
            Category categ = Mapper.Map<Category>(c);
            return categ;
        }

        public Guid GetCategoryId(string name)
        {
            if (name == null)
            {
                name = "Без категории";
            }
            var categorys = _repository.GetAll<Data.Entities.Category>().ToList().Find(x=>x.Name.Equals(name));
            return categorys.Id;
        }

        public void deleteCategory(Guid id)
        {
            _repository.Delete<Data.Entities.Category>(id);
        }

        public IEnumerable<Category> GetCategorys()
        {
            var categorys = _repository.GetAll<Data.Entities.Category>();
            return Mapper.Map<List<Category>>(categorys);
        }

        public string getDiscription(Guid id)
        {
            var categ = _repository.Get<Data.Entities.Category>(id);
            Category discription = Mapper.Map<Category>(categ);
            return discription.Discription;
        }

        public void Update(Category item)
        {
            var category = Mapper.Map<Data.Entities.Category>(item);
            _repository.Update(category);
        }
    }
}
