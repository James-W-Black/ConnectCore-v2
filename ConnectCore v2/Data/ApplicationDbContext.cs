using ConnectCore_v2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ConnectCore_v2.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public ApplicationDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString).UseLazyLoadingProxies();
            }
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<UserToUserType> UserToUserTypes { get; set; }
        public DbSet<ConnectCore_v2.Models.User> User { get; set; }
        public DbSet<Holiday> Holiday { get; set; }
        public DbSet<HolidayStatus> HolidayStatus { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<EquipmentBooked> EquipmentBooked { get; set; }

    }
}