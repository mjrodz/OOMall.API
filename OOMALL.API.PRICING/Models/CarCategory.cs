using System;

namespace OOMALL.API.PRICING.Models
{
    public partial class CarCategory
    {
        public Guid Id { get; set; }
        public byte Type { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
