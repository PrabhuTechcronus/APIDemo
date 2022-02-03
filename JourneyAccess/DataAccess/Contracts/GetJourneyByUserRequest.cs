using System;
using System.Collections.Generic;
using System.Text;

namespace UG.Journey.JourneyAccess.DataAccess.Contracts
{
   public class GetJourneyByUserRequest
    {
        public int UserID { get; set; }
        public int BookingID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
