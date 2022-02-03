using System;
using System.Collections.Generic;
using System.Text;
using Core;
using UG.Journey.JourneyAccess.DataAccess.Models;

namespace UG.Journey.JourneyAccess.DataAccess.Contracts
{
   public class GetMSPByTransportResponse
    {
        public ValidationResults validationResults { get; set; }
        public MspTbl msp { get; set; }
    }
}
