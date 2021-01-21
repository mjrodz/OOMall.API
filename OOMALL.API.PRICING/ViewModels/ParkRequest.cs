using System;

namespace OOMALL.API.PRICING.ViewModels
{
    public class ParkRequest
    {
        public Guid CarCategoryId { get; set; }
        public Guid EntryPointId { get; set; }
    }
}
