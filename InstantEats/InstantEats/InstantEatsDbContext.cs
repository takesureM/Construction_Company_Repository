using InstantEats.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstantEats
{
    public partial class InstantEatsDbContext : DbContext
    {
        public InstantEatsDbContext()
        {

        }

        public InstantEatsDbContext(DbContextOptions<InstantEatsDbContext> options)
            :base(options)
        {

        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<LoginModel> LoginModels { get; set; }
        public virtual DbSet<Institution>  Institutions { get; set; }
        public virtual DbSet<Delivery> Deliveries { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

            });

            modelBuilder.Entity<LoginModel>(entity =>
            {
                entity.ToTable("LoginModel");

            });

            modelBuilder.Entity<Institution>(entity =>
            {
                entity.ToTable("Institution");

            });

            modelBuilder.Entity<Delivery>(entity =>
            {
                entity.ToTable("Delivery");

            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Client");

            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

            });


        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
