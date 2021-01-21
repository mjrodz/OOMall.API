using System;

namespace OOMALL.API.PRICING.ViewModels
{
    public class CarUnparkRequest
    {
        public string PlateNumber { get; set; }
        public DateTime ExitDate { get; set; }
    }
}
