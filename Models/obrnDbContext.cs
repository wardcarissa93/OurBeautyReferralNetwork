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

    public virtual DbSet<Appointmentservice> Appointmentservices { get; set; }

    public virtual DbSet<Business> Businesses { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Feeandcommission> Feeandcommissions { get; set; }

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
            entity.HasKey(e => e.Pkappointmentid).HasName("appointment_pkey");

            entity.ToTable("appointment");

            entity.Property(e => e.Pkappointmentid).HasColumnName("pkappointmentid");
            entity.Property(e => e.Appointmentdate).HasColumnName("appointmentdate");
            entity.Property(e => e.Appointmenttime).HasColumnName("appointmenttime");
            entity.Property(e => e.Fkcustomerid).HasColumnName("fkcustomerid");
            entity.Property(e => e.Fkserviceid).HasColumnName("fkserviceid");
            entity.Property(e => e.Paymentstatus)
                .HasMaxLength(1)
                .HasColumnName("paymentstatus");

            entity.HasOne(d => d.Fkcustomer).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.Fkcustomerid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointment_fkcustomerid_fkey");

            entity.HasOne(d => d.Fkservice).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.Fkserviceid)
                .HasConstraintName("fk_appointment_service_id");
        });

        modelBuilder.Entity<Appointmentservice>(entity =>
        {
            entity.HasKey(e => e.Pkappointmentserviceid).HasName("appointmentservice_pkey");

            entity.ToTable("appointmentservice");

            entity.Property(e => e.Pkappointmentserviceid).HasColumnName("pkappointmentserviceid");
            entity.Property(e => e.Fkappointmentid).HasColumnName("fkappointmentid");
            entity.Property(e => e.Fkserviceid).HasColumnName("fkserviceid");

            entity.HasOne(d => d.Fkappointment).WithMany(p => p.Appointmentservices)
                .HasForeignKey(d => d.Fkappointmentid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_appointment_id");
        });

        modelBuilder.Entity<Business>(entity =>
        {
            entity.HasKey(e => e.Pkbusinessid).HasName("business_pkey");

            entity.ToTable("business");

            entity.Property(e => e.Pkbusinessid).HasColumnName("pkbusinessid");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("city");
            entity.Property(e => e.Commissionpaid)
                .HasMaxLength(1)
                .HasColumnName("commissionpaid");
            entity.Property(e => e.Contactname)
                .HasMaxLength(255)
                .HasColumnName("contactname");
            entity.Property(e => e.Description)
                .HasMaxLength(1200)
                .HasColumnName("description");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Insurancecompany)
                .HasMaxLength(255)
                .HasColumnName("insurancecompany");
            entity.Property(e => e.Insuranceexpirydate).HasColumnName("insuranceexpirydate");
            entity.Property(e => e.Isverified)
                .HasMaxLength(1)
                .HasColumnName("isverified");
            entity.Property(e => e.Logo)
                .HasMaxLength(255)
                .HasColumnName("logo");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Postalcode)
                .HasMaxLength(255)
                .HasColumnName("postalcode");
            entity.Property(e => e.Province)
                .HasMaxLength(255)
                .HasColumnName("province");
            entity.Property(e => e.Registrationdate).HasColumnName("registrationdate");
            entity.Property(e => e.Verificationdocument)
                .HasMaxLength(255)
                .HasColumnName("verificationdocument");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Pkcustomerid).HasName("customer_pkey");

            entity.ToTable("customer");

            entity.HasIndex(e => e.Email, "customer_email_key").IsUnique();

            entity.Property(e => e.Pkcustomerid).HasColumnName("pkcustomerid");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("city");
            entity.Property(e => e.Confirm18)
                .HasMaxLength(1)
                .HasColumnName("confirm18");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(255)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(255)
                .HasColumnName("lastname");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Photo)
                .HasMaxLength(255)
                .HasColumnName("photo");
            entity.Property(e => e.Postalcode)
                .HasMaxLength(255)
                .HasColumnName("postalcode");
            entity.Property(e => e.Province)
                .HasMaxLength(255)
                .HasColumnName("province");
            entity.Property(e => e.Qr)
                .HasMaxLength(255)
                .HasColumnName("qr");
            entity.Property(e => e.Vip)
                .HasMaxLength(1)
                .HasColumnName("vip");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.Pkdiscountid).HasName("discount_pkey");

            entity.ToTable("discount");

            entity.Property(e => e.Pkdiscountid)
                .HasMaxLength(5)
                .HasColumnName("pkdiscountid");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property(e => e.Percent).HasColumnName("percent");
        });

        modelBuilder.Entity<Feeandcommission>(entity =>
        {
            entity.HasKey(e => e.Pkfeeid).HasName("feeandcommission_pkey");

            entity.ToTable("feeandcommission");

            entity.Property(e => e.Pkfeeid)
                .HasMaxLength(4)
                .HasColumnName("pkfeeid");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Feetype)
                .HasMaxLength(12)
                .IsFixedLength()
                .HasColumnName("feetype");
            entity.Property(e => e.Frequency)
                .HasMaxLength(8)
                .IsFixedLength()
                .HasColumnName("frequency");
            entity.Property(e => e.Percent).HasColumnName("percent");
        });

        modelBuilder.Entity<Referral>(entity =>
        {
            entity.HasKey(e => e.Pkreferralid).HasName("referral_pkey");

            entity.ToTable("referral");

            entity.Property(e => e.Pkreferralid).HasColumnName("pkreferralid");
            entity.Property(e => e.Fkreferredbusinessid).HasColumnName("fkreferredbusinessid");
            entity.Property(e => e.Fkreferredcustomerid).HasColumnName("fkreferredcustomerid");
            entity.Property(e => e.Fkreferrerbusinessid).HasColumnName("fkreferrerbusinessid");
            entity.Property(e => e.Fkreferrercustomerid).HasColumnName("fkreferrercustomerid");
            entity.Property(e => e.Referraldate).HasColumnName("referraldate");
            entity.Property(e => e.Referredtype)
                .HasMaxLength(1)
                .HasColumnName("referredtype");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.Fkreferredbusiness).WithMany(p => p.ReferralFkreferredbusinesses)
                .HasForeignKey(d => d.Fkreferredbusinessid)
                .HasConstraintName("referral_fkreferredbusinessid_fkey");

            entity.HasOne(d => d.Fkreferredcustomer).WithMany(p => p.ReferralFkreferredcustomers)
                .HasForeignKey(d => d.Fkreferredcustomerid)
                .HasConstraintName("referral_fkreferredcustomerid_fkey");

            entity.HasOne(d => d.Fkreferrerbusiness).WithMany(p => p.ReferralFkreferrerbusinesses)
                .HasForeignKey(d => d.Fkreferrerbusinessid)
                .HasConstraintName("referral_fkreferrerbusinessid_fkey");

            entity.HasOne(d => d.Fkreferrercustomer).WithMany(p => p.ReferralFkreferrercustomers)
                .HasForeignKey(d => d.Fkreferrercustomerid)
                .HasConstraintName("referral_fkreferrercustomerid_fkey");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Pkreviewid).HasName("review_pkey");

            entity.ToTable("review");

            entity.Property(e => e.Pkreviewid).HasColumnName("pkreviewid");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Fkbusinessid).HasColumnName("fkbusinessid");
            entity.Property(e => e.Fkcustomerid).HasColumnName("fkcustomerid");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property(e => e.Provider)
                .HasMaxLength(255)
                .HasColumnName("provider");
            entity.Property(e => e.Rating).HasColumnName("rating");

            entity.HasOne(d => d.Fkbusiness).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.Fkbusinessid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("review_fkbusinessid_fkey");

            entity.HasOne(d => d.Fkcustomer).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.Fkcustomerid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("review_fkcustomerid_fkey");
        });

        modelBuilder.Entity<Reward>(entity =>
        {
            entity.HasKey(e => e.Pkrewardid).HasName("reward_pkey");

            entity.ToTable("reward");

            entity.Property(e => e.Pkrewardid).HasColumnName("pkrewardid");
            entity.Property(e => e.Fkreferralid).HasColumnName("fkreferralid");
            entity.Property(e => e.Issuedate).HasColumnName("issuedate");
            entity.Property(e => e.Rewardamount).HasColumnName("rewardamount");

            entity.HasOne(d => d.Fkreferral).WithMany(p => p.Rewards)
                .HasForeignKey(d => d.Fkreferralid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reward_fkreferralid_fkey");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Pkserviceid).HasName("service_pkey");

            entity.ToTable("service");

            entity.Property(e => e.Pkserviceid).HasColumnName("pkserviceid");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Fkbusinessid).HasColumnName("fkbusinessid");
            entity.Property(e => e.Fkdiscountid)
                .HasMaxLength(5)
                .HasColumnName("fkdiscountid");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasOne(d => d.Fkbusiness).WithMany(p => p.Services)
                .HasForeignKey(d => d.Fkbusinessid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("service_fkbusinessid_fkey");

            entity.HasOne(d => d.Fkdiscount).WithMany(p => p.Services)
                .HasForeignKey(d => d.Fkdiscountid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("service_fkdiscountid_fkey");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.Pktestimonialid).HasName("testimonial_pkey");

            entity.ToTable("testimonial");

            entity.Property(e => e.Pktestimonialid).HasColumnName("pktestimonialid");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Fkbusinessid).HasColumnName("fkbusinessid");
            entity.Property(e => e.Rating).HasColumnName("rating");

            entity.HasOne(d => d.Fkbusiness).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.Fkbusinessid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("testimonial_fkbusinessid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
