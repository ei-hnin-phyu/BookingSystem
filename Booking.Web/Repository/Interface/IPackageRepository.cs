using Booking.Model;
using Booking.Web.Models;

namespace Booking.Web.Repository.Interface
{
    public interface IPackageRepository
    {
        Task Create(CreatePackageVM package);
        Task Update(EditPackageVM package);
        Task Delete(int id);
        Task<Package> GetPackage(int id);
        Task<List<Package>> GetAllPackages();
        Task<List<Package>> GetAllPackagesByCountry(Country country);
    }
}
