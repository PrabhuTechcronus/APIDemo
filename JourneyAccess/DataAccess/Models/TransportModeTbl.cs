using System;
using System.Collections.Generic;

#nullable disable

namespace UG.Journey.JourneyAccess.DataAccess.Models 
{
    public partial class TransportModeTbl
    {
        public int Id { get; set; }
        public string TransportMode { get; set; }
        public string TransportImage { get; set; }
        public string Description { get; set; }
        public DateTime? InActiveDate { get; set; }
    }
}
