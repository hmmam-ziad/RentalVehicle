using Microsoft.EntityFrameworkCore;
using RentalVehicle.Models;

namespace RentalVehicle.Data
{
    public class RentalDbContext : DbContext
    {
        public RentalDbContext(DbContextOptions<RentalDbContext> options) : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<FuleTypes> FuleTypes { get; set; }
        public DbSet<VehicleCategories> Categories { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<RentalBooking> RentalBookings { get; set; }
        public DbSet<RentalTransaction> RentalTransactions { get; set; }
        public DbSet<VehicleReturn> VehicleReturns { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Customer>().ToTable("Customer");
        //    modelBuilder.Entity<Vehicle>().ToTable("Vehicle");
        //    modelBuilder.Entity<VehicleCategories>().ToTable("VehicleCategories");
        //    modelBuilder.Entity<RentalBooking>().ToTable("RentalBooking");
        //    modelBuilder.Entity<RentalTransaction>().ToTable("RentalTransaction");
        //    modelBuilder.Entity<VehicleReturn>().ToTable("VehicleReturn");
        //}
    }
    
}
