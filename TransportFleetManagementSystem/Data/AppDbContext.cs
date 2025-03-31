using Microsoft.EntityFrameworkCore;
using TransportFleetManagementSystem.Model;
using TransportFleetManagementSystem.Models;

namespace TransportFleetManagementSystem.Data
    {
    public class AppDbContext : DbContext
        {
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Fuel> Fuels { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<Performance> Performances { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Server=LTIN449500\\SQLEXPRESS;Database=TFMSDB;Trusted_Connection=True;TrustServerCertificate=True;");


        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vehicle>().ToTable("Vehicle");
            modelBuilder.Entity<Trip>().ToTable("Trip");
            modelBuilder.Entity<Fuel>().ToTable("Fuel");
            modelBuilder.Entity<Maintenance>().ToTable("Maintenance");
            modelBuilder.Entity<Performance>().ToTable("Performance");
            modelBuilder.Entity<Driver>().ToTable("Driver");

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Vehicle)
                .WithMany(v => v.Trips)
                .HasForeignKey(t => t.VehicleId);

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Driver)
                .WithMany(d => d.Trips)
                .HasForeignKey(t => t.DriverId);

            modelBuilder.Entity<Fuel>()
                .HasOne(f => f.Vehicle)
                .WithMany(v => v.Fuels)
                .HasForeignKey(f => f.VehicleId);

            modelBuilder.Entity<Maintenance>()
                .HasOne(m => m.Vehicle)
                .WithMany(v => v.Maintenances)
                .HasForeignKey(m => m.VehicleId);
            }
        }
    }
