using System;
using System.Collections.Generic;

#nullable disable

namespace UG.Journey.JourneyAccess.DataAccess.Models 
{
    public partial class PaymentHistoryTbl
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string TransactionToken { get; set; }
        public string TransactionError { get; set; }
        public string TransactionType { get; set; }
        public string Status { get; set; }
        public decimal? TransactionAmount { get; set; }
        public int? Mspid { get; set; }
        public int? BookingDetailid { get; set; }

        public virtual UserTbl User { get; set; }
    }
}
