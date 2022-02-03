using System;
using System.Collections.Generic;

#nullable disable

namespace UG.Journey.JourneyAccess.DataAccess.Models 
{
    public partial class UserNotificationTbl
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? NotificationId { get; set; }
        public DateTime? NotificationTime { get; set; }
        public string NotificationDesc { get; set; }
        public bool? IsView { get; set; }

        public virtual NotificationMasterTbl Notification { get; set; }
        public virtual UserTbl User { get; set; }
    }
}
