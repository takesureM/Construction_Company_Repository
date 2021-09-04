using AuthorizationService.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace AuthorizationService
{
    public partial class AuthorizationDbContext : DbContext
    {
        public AuthorizationDbContext() 
        {

        }
        public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Login> Logins { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        internal object GetCollection<T>()
        {
            throw new NotImplementedException();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasKey(e => e.EntityId).HasName("PK_Account_EntityId");

            modelBuilder.Entity<Account>(entity =>
            { 

                entity.HasOne(c => c.Login)
                    .WithOne(c => c.Account);

                entity.Property(p => p.NickName)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(p => p.IsDeleted)
                    .IsRequired();


            });

            modelBuilder.Entity<Login>(entity =>
            {
                entity.Property(p => p.Email)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(p => p.PasswordHash)
                    .IsRequired();

                entity.Property(p => p.Salt)
                    .IsRequired();
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasOne(p => p.Account)
                    .WithMany(p => p.RefreshTokens)
                    .HasForeignKey(p => p.AccountId);
            });

         

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
