using System;
using System.Collections.Generic;
using System.Text;

namespace UG.Journey.JourneyManager.Contracts
{
   public class CreateKrypcTokenResponse
    {
        public string Status { get; set; }
        public string Msg { get; set; }
        public CreateKrypcTokenExtra Extra { get; set; }
    }
    public class CreateKrypcTokenExtra
    {
        public string token { get; set; }
    }
}
