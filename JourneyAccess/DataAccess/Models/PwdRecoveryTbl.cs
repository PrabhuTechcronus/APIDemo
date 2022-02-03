using System;
using System.Collections.Generic;

#nullable disable

namespace UG.Journey.JourneyAccess.DataAccess.Models 
{
    public partial class PwdRecoveryTbl
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ForgotPwdLink { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsExpired { get; set; }

        public virtual UserTbl User { get; set; }
    }
}
