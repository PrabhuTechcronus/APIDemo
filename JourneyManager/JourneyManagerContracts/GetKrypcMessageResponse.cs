using System;
using System.Collections.Generic;
using System.Text;

namespace UG.Journey.JourneyManager.Contracts
{
   public class GetKrypcMessageResponse
    {
        public string Status { get; set; }
        public string Msg { get; set; }
        public GetKrypcMessageExtra Extra { get; set; }
    }
    public class GetKrypcMessageExtra
    {
        public string topicId { get; set; }
        public string transactionId { get; set; }
        public string sequenceNumber { get; set; }
        public string networkType { get; set; }
    }
}
