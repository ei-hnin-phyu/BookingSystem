﻿using Booking.Model;
using Booking.Web.Models;
using Booking.Web.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Booking.Web.Repository
{
    public class UserRepository : IUserRepository
    {
        private BookingDbContext _dbContext;
        public UserRepository(BookingDbContext bookingDbContext) {
            this._dbContext = bookingDbContext;
        }

        public async Task Create(CreateUserVM userVM)
        {
            await this._dbContext.Users.AddAsync(new User 
            {
                Name = userVM.Name,
                Email = userVM.Email,
                NumberOfCredits= userVM.NumberOfCredits,
                Country = userVM.Country,
                Password = userVM.Password,
                UserName = userVM.Name,
                PhoneNumber= userVM.PhoneNumber
                });
            await this._dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await this._dbContext.Users.Where(u => u.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await this._dbContext.Users.ToListAsync();
        }

        public async Task<User> GetUser(int id)
        {
            return await this._dbContext.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public bool IsValidUser(string username, string password)
        {
            return this._dbContext.Users.Any(u => u.UserName.Equals(username) && u.Password.Equals(password));
        }

        public async Task Update(EditUserVM userVM)
        {
            this._dbContext.Users.Update(new User
            {
                Id= userVM.Id,
                Name = userVM.Name,
                Email = userVM.Email,
                NumberOfCredits = userVM.NumberOfCredits,
                Country = userVM.Country,
                Password = userVM.Password,
                UserName = userVM.Name,
                PhoneNumber = userVM.PhoneNumber
            });
            await this._dbContext.SaveChangesAsync();
        }
    }
}