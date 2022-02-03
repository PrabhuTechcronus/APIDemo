using System;
using System.Collections.Generic;

#nullable disable

namespace UG.Journey.JourneyAccess.DataAccess.Models 
{
    public partial class TransportPrice
    {
        public int Id { get; set; }
        public string TransportMode { get; set; }
        public int? PricePerKm { get; set; }
        public string TransportType { get; set; }
        public decimal? Co2PerKm { get; set; }
    }
}
