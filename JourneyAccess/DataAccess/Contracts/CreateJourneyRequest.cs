using System;
using System.Collections.Generic;
using System.Text;

namespace UG.Journey.JourneyAccess.DataAccess.Contracts
{
    public class CreateJourneyRequest
    {
        public Models.BookingTbl Journey { get; set; }
        public List<Models.BookingDetailTbl> JourneyDetail { get; set; }
        public int BookingID { get; set; }

    }
}