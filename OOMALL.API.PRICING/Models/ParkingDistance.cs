using System;
namespace OOMALL.API.PRICING.Models
{
    public partial class ParkingDistance
    {
        public Guid Id { get; set; }
        public Guid ParkingSpaceId { get; set; }
        public Guid EntryPointId { get; set; }
        public decimal? DistanceInMeters { get; set; }
    }
}
