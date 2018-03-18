using Auction.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Business.Services.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetCategorys();
        Category category(Guid id);
        void addCategory(Category category);
        void deleteCategory(Guid id);
        string getDiscription(Guid id);
        Guid GetCategoryId(string name);
        void Update(Category item);
    }
}
