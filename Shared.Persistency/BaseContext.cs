 
using System;
using System.Collections.Generic;
using System.Data; 
using System.Threading.Tasks;
using MediatR; 
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Shared.Domain;

namespace Shared.Persistency
{
    public abstract class BaseContext : DbContext
    {

        protected BaseContext(string schema, IMediator mediator)
        {
            this.schema = schema;
            this.mediator = mediator;
        }

        private IDbContextTransaction currentTransaction;
        private readonly string schema;
        private readonly IMediator mediator; 
 
        public abstract Task EnsureSeedData();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           optionsBuilder.UseSqlServer(DatabaseConfiguration.GetConnectionString(),
               options => options.EnableRetryOnFailure());
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema(schema);
            modelBuilder.Ignore<State>();
 

            modelBuilder.RemovePluralizingTableNameConvention(); 
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            { 
                modelBuilder.Entity(entityType.Name).Ignore("State");

            }
        }

        public void SetSaveState(BaseEntity entity)
        { 

            if (entity.State == SaveState.UnSaved)
            {
                Entry(entity).State = EntityState.Added;
            }
            else
            {
                Entry(entity).State = EntityState.Modified;
            } 
        } 

        public IDbContextTransaction BeginTransaction()
        {
            if (currentTransaction != null)
            {
                return null;
            }

            currentTransaction = Database.BeginTransaction(IsolationLevel.ReadCommitted);
            return currentTransaction;
        }

        private void CommitAndSave()
        {
            try
            {
                SaveChanges();

                currentTransaction?.Commit(); 
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (currentTransaction != null)
                {
                    currentTransaction.Dispose();
                    currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                currentTransaction?.Rollback();
            }
            finally
            {
                if (currentTransaction != null)
                {
                    currentTransaction.Dispose();
                    currentTransaction = null;
                }
            }
        }

        public void CommitTransaction()
        {
            try
            { 
                currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (currentTransaction != null)
                {
                    currentTransaction.Dispose();
                    currentTransaction = null;
                }
            }
        }
 

        public async Task Publish<T>(T entity) where T : BaseEntity
        {
            foreach (var domainEvent in entity.Events)
            {
                await mediator.Publish(domainEvent);
            }
        }

        public async Task Publish<T>(IEnumerable<T> entities) where T : BaseEntity
        {
            foreach (var entity in entities)
            {
                foreach (var domainEvent in entity.Events)
                {
                    await mediator.Publish(domainEvent);
                }
            }
        }

        public async Task<T> SaveAndPublishAsync<T>(T entity, DbSet<T> set) where T : BaseEntity
        {
          //  BeginTransaction();
             
            try
            {
                SetSaveState(entity);

                if (entity.State == SaveState.UnSaved)
                {
                    set.Add(entity);
                }

                SaveChanges();
                //CommitAndSave();

                entity.SetSaved();

                await Publish(entity);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }


            return entity;
        }

        public int Save<T>(T entity, DbSet<T> set) where T : BaseEntity
        {
            SetSaveState(entity);

            if (entity.State == SaveState.UnSaved)
            {
                set.Add(entity);
            }

            int changes = SaveChanges();


            entity.SetSaved();

            return changes;
        }

        public void Delete(BaseEntity entity)
        {
          //  BeginTransaction();

            Entry(entity).State = EntityState.Deleted;
            SaveChanges();
            // CommitAndSave();
        }
    }

}
