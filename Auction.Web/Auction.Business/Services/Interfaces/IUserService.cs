using Auction.Business.Entities;
using Auction.Business.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Business.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserDTO> GetUsers();
        void addUser(UserDTO user);
        void deleteUser(Guid id);
        UserDTO user(Guid id);
        Guid GetUserId(string name);
        Task SetInitialDataAsync();
        void Update(UserDTO item);
    }
}
