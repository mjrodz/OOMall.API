using System;
namespace OOMALL.API.PRICING.Models
{
    public partial class EntryPoint
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
    }
}
