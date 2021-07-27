using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OHMI_Keeper_League.Models;

#nullable disable

namespace OHMI_Keeper_League.DAL
{
    public partial class OHMIKeeperLeagueContext : DbContext
    {
        public OHMIKeeperLeagueContext()
        {
        }

        public OHMIKeeperLeagueContext(DbContextOptions<OHMIKeeperLeagueContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Manager> Managers { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Roster> Rosters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Manager>(entity =>
            {
                entity.Property(e => e.ManagerId).HasColumnName("ManagerID");

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.Property(e => e.PlayerId).HasColumnName("PlayerID");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Position)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Team)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Roster>(entity =>
            {
                entity.Property(e => e.RosterId).HasColumnName("RosterID");

                entity.Property(e => e.Dst).HasColumnName("DST");

                entity.Property(e => e.ManagerId).HasColumnName("ManagerID");

                entity.Property(e => e.Qb1).HasColumnName("QB1");

                entity.Property(e => e.Rb1).HasColumnName("RB1");

                entity.Property(e => e.Rb2).HasColumnName("RB2");

                entity.Property(e => e.Wr1).HasColumnName("WR1");

                entity.Property(e => e.Wr2).HasColumnName("WR2");

                entity.HasOne(d => d.B1Navigation)
                    .WithMany(p => p.RosterB1Navigations)
                    .HasForeignKey(d => d.B1);

                entity.HasOne(d => d.B2Navigation)
                    .WithMany(p => p.RosterB2Navigations)
                    .HasForeignKey(d => d.B2);

                entity.HasOne(d => d.B3Navigation)
                    .WithMany(p => p.RosterB3Navigations)
                    .HasForeignKey(d => d.B3);

                entity.HasOne(d => d.B4Navigation)
                    .WithMany(p => p.RosterB4Navigations)
                    .HasForeignKey(d => d.B4);

                entity.HasOne(d => d.B5Navigation)
                    .WithMany(p => p.RosterB5Navigations)
                    .HasForeignKey(d => d.B5);

                entity.HasOne(d => d.B6Navigation)
                    .WithMany(p => p.RosterB6Navigations)
                    .HasForeignKey(d => d.B6);

                entity.HasOne(d => d.B7Navigation)
                    .WithMany(p => p.RosterB7Navigations)
                    .HasForeignKey(d => d.B7);

                entity.HasOne(d => d.DstNavigation)
                    .WithMany(p => p.RosterDstNavigations)
                    .HasForeignKey(d => d.Dst);

                entity.HasOne(d => d.FlexNavigation)
                    .WithMany(p => p.RosterFlexNavigations)
                    .HasForeignKey(d => d.Flex);

                entity.HasOne(d => d.KNavigation)
                    .WithMany(p => p.RosterKNavigations)
                    .HasForeignKey(d => d.K)
                    .HasConstraintName("FK_Rosters_Rosters_K");

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.Rosters)
                    .HasForeignKey(d => d.ManagerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rosters_Managers");

                entity.HasOne(d => d.Qb1Navigation)
                    .WithMany(p => p.RosterQb1Navigations)
                    .HasForeignKey(d => d.Qb1);

                entity.HasOne(d => d.Rb1Navigation)
                    .WithMany(p => p.RosterRb1Navigations)
                    .HasForeignKey(d => d.Rb1);

                entity.HasOne(d => d.Rb2Navigation)
                    .WithMany(p => p.RosterRb2Navigations)
                    .HasForeignKey(d => d.Rb2);

                entity.HasOne(d => d.Wr1Navigation)
                    .WithMany(p => p.RosterWr1Navigations)
                    .HasForeignKey(d => d.Wr1);

                entity.HasOne(d => d.Wr2Navigation)
                    .WithMany(p => p.RosterWr2Navigations)
                    .HasForeignKey(d => d.Wr2);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
