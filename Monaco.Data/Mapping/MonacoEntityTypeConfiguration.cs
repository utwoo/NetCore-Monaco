using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Monaco.Data.Entities;

namespace Monaco.Data.Mapping
{
    /// <summary>
    /// Represents base entity mapping configuration
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public class MonacoEntityTypeConfiguration<TEntity> : IMappingConfiguration, IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        public void ApplyConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(this);
        }

        public void Configure(EntityTypeBuilder<TEntity> builder) { }
    }
}
