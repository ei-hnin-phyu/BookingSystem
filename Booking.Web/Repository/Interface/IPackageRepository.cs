using Booking.Model;
using Booking.Web.Models;

namespace Booking.Web.Repository.Interface
{
    public interface IPackageRepository
    {
        Task Create(CreatePackageVM package);
        Task<Package> GetPackage(int id);
        Task<List<Package>> GetAllPackagesByCountry(Country country);
        Task<List<Package>> GetAllPackagesByCountry(Country country,int userID);
        Task<List<Package>> GetPurchasedPackages(int userId);
    }
}
