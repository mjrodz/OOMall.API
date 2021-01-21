using System;

namespace OOMALL.API.PRICING.Models
{
    public partial class ParkingTransaction
    {
        public Guid Id { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime? ExitDate { get; set; }
        public string PlateNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public Guid CarCategoryId { get; set; }
        public Guid ParkingSpaceId { get; set; }
        public decimal? TotalHours { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Penalties { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
