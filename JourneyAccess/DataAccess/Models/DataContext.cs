using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace UG.Journey.JourneyAccess.DataAccess.Models
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BookingDetailTbl> BookingDetailTbls { get; set; }
        public virtual DbSet<BookingTbl> BookingTbls { get; set; }
        public virtual DbSet<NotificationMasterTbl> NotificationMasterTbls { get; set; }
        public virtual DbSet<PaymentHistoryTbl> PaymentHistoryTbls { get; set; }
        public virtual DbSet<PwdRecoveryTbl> PwdRecoveryTbls { get; set; }
        public virtual DbSet<TransportModeTbl> TransportModeTbls { get; set; }
        public virtual DbSet<UserDocumentTbl> UserDocumentTbls { get; set; }
        public virtual DbSet<UserNotificationTbl> UserNotificationTbls { get; set; }
        public virtual DbSet<UserSignInHistory> UserSignInHistories { get; set; }
        public virtual DbSet<UserTbl> UserTbls { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=ugdevdbinstance.cfu214w2y9yf.eu-west-1.rds.amazonaws.com;Database=UrbanGo;User ID=devdbadmin;Password=DIZiTIdM12pLkKgsueB9;MultipleActiveResultSets=False;");
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

                entity.Property(e => e.LocationFrom)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LocationTo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TimeFrom).HasColumnType("datetime");

                entity.Property(e => e.TimeTo).HasColumnType("datetime");

                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TransportCity)
                    .HasMaxLength(20)
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

                entity.Property(e => e.LocationFrom)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LocationTo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TimeFrom).HasColumnType("datetime");

                entity.Property(e => e.TimeTo).HasColumnType("datetime");

                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

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

                entity.Property(e => e.Address1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Address2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("DOB");

                entity.Property(e => e.EmailId)
                    .HasMaxLength(50)
                    .HasColumnName("EmailID");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(255);

                entity.Property(e => e.RefreshToken).HasMaxLength(400);

                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Token).HasMaxLength(4000);

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
