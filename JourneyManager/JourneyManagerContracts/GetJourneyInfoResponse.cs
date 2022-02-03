
using Core;
using System;
using System.Collections.Generic;
using UG.Journey.JourneyAccess.DataAccess.Contracts;

namespace UG.Journey.JourneyManager.Contracts
{
    public class GetJourneyInfoResponse
    {
        public TransportRouteType TransportRoutes { get; set; }
        public List<TransportRoutes> DetailRoutes { get; set; }
    }

    public class TransportRouteType
    {
        public List<TransportRoutes> FastestRoutes { get; set; }
        public List<TransportRoutes> CheapestRoutes { get; set; }
        public List<TransportRoutes> GreenestRoutes { get; set; }
    }

    public class TransportRoutes
    {
        public int RoutNo { get; set; }
        public string TransportMode { get; set; }
        public decimal TransportCharge { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
        public string Hours { get; set; }
        public string Distance { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public string CurrencySymbol { get; set; }
        public string Polyline { get; set; }
        public string NoOfStops { get; set; }
        public int HourValue { get; set; }
        public int TransportModeID { get; set; }
        public string StartLocationLatLong { get; set; }
        public string EndLocationLatLong { get; set; }
        public decimal CarbonEmission { get; set; }
        public decimal GreenPoint { get; set; }
        public string MSPName { get; set; }
        public string MSPImage { get; set; }

        public string TransportVehicleNo { get; set; }
    }

}
