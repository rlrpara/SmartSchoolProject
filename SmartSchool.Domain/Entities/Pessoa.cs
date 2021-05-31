using SmartSchool.Domain.Enumerable;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartSchool.Domain.Entities
{
    [Table("Pessoa")]
    public class Pessoa
    {
        [Key]
        [Column("Id")]
        public int Codigo { get; set; }

        [Column("TipoPessoa")]
        public ETipoPessoa TipoPessoa { get; set; }

        [Column("Nome")]
        public string Nome { get; set; }

        [Column("Telefone")]
        public string Telefone { get; set; }
    }
}
