using Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace UG.Journey.JourneyAccess.DataAccess.Contracts
{
    public class CreateJourenyResponse
    {
        public ValidationResults ValidationResults { get; set; }
        public List<Models.BookingDetailTbl> JourneyDetail { get; set; }
    }
}
