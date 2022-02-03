using System;
using System.Collections.Generic;

#nullable disable

namespace UG.Journey.JourneyAccess.DataAccess.Models 
{
    public partial class UserTbl
    {
        public UserTbl()
        {
            BookingTbls = new HashSet<BookingTbl>();
            PaymentHistoryTbls = new HashSet<PaymentHistoryTbl>();
            PwdRecoveryTbls = new HashSet<PwdRecoveryTbl>();
            UserDocumentTbls = new HashSet<UserDocumentTbl>();
            UserNotificationTbls = new HashSet<UserNotificationTbl>();
            UserSignInHistories = new HashSet<UserSignInHistory>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string PhoneNo { get; set; }
        public DateTime? Dob { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string UserProfileImg { get; set; }
        public decimal? WalletAmount { get; set; }
        public short? UserRole { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string ResetPasswordCode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }
        public int? PassportStatus { get; set; }
        public string PassportComment { get; set; }
        public string PassportImage { get; set; }
        public int? ImCertificateStatus { get; set; }
        public string ImCertificateComment { get; set; }
        public string ImCertificateImage { get; set; }
        public int? LicenseStatus { get; set; }
        public string LicenseComment { get; set; }
        public string LicenseImange { get; set; }
        public int? UserVerfied { get; set; }
        public string AccountId { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public decimal? GreenPoint { get; set; }

        public int? CorporateId { get; set; }

        public virtual ICollection<BookingTbl> BookingTbls { get; set; }
        public virtual ICollection<PaymentHistoryTbl> PaymentHistoryTbls { get; set; }
        public virtual ICollection<PwdRecoveryTbl> PwdRecoveryTbls { get; set; }
        public virtual ICollection<UserDocumentTbl> UserDocumentTbls { get; set; }
        public virtual ICollection<UserNotificationTbl> UserNotificationTbls { get; set; }
        public virtual ICollection<UserSignInHistory> UserSignInHistories { get; set; }
    }
}
