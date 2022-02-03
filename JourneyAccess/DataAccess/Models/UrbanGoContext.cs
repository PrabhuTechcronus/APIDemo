using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace UG.Journey.JourneyAccess.DataAccess.Models
{
    public partial class UrbanGoContext : DbContext
    {
        public UrbanGoContext()
        {
        }

        public UrbanGoContext(DbContextOptions<UrbanGoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BookingDetailTbl> BookingDetailTbls { get; set; }
        public virtual DbSet<BookingTbl> BookingTbls { get; set; }
        public virtual DbSet<CorporateTbl> CorporateTbls { get; set; }
        public virtual DbSet<FungibleTokenTbl> FungibleTokenTbls { get; set; }
        public virtual DbSet<KrypcTokenTbl> KrypcTokenTbls { get; set; }
        public virtual DbSet<MspTbl> MspTbls { get; set; }
        public virtual DbSet<NotificationMasterTbl> NotificationMasterTbls { get; set; }
        public virtual DbSet<PaymentHistoryTbl> PaymentHistoryTbls { get; set; }
        public virtual DbSet<PaymentTransactionTbl> PaymentTransactionTbls { get; set; }
        public virtual DbSet<PwdRecoveryTbl> PwdRecoveryTbls { get; set; }
        public virtual DbSet<TransportModeTbl> TransportModeTbls { get; set; }
        public virtual DbSet<TransportPrice> TransportPrices { get; set; }
        public virtual DbSet<TripLegsTbl> TripLegsTbls { get; set; }
        public virtual DbSet<UserDocumentTbl> UserDocumentTbls { get; set; }
        public virtual DbSet<UserNotificationTbl> UserNotificationTbls { get; set; }
        public virtual DbSet<UserSignInHistory> UserSignInHistories { get; set; }
        public virtual DbSet<UserTbl> UserTbls { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Server=TBS35\\SQLEXPRESS;Database=UrbanGo;User ID=Monil;Password=TBS@12345");
                //string connectionString = ConfigurationManager.AppSettings["Connectionstring"];
                string s = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                //string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetCallingAssembly().CodeBase) + @"\DataAccess.dll.config";
                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetCallingAssembly().CodeBase) + @"\JourneyWebHost.dll.config";
                path = System.IO.Path.GetFileName("App.Config");
                XDocument doc = XDocument.Load(path);
                var connectionString = doc.Descendants("DBConnection").Nodes().Cast<XElement>().Where(x => x.Attribute("key").Value.ToString() == "Connectionstring").FirstOrDefault().LastAttribute.Value;

                optionsBuilder.UseSqlServer(connectionString,
                    opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)
                    .EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(3), errorNumbersToAdd: null));

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<BookingDetailTbl>(entity =>
            {
                entity.ToTable("BookingDetailTbl");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.CarbonEmission).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.ExpireTime).HasColumnType("datetime");

                entity.Property(e => e.LocationFrom)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LocationTo)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NetworkType)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("networkType");

                entity.Property(e => e.SequenceNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("sequenceNumber");

                entity.Property(e => e.TimeFrom).HasColumnType("datetime");

                entity.Property(e => e.TimeTo).HasColumnType("datetime");

                entity.Property(e => e.TopicId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("topicId");

                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TransactionId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("transactionId");

                entity.Property(e => e.TransportCity)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TransportPassNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransportStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TransportVehicleNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BookingTbl>(entity =>
            {
                entity.ToTable("BookingTbl");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CarbonEmission).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.GreenPoints).HasColumnType("numeric(5, 0)");

                entity.Property(e => e.LocationFrom)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LocationTo)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NetworkType)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("networkType");

                entity.Property(e => e.SequenceNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("sequenceNumber");

                entity.Property(e => e.TimeFrom).HasColumnType("datetime");

                entity.Property(e => e.TimeTo).HasColumnType("datetime");

                entity.Property(e => e.TopicId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("topicId");

                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TransactionId)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("transactionId");

                entity.Property(e => e.TransportStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BookingTbls)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BookingTbl_UserTbl");
            });

            modelBuilder.Entity<CorporateTbl>(entity =>
            {
                entity.HasKey(e => e.CorporateId)
                    .HasName("PK__Corporat__87E40386EF63E883");

                entity.ToTable("CorporateTbl");

                entity.Property(e => e.AccountId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("AccountID");

                entity.Property(e => e.ContactPerson)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CorporateName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PrivateKey).IsUnicode(false);

                entity.Property(e => e.PublicKey).IsUnicode(false);
            });

            modelBuilder.Entity<FungibleTokenTbl>(entity =>
            {
                entity.ToTable("FungibleTokenTbl");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Symbol)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TokenDate).HasColumnType("datetime");

                entity.Property(e => e.TokenId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TokenName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<KrypcTokenTbl>(entity =>
            {
                entity.ToTable("KrypcTokenTbl");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Token)
                    .IsUnicode(false)
                    .HasColumnName("token");
            });

            modelBuilder.Entity<MspTbl>(entity =>
            {
                entity.ToTable("MspTbl");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("accountId");

                entity.Property(e => e.AccountemailId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("accountemailId");

                entity.Property(e => e.ContactPerson)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EmailId)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("EmailID");

                entity.Property(e => e.MspLogo)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.MspName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PrivateKey)
                    .IsUnicode(false)
                    .HasColumnName("privateKey");

                entity.Property(e => e.PublicKey)
                    .IsUnicode(false)
                    .HasColumnName("publicKey");

                entity.Property(e => e.WalletAmmout).HasColumnType("numeric(12, 2)");
            });

            modelBuilder.Entity<NotificationMasterTbl>(entity =>
            {
                entity.ToTable("NotificationMasterTbl");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.InActiveDate).HasColumnType("datetime");

                entity.Property(e => e.NotificationEvent)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NotificationMessage)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PaymentHistoryTbl>(entity =>
            {
                entity.ToTable("PaymentHistoryTbl");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionAmount).HasColumnType("numeric(12, 2)");

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

                entity.Property(e => e.TransactionError)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionToken).HasMaxLength(500);

                entity.Property(e => e.TransactionType)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PaymentHistoryTbls)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PaymentHistoryTbl_UserTbl");
            });

            modelBuilder.Entity<PaymentTransactionTbl>(entity =>
            {
                entity.ToTable("PaymentTransactionTbl");

                entity.Property(e => e.Mspid).HasColumnName("MSPId");

                entity.Property(e => e.ReceiverAccountId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReceiverPublicKey).IsUnicode(false);

                entity.Property(e => e.SenderAcountId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SenderPublicKey).IsUnicode(false);

                entity.Property(e => e.TransactionAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

                entity.Property(e => e.TransactionId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PwdRecoveryTbl>(entity =>
            {
                entity.ToTable("PwdRecoveryTbl");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.ForgotPwdLink)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PwdRecoveryTbls)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PwdRecoveryTbl_UserTbl");
            });

            modelBuilder.Entity<TransportModeTbl>(entity =>
            {
                entity.ToTable("TransportModeTbl");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.InActiveDate).HasColumnType("datetime");

                entity.Property(e => e.TransportImage)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.TransportMode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TransportPrice>(entity =>
            {
                entity.ToTable("TransportPrice");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Co2PerKm)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("Co2PerKM");

                entity.Property(e => e.PricePerKm).HasColumnName("PricePerKM");

                entity.Property(e => e.TransportMode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransportType)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TripLegsTbl>(entity =>
            {
                entity.ToTable("TripLegsTbl");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.CarbonEmission).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.ExpireTime).HasColumnType("datetime");

                entity.Property(e => e.LocationFrom)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LocationTo)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TimeFrom).HasColumnType("datetime");

                entity.Property(e => e.TimeTo).HasColumnType("datetime");

                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TransportCity)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TransportPassNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransportStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TransportVehicleNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserDocumentTbl>(entity =>
            {
                entity.ToTable("UserDocumentTbl");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.UserDocument)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserDocumentTbls)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserDocumentTbl_UserTbl");
            });

            modelBuilder.Entity<UserNotificationTbl>(entity =>
            {
                entity.ToTable("UserNotificationTbl");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.NotificationDesc)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.NotificationId).HasColumnName("NotificationID");

                entity.Property(e => e.NotificationTime).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Notification)
                    .WithMany(p => p.UserNotificationTbls)
                    .HasForeignKey(d => d.NotificationId)
                    .HasConstraintName("FK_UserNotificationTbl_NotificationMasterTbl");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserNotificationTbls)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserNotificationTbl_UserTbl");
            });

            modelBuilder.Entity<UserSignInHistory>(entity =>
            {
                entity.ToTable("UserSignInHistory");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Latitude)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Longitude)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.SignInTime).HasColumnType("datetime");

                entity.Property(e => e.SignOutTime).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSignInHistories)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserSignInHistory_UserTbl");
            });

            modelBuilder.Entity<UserTbl>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("UserTbl");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.AccountId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("accountId");

                entity.Property(e => e.Address1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Address2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CorporateId).HasColumnName("CorporateID");

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("DOB");

                entity.Property(e => e.EmailId)
                    .HasMaxLength(50)
                    .HasColumnName("EmailID");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.GreenPoint).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ImCertificateComment)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ImCertificateImage)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.LicenseComment)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.LicenseImange)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PassportComment)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PassportImage)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password).HasMaxLength(255);

                entity.Property(e => e.PhoneNo).HasMaxLength(50);

                entity.Property(e => e.PrivateKey)
                    .IsUnicode(false)
                    .HasColumnName("privateKey");

                entity.Property(e => e.PublicKey)
                    .IsUnicode(false)
                    .HasColumnName("publicKey");

                entity.Property(e => e.ResetPasswordCode)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName).HasMaxLength(50);

                entity.Property(e => e.UserProfileImg)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.WalletAmount).HasColumnType("decimal(18, 2)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
