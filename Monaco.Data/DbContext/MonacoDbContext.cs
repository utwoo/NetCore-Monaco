using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Monaco.Data.Mapping;

namespace Monaco.Data.DbContexts
{
    /// <summary>
    /// Represents base object context
    /// </summary>
    public class MonacoDbContext : DbContext, IDbContext
    {
        #region Ctor
        public MonacoDbContext(DbContextOptions<MonacoDbContext> options) : base(options) { }
        #endregion

        #region Utilities
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //dynamically load all entity and query type configurations
            var typesConfigurations = Assembly.GetExecutingAssembly().GetTypes().Where(type =>
                (type.BaseType?.IsGenericType ?? false)
                    && (type.BaseType.GetGenericTypeDefinition() == typeof(MonacoEntityTypeConfiguration<>)));

            foreach (var typesConfiguration in typesConfigurations)
            {
                var configuration = (IMappingConfiguration)Activator.CreateInstance(typesConfiguration);
                configuration.ApplyConfiguration(modelBuilder);
            }

            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}
