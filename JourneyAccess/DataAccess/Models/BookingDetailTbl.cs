using System;
using System.Collections.Generic;

#nullable disable

namespace UG.Journey.JourneyAccess.DataAccess.Models
{
    public partial class BookingDetailTbl
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int? TransportMode { get; set; }
        public string TransportPassNumber { get; set; }
        public string TransportVehicleNo { get; set; }
        public string LocationFrom { get; set; }
        public string LocationTo { get; set; }
        public string TransportCity { get; set; }
        public DateTime? TimeFrom { get; set; }
        public DateTime? TimeTo { get; set; }
        public TimeSpan? TotalHours { get; set; }
        public decimal? TotalPrice { get; set; }
        public string TransportStatus { get; set; }
        public decimal? CarbonEmission { get; set; }
        public DateTime? ExpireTime { get; set; }
        public string TopicId { get; set; }
        public string TransactionId { get; set; }
        public string SequenceNumber { get; set; }
        public string NetworkType { get; set; }

        public virtual BookingTbl Booking { get; set; }
    }
}
