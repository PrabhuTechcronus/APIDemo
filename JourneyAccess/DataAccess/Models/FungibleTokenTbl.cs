using System;
using System.Collections.Generic;

#nullable disable

namespace UG.Journey.JourneyAccess.DataAccess.Models 
{
    public partial class FungibleTokenTbl
    {
        public int Id { get; set; }
        public string TokenId { get; set; }
        public string TokenName { get; set; }
        public string Symbol { get; set; }
        public double? TokenDecimal { get; set; }
        public double? Supply { get; set; }
        public DateTime? TokenDate { get; set; }
        public double? OfferPrice { get; set; }
        public string CreatedBy { get; set; }
    }
}
