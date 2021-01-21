using System;

namespace OOMALL.API.PRICING.ViewModels
{
    public class CarParkRequest
    {
        public Guid ParkingSpaceId { get; set; }
        public Guid CarCategoryId { get; set; }
        public string PlateNumber { get; set; }
    }
}
