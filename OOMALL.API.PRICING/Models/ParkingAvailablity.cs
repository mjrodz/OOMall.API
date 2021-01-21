using System;
namespace OOMALL.API.PRICING.Models
{
    public partial class ParkingAvailablity
    {
        public Guid Id { get; set; }
        public Guid ParkingCategoryId { get; set; }
        public Guid CarCategoryId { get; set; }
        public int Priority { get; set; }
    }
}
