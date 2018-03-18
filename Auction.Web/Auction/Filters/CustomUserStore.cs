using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Auction.Models;
using System.Security.Claims;
using Auction.Business.Services.Interfaces;
using Autofac;
using System.Configuration;
using System.Globalization;
using AutoMapper;
using System.Web.Routing;

namespace Auction.Filters
{
    public class CustomUserStore : IUserStore<LoginUserModel>, IUserPasswordStore<LoginUserModel>, IUserClaimStore<LoginUserModel>
    {
        private IUserService _userService;
        private IRoles _iRole;
        private string _path;
        private readonly IComponentContext _context;

        public CustomUserStore(IComponentContext context)
        {
            _context = context;
            _path = ConfigurationManager.AppSettings["JsonRepositoryPath"];
            _userService = _context.Resolve<IUserService>(new NamedParameter("path", _path));
            _iRole = _context.Resolve<IRoles>(new NamedParameter("path", _path));
        }

        public Task AddClaimAsync(LoginUserModel user, Claim claim)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(LoginUserModel user)
        {
            user.RegistrionDate = DateTime.Now;
            user.TZone = TimeZoneInfo.Utc.BaseUtcOffset.ToString();
            _userService.addUser(Mapper.Map<Business.Entities.UserDTO>(user));

            return Task.CompletedTask;
        }

        public Task DeleteAsync(LoginUserModel user)
        {
            _userService.deleteUser(Guid.Parse(user.Id));
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }

        public Task<LoginUserModel> FindByIdAsync(string userId)
        {
            var user = _userService.user(Guid.Parse(userId));
            return Task.FromResult(Mapper.Map<LoginUserModel>(user));
        }

        public Task<LoginUserModel> FindByNameAsync(string userName)
        {
            var user = _userService.GetUsers().ToList().Find(x => x.Login.Equals(userName,StringComparison.OrdinalIgnoreCase));
            if(user == null)
            {
                LoginUserModel newUser = null;
                return Task.FromResult(newUser);
            }
            return Task.FromResult(new LoginUserModel { Id = user.Id.ToString(), UserName = user.Login });
        }

        public Task<IList<Claim>> GetClaimsAsync(LoginUserModel user)
        {
            var roles = _iRole.GetUserRoles(new Guid(user.Id));
            IList<Claim> claims = roles.Select(x => new Claim(ClaimTypes.Role, x.ToString())).ToList();
            return Task.FromResult(claims);
        }

        public Task<string> GetPasswordHashAsync(LoginUserModel user)
        {
            var newUser = _userService.user(Guid.Parse(user.Id));
            var pas = new PasswordHasher().HashPassword(newUser.Password);
            return Task.FromResult(new PasswordHasher().HashPassword(newUser.Password));
        }

        public Task<bool> HasPasswordAsync(LoginUserModel user)
        {
            return Task.FromResult(true);
        }

        public Task RemoveClaimAsync(LoginUserModel user, Claim claim)
        {
            _userService.deleteUser(Guid.Parse(user.Id));
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(LoginUserModel user, string passwordHash)
        {
            user.Password = new PasswordHasher().HashPassword(passwordHash);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(LoginUserModel user)
        {
            _userService.Update(Mapper.Map<Business.Entities.UserDTO>(user));
            return Task.CompletedTask;
        }
       
    }
}