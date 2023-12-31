﻿using Booking.Model;
using Booking.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Web.Repository.Interface
{
    public interface IUserRepository
    {
        bool IsValidUser(string email,string password);
        Task<int> GetUserId(string email);
        Task Create(CreateUserVM userVM);
        Task Update(EditUserVM userVM);
        Task Delete(int id);
        Task<User> GetUser(int id);
        Task<List<User>> GetAllUsers();
    }
}
