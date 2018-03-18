using Auction.Business.Entities;
using Auction.Business.Services.Interfaces;
using Auction.Data.Entities;
using Auction.Data.Interfaces;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Business.Services.Implementations
{
    public class RoleService:IRoles
    {
        private string _path;
        IRepository _repository;
        private readonly IComponentContext _context;

        public RoleService(string path, IComponentContext context)
        {
            _context = context;
            _path = path;
            _repository = _context.ResolveNamed<IRepository>("JSON", new NamedParameter("path", _path));
        }

        public void AddUserToRole(Guid userId, Role role, Guid categoryId)
        {
            var user = _repository.Get<User>(userId);

            var roles = new List<int>(user.Roles);
            roles.Clear();
            roles.Add((int)role);
            user.Roles = roles.ToArray();

            _repository.Update(user);
        }

        public IEnumerable<Role> GetUserRoles(Guid userId)
        {
            var user = _repository.Get<User>(userId);
            return user.Roles.Select(r => (Role)r);
        }
    }
}
