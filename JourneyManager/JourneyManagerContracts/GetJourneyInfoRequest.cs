using System;
using System.Collections.Generic;
using System.Text;

namespace UG.Journey.JourneyManager.Contracts
{
    public class GetJourneyInfoRequest
    {
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public List<string> Transportmode { get; set; }
        //public string IsFastest { get; set; }
        //public string IsCheapest { get; set; }
        //public string IsGreenest { get; set; }

        public string RequestUrl { get; set; }
    }

   
}
