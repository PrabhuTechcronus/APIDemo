using System;
using System.Collections.Generic;
using System.Text;
using Core;

namespace UG.Journey.JourneyAccess.DataAccess.Contracts
{
   public class GeneratePaymentHistoryResponse
    {
        public List<PaymentHistory> paymentHistory { get; set; }
        public ValidationResults validationResults { get; set; }
    }

    public class PaymentHistory
    {
        public DateTime? PaymentDate { get; set; }
        public int PaymentId { get; set; }
        public int BookingId { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public string MspName { get; set; }
        public string TransportName { get; set; }
        public decimal? GreenPoint { get; set; }
        public decimal? CarbonEmbission { get; set; }
        public string TotalTime { get; set; }
    }
}
