using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartSchool.Domain.Entities
{
    [Table("Disciplina")]
    public class Disciplina
    {
        [Key]
        [Column("Id")]
        public int Codigo { get; set; }

        [Column("Nome")]
        public string Nome { get; set; }
    }
}
