using System;
using System.Collections.Generic;
using System.Text;

namespace UG.Journey.JourneyManager.Contracts
{
    public class GetJourneyByUserResponse
    {
        public List<BookingDetail> JourneyDetail { get; set; }
        //public int BookingDetailID { get; set; }
        public List<Booking> Journey { get; set; }
    }
}
