using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Monaco.Data.Core.Entities;

namespace Monaco.Data.Test
{
    public class Class : BaseEntity
    {
        [Required]
        [Column("FGRADENO")]
        public int GradeNo { get; set; }

        [Required]
        [Column("FCLASSNO")]
        public int ClassNo { get; set; }

        [Required]
        [Column("FNAME")]
        public string Name { get; set; }
    }
}
