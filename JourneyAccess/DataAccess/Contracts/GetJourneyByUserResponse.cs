using Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace UG.Journey.JourneyAccess.DataAccess.Contracts
{
    public class GetJourneyByUserResponse
    {
        public ValidationResults ValidationResults { get; set; }
        public List<BookingDetail> JourneyDetail { get; set; }
        public List<Booking> Journey { get; set; }
    }

    public class BookingDetail
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int? TransportModeID { get; set; }
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
        public string TransportMode { get; set; }
        public decimal CarbonEmission { get; set; }
        public string MspName { get; set; }
        public string MspLogo { get; set; }

    }

    public class Booking
    {
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
        public List<msp> mspdetail { get; set; }
    }
    public class msp
    {
        public string mspName { get; set; }
        public string Type { get; set; }
        public decimal carbonembission { get; set; }
    }
}
