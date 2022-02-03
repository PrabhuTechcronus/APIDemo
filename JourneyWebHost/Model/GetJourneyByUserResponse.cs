using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UG.Journey.JourneyManager.Contracts;

namespace JourneyWebHost.Model
{
    public class GetJourneyByUser
    {
        public bool status { get; set; }
        public string message { get; set; }
        public GetJourneyByUserResponse getJourneyByUserResponse { get; set; }
    }
}
