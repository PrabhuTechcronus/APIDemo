using System;
using System.Collections.Generic;

#nullable disable

namespace UG.Journey.JourneyAccess.DataAccess.Models 
{
    public partial class UserSignInHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public DateTime? SignInTime { get; set; }
        public DateTime? SignOutTime { get; set; }

        public virtual UserTbl User { get; set; }
    }
}
