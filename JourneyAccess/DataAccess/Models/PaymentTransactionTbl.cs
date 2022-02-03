using System;
using System.Collections.Generic;

#nullable disable

namespace UG.Journey.JourneyAccess.DataAccess.Models 
{
    public partial class PaymentTransactionTbl
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? BookingId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public int? TokenId { get; set; }
        public string TransactionType { get; set; }
        public decimal? TransactionAmount { get; set; }
        public string TransactionId { get; set; }
        public string SenderAcountId { get; set; }
        public string SenderPublicKey { get; set; }
        public string ReceiverAccountId { get; set; }
        public string ReceiverPublicKey { get; set; }
        public int? Mspid { get; set; }
        public int? TransportMode { get; set; }
        public int? BookingDetailId { get; set; }

    }
}
