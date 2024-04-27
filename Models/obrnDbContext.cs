using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OurBeautyReferralNetwork.Models;

public partial class obrnDbContext : DbContext
{
    public obrnDbContext()
    {
    }

    public obrnDbContext(DbContextOptions<obrnDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<AppointmentService> AppointmentServices { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Business> Businesses { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<FeeAndCommission> FeeAndCommissions { get; set; }

    public virtual DbSet<Referral> Referrals { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Reward> Rewards { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=beauty-network.postgres.database.azure.com;Database=postgres;Port=5432;Username=beauty_network;Password=P@ssw0rd!;SSL Mode=Require;Trust Server Certificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("pg_catalog", "azure")
            .HasPostgresExtension("pg_catalog", "pgaadauth");

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.PkAppointmentId).HasName("Appointment_pkey");

            entity.ToTable("Appointment");

            entity.Property(e => e.PkAppointmentId).HasColumnName("pkAppointmentID");
            entity.Property(e => e.FkCustomerId)
                .HasMaxLength(20)
                .HasColumnName("fkCustomerID");
            entity.Property(e => e.FkServiceId).HasColumnName("fkServiceID");
            entity.Property(e => e.Referred).HasDefaultValue(false);

            entity.HasOne(d => d.FkCustomer).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.FkCustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Appointment_fkCustomerID_fkey");

            entity.HasOne(d => d.FkService).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.FkServiceId)
                .HasConstraintName("fk_appointment_service_id");
        });

        modelBuilder.Entity<AppointmentService>(entity =>
        {
            entity.HasKey(e => e.PkAppointmentServiceId).HasName("AppointmentService_pkey");

            entity.ToTable("AppointmentService");

            entity.Property(e => e.PkAppointmentServiceId).HasColumnName("pkAppointmentServiceID");
            entity.Property(e => e.FkAppointmentId).HasColumnName("fkAppointmentID");
            entity.Property(e => e.FkServiceId).HasColumnName("fkServiceID");

            entity.HasOne(d => d.FkAppointment).WithMany(p => p.AppointmentServices)
                .HasForeignKey(d => d.FkAppointmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_appointment_id");
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Business>(entity =>
        {
            entity.HasKey(e => e.PkBusinessId).HasName("Business_pkey");

            entity.ToTable("Business");

            entity.Property(e => e.PkBusinessId)
                .HasMaxLength(30)
                .HasColumnName("pkBusinessID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.BusinessName).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.CommissionPaid).HasDefaultValue(false);
            entity.Property(e => e.ContactName).HasMaxLength(255);
            entity.Property(e => e.Description).HasMaxLength(1200);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.InsuranceCompany).HasMaxLength(255);
            entity.Property(e => e.IsVerified).HasDefaultValue(false);
            entity.Property(e => e.Logo).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.PostalCode).HasMaxLength(255);
            entity.Property(e => e.Province).HasMaxLength(255);
            entity.Property(e => e.VerificationDocument).HasMaxLength(255);
            entity.Property(e => e.Vip)
                .HasDefaultValue(false)
                .HasColumnName("VIP");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.PkCategoryId).HasName("Category_pkey");

            entity.ToTable("Category");

            entity.Property(e => e.PkCategoryId).HasColumnName("pkCategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(255);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.PkCustomerId).HasName("Customer_pkey");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.Email, "Customer_Email_key").IsUnique();

            entity.Property(e => e.PkCustomerId)
                .HasMaxLength(20)
                .HasColumnName("pkCustomerID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.Confirm18).HasDefaultValue(false);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Photo).HasMaxLength(255);
            entity.Property(e => e.PostalCode).HasMaxLength(255);
            entity.Property(e => e.Province).HasMaxLength(255);
            entity.Property(e => e.Qr)
                .HasMaxLength(255)
                .HasColumnName("QR");
            entity.Property(e => e.Vip)
                .HasDefaultValue(false)
                .HasColumnName("VIP");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.PkDiscountId).HasName("Discount_pkey");

            entity.ToTable("Discount");

            entity.Property(e => e.PkDiscountId)
                .HasMaxLength(5)
                .HasColumnName("pkDiscountID");
        });

        modelBuilder.Entity<FeeAndCommission>(entity =>
        {
            entity.HasKey(e => e.PkFeeId).HasName("FeeAndCommission_pkey");

            entity.ToTable("FeeAndCommission");

            entity.Property(e => e.PkFeeId)
                .HasMaxLength(4)
                .HasColumnName("pkFeeID");
            entity.Property(e => e.Description).HasMaxLength(1200);
            entity.Property(e => e.FeeType)
                .HasMaxLength(12)
                .IsFixedLength();
            entity.Property(e => e.Frequency)
                .HasMaxLength(8)
                .IsFixedLength();
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Referral>(entity =>
        {
            entity.HasKey(e => e.PkReferralId).HasName("Referral_pkey");

            entity.ToTable("Referral");

            entity.Property(e => e.PkReferralId).HasColumnName("pkReferralID");
            entity.Property(e => e.FkReferredBusinessId)
                .HasMaxLength(30)
                .HasColumnName("fkReferredBusinessID");
            entity.Property(e => e.FkReferredCustomerId)
                .HasMaxLength(20)
                .HasColumnName("fkReferredCustomerID");
            entity.Property(e => e.FkReferrerBusinessId)
                .HasMaxLength(30)
                .HasColumnName("fkReferrerBusinessID");
            entity.Property(e => e.FkReferrerCustomerId)
                .HasMaxLength(20)
                .HasColumnName("fkReferrerCustomerID");
            entity.Property(e => e.ReferredType).HasMaxLength(1);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.FkReferredBusiness).WithMany(p => p.ReferralFkReferredBusinesses)
                .HasForeignKey(d => d.FkReferredBusinessId)
                .HasConstraintName("Referral_fkReferredBusinessID_fkey");

            entity.HasOne(d => d.FkReferredCustomer).WithMany(p => p.ReferralFkReferredCustomers)
                .HasForeignKey(d => d.FkReferredCustomerId)
                .HasConstraintName("Referral_fkReferredCustomerID_fkey");

            entity.HasOne(d => d.FkReferrerBusiness).WithMany(p => p.ReferralFkReferrerBusinesses)
                .HasForeignKey(d => d.FkReferrerBusinessId)
                .HasConstraintName("Referral_fkReferrerBusinessID_fkey");

            entity.HasOne(d => d.FkReferrerCustomer).WithMany(p => p.ReferralFkReferrerCustomers)
                .HasForeignKey(d => d.FkReferrerCustomerId)
                .HasConstraintName("Referral_fkReferrerCustomerID_fkey");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.PkReviewId).HasName("Review_pkey");

            entity.ToTable("Review");

            entity.Property(e => e.PkReviewId).HasColumnName("pkReviewId");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.FkBusinessId)
                .HasMaxLength(30)
                .HasColumnName("fkBusinessID");
            entity.Property(e => e.FkCustomerId)
                .HasMaxLength(20)
                .HasColumnName("fkCustomerID");
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.Provider).HasMaxLength(255);

            entity.HasOne(d => d.FkBusiness).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.FkBusinessId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Review_fkBusinessID_fkey");

            entity.HasOne(d => d.FkCustomer).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.FkCustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Review_fkCustomerID_fkey");
        });

        modelBuilder.Entity<Reward>(entity =>
        {
            entity.HasKey(e => e.PkRewardId).HasName("Reward_pkey");

            entity.ToTable("Reward");

            entity.Property(e => e.PkRewardId).HasColumnName("pkRewardID");
            entity.Property(e => e.FkReferralId).HasColumnName("fkReferralID");

            entity.HasOne(d => d.FkReferral).WithMany(p => p.Rewards)
                .HasForeignKey(d => d.FkReferralId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Reward_fkReferralID_fkey");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.PkServiceId).HasName("Service_pkey");

            entity.ToTable("Service");

            entity.Property(e => e.PkServiceId).HasColumnName("pkServiceID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.FkBusinessId)
                .HasMaxLength(30)
                .HasColumnName("fkBusinessID");
            entity.Property(e => e.FkCategoryId).HasColumnName("fkCategoryID");
            entity.Property(e => e.FkDiscountId)
                .HasMaxLength(5)
                .HasColumnName("fkDiscountID");
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.ServiceName).HasMaxLength(255);

            entity.HasOne(d => d.FkBusiness).WithMany(p => p.Services)
                .HasForeignKey(d => d.FkBusinessId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Service_fkBusinessID_fkey");

            entity.HasOne(d => d.FkCategory).WithMany(p => p.Services)
                .HasForeignKey(d => d.FkCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Service_fkCategoryID_fkey");

            entity.HasOne(d => d.FkDiscount).WithMany(p => p.Services)
                .HasForeignKey(d => d.FkDiscountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Service_fkDiscountID_fkey");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.PkTestimonialId).HasName("Testimonial_pkey");

            entity.ToTable("Testimonial");

            entity.Property(e => e.PkTestimonialId).HasColumnName("pkTestimonialId");
            entity.Property(e => e.Approved).HasDefaultValue(false);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.FkBusinessId)
                .HasMaxLength(30)
                .HasColumnName("fkBusinessID");

            entity.HasOne(d => d.FkBusiness).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.FkBusinessId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Testimonial_fkBusinessID_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
