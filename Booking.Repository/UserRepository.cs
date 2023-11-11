using Booking.Model;
using Booking.Repository.Interface;

namespace Booking.Repository
{
    public class UserRepository : IUserRepository
    {
        private BookingDbContext _dbContext;
        public UserRepository(BookingDbContext bookingDbContext) {
            this._dbContext = bookingDbContext;
        }

        public async Task Create(User userVM)
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

        public bool IsValidUser(string username, string password)
        {
            return this._dbContext.Users.Any(u => u.UserName.Equals(username) && u.Password.Equals(password));
        }
    }
}