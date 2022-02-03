using System;
using System.Collections.Generic;

#nullable disable

namespace UG.Journey.JourneyAccess.DataAccess.Models 
{
    public partial class NotificationMasterTbl
    {
        public NotificationMasterTbl()
        {
            UserNotificationTbls = new HashSet<UserNotificationTbl>();
        }

        public int Id { get; set; }
        public string NotificationEvent { get; set; }
        public string NotificationMessage { get; set; }
        public DateTime? InActiveDate { get; set; }

        public virtual ICollection<UserNotificationTbl> UserNotificationTbls { get; set; }
    }
}
