using System;

namespace OOMALL.API.PRICING.Models
{
    public partial class ParkingCategory
    {
        public Guid Id { get; set; }
        public byte Type { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal HourlyRate { get; set; }
    }
}
