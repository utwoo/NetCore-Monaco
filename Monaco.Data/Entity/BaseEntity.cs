using System.ComponentModel.DataAnnotations.Schema;

namespace Monaco.Data.Entities
{
    /// <summary>
    /// Represents the base class for entities
    /// </summary>
    public abstract class BaseEntity
    {
        private BaseEntity() { }

        [Column("FID")]
        public int Id { get; set; }
    }
}
