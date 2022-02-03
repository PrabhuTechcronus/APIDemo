using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UG.Journey.JourneyManager.Contracts;

namespace JourneyWebHost.Model
{
    public class JourneyResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public CreateJourneyResponse createJourneyResponse { get; set; }
    }
}
