using Auction.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Business.Services.Interfaces
{
    public interface IRoles
    {
        IEnumerable<Role> GetUserRoles(Guid userId);
        void AddUserToRole(Guid userId, Role role, Guid categoryId);
    }
}
