using Microsoft.EntityFrameworkCore;

namespace OOMALL.API.PRICING.Models
{
    public partial class OOParkingLotDbContext : DbContext
    {
        public OOParkingLotDbContext(DbContextOptions<OOParkingLotDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarCategory>().ToTable("CarCategories", schema: "dbo");
            modelBuilder.Entity<EntryPoint>().ToTable("EntryPoints", schema: "dbo");
            modelBuilder.Entity<ParkingAvailablity>().ToTable("ParkingAvailablity", schema: "dbo");
            modelBuilder.Entity<ParkingCategory>().ToTable("ParkingCategories", schema: "dbo");
            modelBuilder.Entity<ParkingDistance>().ToTable("ParkingDistances", schema: "dbo");
            modelBuilder.Entity<ParkingSpace>().ToTable("ParkingSpaces", schema: "dbo");
            modelBuilder.Entity<ParkingTransaction>().ToTable("ParkingTransactions", schema: "dbo");
        }

        public virtual DbSet<CarCategory> CarCategories { get; set; }
        public virtual DbSet<EntryPoint> EntryPoints { get; set; }
        public virtual DbSet<ParkingAvailablity> ParkingAvailablities { get; set; }
        public virtual DbSet<ParkingCategory> ParkingCategories { get; set; }
        public virtual DbSet<ParkingDistance> ParkingDistances { get; set; }
        public virtual DbSet<ParkingSpace> ParkingSpaces { get; set; }
        public virtual DbSet<ParkingTransaction> ParkingTransactions { get; set; }

    }
}
