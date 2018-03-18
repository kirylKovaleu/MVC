using Auction.Business.Entities;
using Auction.Business.Infrastructure;
using Auction.Business.Services.Interfaces;
using Auction.Data.Entities;
using Auction.Data.Interfaces;
using Autofac;
using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Business.Services.Implementations
{
    public class UserService : IUserService
    {
        private string _path;
        IRepository _repository;
        private readonly IComponentContext _context;

        public UserService(string path, IComponentContext context)
        {
            _context = context;
            _path = path;
            _repository = _context.ResolveNamed<IRepository>("JSON", new NamedParameter("path", _path));
        }

        public void addUser(UserDTO user)
        {
            var userBusiness = Mapper.Map<User>(user);
            userBusiness.Id = Guid.NewGuid();
            userBusiness.Roles = new int[] { (int)Role.User }; 
            _repository.Add(userBusiness);
        }

        public void deleteUser(Guid id)
        {
            _repository.Delete<Data.Entities.User>(id);
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            var p = _repository.GetAll<Data.Entities.User>();

            return Mapper.Map<List<UserDTO>>(p);
        }

        public UserDTO user(Guid id)
        {
            var us = _repository.Get<Data.Entities.User>(id);
            return Mapper.Map<UserDTO>(us);
        }

        public Guid GetUserId(string name)
        {
            return _repository.GetAll<Data.Entities.User>().ToList().Find(x => x.Login.Equals(name)).Id;
        }

        public  Task SetInitialDataAsync()
        {
            _repository.Add(new User
            {
                Id = Guid.NewGuid(),
                Login = "Admin",
                LastName = "Кирилл",
                FirstName = "Ковалев",
                Password = "Admin",
                RegistrionDate = new DateTime(2016, 05, 12),
                Locale = "ru",
                TZone = "03:00:00",
                Roles = new int[] { (int)Role.Admin }
            });

            _repository.Add(new User
            {
                Id = Guid.NewGuid(),
                Login = "Manager",
                LastName = "Вася",
                FirstName = "Пупкин",
                Password = "Manager",
                RegistrionDate = new DateTime(2016, 05, 12),
                Locale = "en",
                TZone = "05:00:00",
                Roles = new int[] { (int)Role.Manager }
                
            });

            _repository.Add(new User
            {
                Id = Guid.NewGuid(),
                Login = "User",
                LastName = "Вася",
                FirstName = "Пупкин",
                Password = "User",
                RegistrionDate = new DateTime(2016, 05, 12),
                Locale = "br",
                TZone = "-02:00:00",
                Roles = new int[] { (int)Role.User }

            });
            return Task.CompletedTask;
        }

        public void Update(UserDTO item)
        {
            var result = _repository.GetAll<Data.Entities.User>().ToList().Find(X => X.Id.Equals(item.Id));
            var user = Mapper.Map<Data.Entities.User>(item);
            user.Roles = result.Roles;
            _repository.Update(user);
        }
    }
}
