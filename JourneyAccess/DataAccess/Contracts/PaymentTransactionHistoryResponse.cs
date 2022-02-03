using System;
using System.Collections.Generic;
using System.Text;
using Core;

namespace UG.Journey.JourneyAccess.DataAccess.Contracts
{
    public class PaymentTransactionHistoryResponse
    {
        public List<PaymentTransactionHistory> transaction { get; set; }
        public ValidationResults validationResults { get; set; }
    }
    public class PaymentTransactionHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookingId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TokenType { get; set; }
        public string TransactionType { get; set; }
        public decimal TransactionAmount { get; set; }
    }
}
