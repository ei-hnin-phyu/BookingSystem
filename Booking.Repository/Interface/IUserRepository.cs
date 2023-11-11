using Booking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Repository.Interface
{
    public interface IUserRepository
    {
        bool IsValidUser(string username,string password);
        Task Create(UserVM userVM);
    }
}
