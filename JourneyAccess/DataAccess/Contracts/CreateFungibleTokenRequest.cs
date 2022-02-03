using System;
using System.Collections.Generic;
using System.Text;

namespace UG.Journey.JourneyAccess.DataAccess.Contracts
{
   public class CreateFungibleTokenRequest
    {
        public bool apiMode { get; set; }
        public string createdBy { get; set; }
        public string dateAndTime { get; set; }
        public decimal decimals { get; set; }
        public string name { get; set; }
        public decimal supply { get; set; }
        public string symbol { get; set; }
    }
}
