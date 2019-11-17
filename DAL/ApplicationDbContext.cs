using DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DAL.Models.Interfaces;

namespace DAL
{
    public class ApplicationDbContext : DbContext
    {
        public string CurrentUserId { get; set; }

        public DbSet<RegisteredUser> RegisteredUsers { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);            

            builder.Entity<RegisteredUser>().Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.Entity<RegisteredUser>().HasIndex(c => c.Name);
            builder.Entity<RegisteredUser>().Property(c => c.Email).HasMaxLength(100);
            builder.Entity<RegisteredUser>().Property(c => c.PhoneNumber).IsUnicode(false).HasMaxLength(30);
            builder.Entity<RegisteredUser>().Property(c => c.City).HasMaxLength(50);
            builder.Entity<RegisteredUser>().ToTable($"App{nameof(this.RegisteredUsers)}");

            builder.Entity<Book>().Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Entity<Book>().HasIndex(p => p.Name);
            builder.Entity<Book>().Property(p => p.Description).HasMaxLength(500);
            builder.Entity<Book>().ToTable($"App{nameof(this.Books)}");

            builder.Entity<Reservation>().Property(o => o.Comments).HasMaxLength(500);
            builder.Entity<Reservation>().ToTable($"App{nameof(this.Reservations)}");
        }




        public override int SaveChanges()
        {
            UpdateAuditEntities();
            return base.SaveChanges();
        }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateAuditEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(cancellationToken);
        }


        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }


        private void UpdateAuditEntities()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));


            foreach (var entry in modifiedEntries)
            {
                var entity = (IAuditableEntity)entry.Entity;
                DateTime now = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = now;
                    entity.CreatedBy = CurrentUserId;
                }
                else
                {
                    base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                    base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                }

                entity.UpdatedDate = now;
                entity.UpdatedBy = CurrentUserId;
            }
        }
    }
}
