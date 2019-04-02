using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Monaco.Data.Core.DbContexts;
using Monaco.Data.Core.Entities;

namespace Monaco.Data.Core.Repository
{
    /// <summary>
    /// Represents the Entity Framework repository
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbContext _context;
        private readonly ILogger<EfRepository<TEntity>> _logger;

        /// <summary>
        /// Gets an entity set
        /// </summary>
        private DbSet<TEntity> _entities => _context.Set<TEntity>();

        public EfRepository(
            MonacoDbContext context,
            ILogger<EfRepository<TEntity>> logger)
        {
            this._context = context;
            this._logger = logger;
        }

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public TEntity GetById(Guid id)
        {
            return this._entities.Find(id);
        }

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public TEntity Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var result = this._entities.Add(entity);
            this._context.SaveChanges();

            return result.Entity;
        }

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public void Insert(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            this._entities.AddRange(entities);
            this._context.SaveChanges();
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            this._entities.Update(entity);
            this._context.SaveChanges();
        }

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public void Update(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            this._entities.UpdateRange(entities);
            this._context.SaveChanges();
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            this._entities.Remove(entity);
            this._context.SaveChanges();
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public void Delete(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            this._entities.RemoveRange(entities);
            this._context.SaveChanges();
        }
    }
}
