using System;
using System.Collections.Generic;
using System.Text;

namespace UG.Journey.JourneyManager.Contracts
{
   public class TransferTokenKrypcRequest
    {
        public bool apiMode { get; set; }
        public string createdBy { get; set; }
        public DateTime? dateAndTime { get; set; }
        public decimal offerPrice { get; set; }
        public Int64 quantity { get; set; }
        public string receiverAccount { get; set; }
        public string receiverPrivateKey { get; set; }
        public string senderAccount { get; set; }
        public string senderPrivateKey { get; set; }
        public string tokenid { get; set; }
     
    }
}
