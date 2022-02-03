using System;
using System.Collections.Generic;
using System.Text;

namespace UG.Journey.JourneyManager.Contracts
{
   public class TransferTokenRequest
    {
        public DateTime? dateAndTime { get; set; }
        public decimal offerPrice { get; set; }
        public decimal quantity { get; set; }
        public string receiverUsername { get; set; }
        public string senderUsername { get; set; }
        public string tokenType { get; set; }
        public int tokenId { get; set; }
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public string transactionType { get; set; }
    }
}
