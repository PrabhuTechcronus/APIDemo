using System;
using System.Collections.Generic;

#nullable disable

namespace UG.Journey.JourneyAccess.DataAccess.Models 
{
    public partial class MspTbl
    {
        public int Id { get; set; }
        public string MspName { get; set; }
        public string ContactPerson { get; set; }
        public string EmailId { get; set; }
        public string PhoneNo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public decimal? WalletAmmout { get; set; }
        public int? Status { get; set; }
        public int? TransportMode { get; set; }
        public string MspLogo { get; set; }
        public string AccountemailId { get; set; }
        public string AccountId { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
    }
}
