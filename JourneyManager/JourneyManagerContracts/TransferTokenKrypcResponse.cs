using System;
using System.Collections.Generic;
using System.Text;

namespace UG.Journey.JourneyManager.Contracts
{
   public class TransferTokenKrypcResponse
    {
        public string Status { get; set; }
        public string Msg { get; set; }
        public TransferTokenKrypc Extra { get; set; }

    }
    public class TransferTokenKrypc
    {
        public string tokenid { get; set; }
        public string senderAccount { get; set; }
        public string receiverAccount { get; set; }
        public Int64 quantity { get; set; }
        public string transactionId { get; set; }
        public string transactionHash { get; set; }
    }
}
