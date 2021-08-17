using AppGlobal.Constants;
using AppGlobal.Entities;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;

namespace AppGlobal.DBContext
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        private void AuditEntities()
        {
            var now = DateTime.Now.ToUniversalTime();
            var active = (int)StatusType.Active;

            foreach (EntityEntry<TrackEntity> entry in ChangeTracker.Entries<TrackEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DateCreated").CurrentValue = now;
                    entry.Property("DateUpdated").CurrentValue = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property("DateUpdated").CurrentValue = now;
                }
            }

            foreach (EntityEntry<AuditEntity> entry in ChangeTracker.Entries<AuditEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("StatusID").CurrentValue = active;

                    entry.Property("DateCreated").CurrentValue = now;
                    entry.Property("DateUpdated").CurrentValue = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property("DateUpdated").CurrentValue = now;
                }
            }
        }

        public void BulkInsert<TEntity>(IList<TEntity> listEntities) where TEntity : class
        {
            AuditEntities();
            DbContextBulkExtensions.BulkInsert(this, listEntities);
        }

        public void BulkUpdate<TEntity>(IList<TEntity> listEntities) where TEntity : class
        {
            AuditEntities();
            DbContextBulkExtensions.BulkUpdate(this, listEntities);
        }

        public override int SaveChanges()
        {
            AuditEntities();
            return base.SaveChanges();
        }

        public int SoftDelete<TEntity>(IList<TEntity> listEntities) where TEntity : AuditEntity
        {
            var deleted = (int)StatusType.Deleted;
            var now = DateTime.Now.ToUniversalTime();

            foreach (var entity in listEntities)
            {
                entity.StatusID = deleted;
                entity.DateUpdated = now;
            }

            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
