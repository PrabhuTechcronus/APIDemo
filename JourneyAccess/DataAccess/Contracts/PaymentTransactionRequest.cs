using System;
using System.Collections.Generic;
using System.Text;

namespace UG.Journey.JourneyAccess.DataAccess.Contracts
{
    public class PaymentTransactionRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookingId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int TokenId { get; set; }
        public string TransactionType { get; set; }
        public decimal TransactionAmount { get; set; }
        public string TransactionId { get; set; }
        public string SenderAcountId { get; set; }
        public string SenderPublicKey { get; set; }
        public string ReceiverAccountId { get; set; }
        public string ReceiverPublicKey { get; set; }
        public int MSPId { get; set; }
        public int TransportMode { get; set; }
        public int BookingDetailId { get; set; }
    }
}
