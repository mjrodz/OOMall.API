using System;

namespace OOMALL.API.PRICING.Models
{
    public partial class ParkingSpace
    {
        public Guid Id { get; set; }
        public Guid ParkingCategoryId { get; set; }
        public string Code { get; set; }
        public bool IsOccupied { get; set; }
        public bool Status { get; set; }
    }
}
