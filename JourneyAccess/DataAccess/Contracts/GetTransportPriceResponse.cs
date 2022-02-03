using System;
using System.Collections.Generic;
using System.Text;
using UG.Journey.JourneyAccess.DataAccess.Models;

namespace UG.Journey.JourneyAccess.DataAccess.Contracts
{
   public class GetTransportPriceResponse
    {
        public string MspLogo { get; set; }

        public string MspName { get; set; }

        public TransportPrice transportPrice { get; set; }
    }
}
