using System;
using System.Collections.Generic;
using System.Text;

namespace UG.Journey.JourneyManager.Contracts
{
    public class CreateJourneyRequest
    {
        public BookingJourney Journey { get; set; }
        public List<BookingList> JourneyDetail { get; set; }
        public int BookingDetailID { get; set; }

    }

    public class BookingJourney
    {
        public int UserId { get; set; }
        //public int Id { get; set; }
        //public int BookingId { get; set; }
        //public int? TransportMode { get; set; }
        //public string TransportPassNumber { get; set; }
        //public string TransportVehicleNo { get; set; }
        public string LocationFrom { get; set; }
        public string LocationTo { get; set; }
        //public string TransportCity { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public string TotalHours { get; set; }
        public decimal? TotalPrice { get; set; }
        public string TransportStatus { get; set; }
        public decimal? CarbonEmission { get; set; }
        public decimal? GreenPoint { get; set; }
        public string topicId { get; set; }
        public string transactionId { get; set; }
        public string sequenceNumber { get; set; }
        public string networkType { get; set; }
    }

    public class BookingList
    {
        public int? TransportMode { get; set; }
        public string TransportPassNumber { get; set; }
        public string TransportVehicleNo { get; set; }
        public string LocationFrom { get; set; }
        public string LocationTo { get; set; }
        public string TransportCity { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public string TotalHours { get; set; }
        public decimal? TotalPrice { get; set; }
        public string TransportStatus { get; set; }
        public decimal? CarbonEmission { get; set; }
    }

}
