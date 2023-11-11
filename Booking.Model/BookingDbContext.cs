using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
                Email = "eihninphyu1996@gmail.com",
                NumberOfCredits = 10,
                PhoneNumber = "09245555775",
                Password="12345",
            });
            modelBuilder.Entity<Package>().HasData(new Package
                {
                    Id = 1,
                    Name = "Yoga class",
                    Country =Country.US,
                    Fee = 2,
                    StartTime= DateTime.Now,
                    EndTime= DateTime.Now.AddDays(7),
                    Description = "It is yoga class"
                },
                new Package
                {
                    Id = 2,
                    Name = "Golf class",
                    Country = Country.Singapore,
                    Fee = 2,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddDays(7),
                    Description = "It is golf class"
                },
                new Package
                {
                    Id = 3,
                    Name = "Language class",
                    Country = Country.Myanmar,
                    Fee = 2,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddDays(7),
                    Description = "It is luagage class"
                }
            );
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
