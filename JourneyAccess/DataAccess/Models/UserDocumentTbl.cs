using System;
using System.Collections.Generic;

#nullable disable

namespace UG.Journey.JourneyAccess.DataAccess.Models 
{
    public partial class UserDocumentTbl
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserDocument { get; set; }
        public bool? IsActive { get; set; }

        public virtual UserTbl User { get; set; }
    }
}
