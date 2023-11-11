using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Model
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Package> Packages {  get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Country = Country.Myanmar,
                Email = "myuser123@gmail.com",
                Name = "User001",
                NumberOfCredits = 10,
                PhoneNumber = "09245555775",
                Password="12345",
            });
            base.OnModelCreating(modelBuilder);
        }
    }
    public class DbContextFactory : IDesignTimeDbContextFactory<BookingDbContext>
    {
        const string CONN = "Server=localhost; Database=BookingSystem; User Id=root; Password=$ecr3t;Allow User Variables=true;";
        public BookingDbContext CreateDbContext(string[] args)
        {
            string connectionstring = CONN;
            if (args.Length > 0)
                 connectionstring = args[0];
            var optionsBuilder = new DbContextOptionsBuilder<BookingDbContext>();
            optionsBuilder.UseMySql(connectionstring, ServerVersion.AutoDetect(connectionstring));

            return new BookingDbContext(optionsBuilder.Options);
        }
    }
}
