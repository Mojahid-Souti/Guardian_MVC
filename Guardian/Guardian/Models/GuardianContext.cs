using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Guardian.Models;

public partial class GuardianContext : DbContext
{
    public GuardianContext()
    {
    }

    public GuardianContext(DbContextOptions<GuardianContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Incident> Incidents { get; set; }

    public virtual DbSet<PacketEntry> PacketEntries { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(WebApplication.CreateBuilder().Configuration.GetConnectionString("Guardian"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Incident>(entity =>
        {
            entity.HasKey(e => new { e.IncidentId, e.PacketEntryId });

            entity.Property(e => e.IncidentId).ValueGeneratedOnAdd();
            entity.Property(e => e.AssignedTo).HasMaxLength(100);
            entity.Property(e => e.ResolvedAt).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Open");

            entity.HasOne(d => d.PacketEntry).WithMany(p => p.Incidents)
                .HasForeignKey(d => d.PacketEntryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Incidents_PacketEntries");
        });

        modelBuilder.Entity<PacketEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PacketEn__3214EC078903C3FE");

            entity.Property(e => e.Attack).HasMaxLength(255);
            entity.Property(e => e.Dst).HasMaxLength(255);
            entity.Property(e => e.Src).HasMaxLength(255);
            entity.Property(e => e.ThreatLevel).HasMaxLength(50);
            entity.Property(e => e.Timestamp).HasColumnType("datetime");

            entity.HasOne(d => d.Service).WithMany(p => p.PacketEntries)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PacketEntries_Services");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Services__C51BB00A6E7F665F");

            entity.Property(e => e.ServiceId).ValueGeneratedNever();
            entity.Property(e => e.ServiceName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C3B3EF2E8");

            entity.HasIndex(e => e.EmailAddress, "IX_Users_EmailAddress").IsUnique();

            entity.HasIndex(e => e.FullName, "IX_Users_FullName").IsUnique();

            entity.HasIndex(e => e.EmailAddress, "UQ__Users__49A14740F3B63A9C").IsUnique();

            entity.Property(e => e.EmailAddress)
                .HasMaxLength(255)
                .HasDefaultValue("");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
