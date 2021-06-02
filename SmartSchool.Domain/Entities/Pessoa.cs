using SmartSchool.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartSchool.Domain.Entities
{
    [Table("Pessoa")]
    public class Pessoa
    {
        [Key]
        [Column("CD_ID")]
        public int Codigo { get; set; }

        [Column("CD_TIPO_PESSOA")]
        public ETipoPessoa TipoPessoa { get; set; }

        [Column("DS_NOME")]
        public string Nome { get; set; }

        [Column("DS_TELEFONE")]
        public string Telefone { get; set; }

        [Column("DS_EMAIL")]
        public string Email { get; set; }

        [Column("DT_CADASTRO")]
        public DateTime DataCadastro { get; set; }

        [Column("X_ATIVO")]
        public bool EstaAtivo { get; set; }
    }
}
