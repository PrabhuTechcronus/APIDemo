using System;
using System.Collections.Generic;

#nullable disable

namespace UG.Journey.JourneyAccess.DataAccess.Models 
{
    public partial class CorporateTbl
    {
        public int CorporateId { get; set; }
        public string CorporateName { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public int? AccountId { get; set; }
    }
}
