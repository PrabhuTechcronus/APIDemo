using Core;
using System;
using System.Collections.Generic;
using System.Text;
using UG.Journey.JourneyAccess.DataAccess.Models;

namespace UG.Journey.JourneyAccess.DataAccess.Contracts
{
    public class JourneyResponse
    {
        public ValidationResults ValidationResults { get; set; }
        public GetTransportPriceResponse TransportModePrice { get; set; }
        public ICollection<BookingTbl> JourneyList { get; set; }
    }

  
}
