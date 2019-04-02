using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Monaco.Data.Core.Entities
{
    /// <summary>
    /// Represents the base class for entities
    /// </summary>
    public abstract class BaseEntity
    {
        [Column("FUID")]
        public Guid Id { get; set; }
    }
}
