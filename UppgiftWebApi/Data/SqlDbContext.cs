using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using UppgiftWebApi.Entities;

#nullable disable

namespace UppgiftWebApi.Data
{
    public partial class SqlDbContext : DbContext
    {
        public SqlDbContext()
        {
        }

        public SqlDbContext(DbContextOptions<SqlDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Administrator> Administrators { get; set; }
        public virtual DbSet<Case> Cases { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<SessionToken> SessionTokens { get; set; }

     
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Administrator>(entity =>
            {
                entity.Property(e => e.AdminHash).IsRequired();

                entity.Property(e => e.AdminSalt).IsRequired();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Case>(entity =>
            {
                entity.Property(e => e.CaseDate).HasColumnType("datetime");

                entity.Property(e => e.CaseDescription).IsRequired();

                entity.Property(e => e.CaseStatus)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.UpdateCaseDate).HasColumnType("datetime");

                entity.HasOne(d => d.Administrator)
                    .WithMany(p => p.Cases)
                    .HasForeignKey(d => d.AdministratorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Cases__Administr__286302EC");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Cases)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Cases__CustomerI__276EDEB3");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SessionToken>(entity =>
            {
                entity.HasKey(e => e.AdminId)
                    .HasName("PK__SessionT__719FE488A82CADF5");

                entity.Property(e => e.AdminId).ValueGeneratedNever();

                entity.Property(e => e.AccessToken).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
