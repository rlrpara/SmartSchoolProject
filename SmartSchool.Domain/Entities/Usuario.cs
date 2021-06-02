using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartSchool.Domain.Entities
{
    public class Usuario
    {
        [Key]
        [Column("CD_ID")]
        public int Codigo { get; set; }

        [Column("DS_NOME")]
        public string Nome { get; set; }

        [Column("DS_EMAIL")]
        public string Email { get; set; }

        [Column("DS_SENHA")]
        public string Senha { get; set; }

        [Column("DT_CADASTRO")]
        public DateTime DataCadastro { get; set; }

        [Column("X_ATIVO")]
        public bool EstaAtivo { get; set; }
    }
}
