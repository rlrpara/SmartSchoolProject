using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartSchool.Domain.Entities
{
    [Table("Disciplina")]
    public class Disciplina
    {
        [Key]
        [Column("CD_ID")]
        public int Codigo { get; set; }

        [Column("DS_NOME")]
        public string NomeCompleto { get; set; }
    }
}
