using System;
using System.Collections.Generic;

#nullable disable

namespace UG.Journey.JourneyAccess.DataAccess.Models 
{
    public partial class BookingTbl
    {
        public BookingTbl()
        {
            BookingDetailTbls = new HashSet<BookingDetailTbl>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string LocationFrom { get; set; }
        public string LocationTo { get; set; }
        public DateTime? TimeFrom { get; set; }
        public DateTime? TimeTo { get; set; }
        public TimeSpan? TotalHours { get; set; }
        public decimal? TotalPrice { get; set; }
        public string TransportStatus { get; set; }
        public decimal? GreenPoints { get; set; }
        public decimal? CarbonEmission { get; set; }
        public string TopicId { get; set; }
        public string TransactionId { get; set; }
        public string SequenceNumber { get; set; }
        public string NetworkType { get; set; }
        public virtual UserTbl User { get; set; }
        public virtual ICollection<BookingDetailTbl> BookingDetailTbls { get; set; }
    }
}
