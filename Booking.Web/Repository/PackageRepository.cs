using Booking.Model;
using Booking.Web.Models;
using Booking.Web.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Booking.Web.Repository
{
    public class PackageRepository : IPackageRepository
    {
        private BookingDbContext _dbContext;
        public PackageRepository(BookingDbContext bookingDbContext)
        {
            this._dbContext = bookingDbContext;
        }
        public async Task Create(CreatePackageVM package)
        {
            await this._dbContext.Packages.AddAsync(new Package
            {
                Name = package.Name,
                Fee = package.Fee,
                Description = package.Description,
                EndTime = package.EndTime,
                StartTime = package.StartTime,                
                Country = package.Country,
            });
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<List<Package>> GetAllPackagesByCountry(Country country)
        {
            return await this._dbContext.Packages.Where(p =>p.Country == country).ToListAsync();
        }

        public async Task<Package> GetPackage(int id)
        {
            return await _dbContext.Packages.Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Package>> GetPurchasedPackages(int userId)
        {
            var user = await _dbContext.Users.Include(x => x.Packages).FirstOrDefaultAsync(x => x.Id == userId);
            if(user == null)
                return new List<Package>();
            return user.Packages;
        }
    }
}
