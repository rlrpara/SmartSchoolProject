using Dapper;
using SmartSchool.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SmartSchool.Infra.Utilities
{
    public static class GeradorDapper
    {
        #region Métodos Privados
        private static bool EhBrancoNulo(string valor) => string.IsNullOrWhiteSpace(valor.ToString().Trim());
        private static object ObterCamposInsert<T>() where T : class
        {
            var retorno = new StringBuilder();

            foreach (var field in typeof(T).GetProperties())
            {
                var campo = field.GetCustomAttribute<ColumnAttribute>()?.Name ?? "";

                if (field.GetCustomAttribute<KeyAttribute>() == null)
                {
                    if (EhBrancoNulo(retorno.ToString().Trim()) && !EhBrancoNulo(campo))
                        retorno.Append($"{campo}");
                    else if (!EhBrancoNulo(campo.Trim()))
                        retorno.Append($", {campo}");
                }
            }
            return retorno.ToString().Trim();
        }
        private static string ObterCampoSelect<T>() where T : class
        {
            var retorno = new StringBuilder();

            foreach (var field in typeof(T).GetProperties())
            {
                var campo = field.GetCustomAttribute<ColumnAttribute>()?.Name ?? "";
                var alias = field?.Name ?? "";

                if (EhBrancoNulo(retorno.ToString().Trim()) && !EhBrancoNulo(campo))
                    retorno.AppendLine($"{campo} as {alias} ");
                else if (!EhBrancoNulo(campo.Trim()))
                    retorno.AppendLine($"{(campo.Length == 0 ? campo : new string(' ', 5)) + ", " + campo} as {alias} ");
            }
            return retorno.ToString().Trim();
        }
        private static string ObterNomeTabela<T>() where T : class
        {
            dynamic tableattr = typeof(T).GetCustomAttributes(false).SingleOrDefault(attr => attr.GetType().Name == "TableAttribute");

            return (tableattr != null ? tableattr.Name : "");
        }
        public static DynamicParameters ObterParametros<T>(T entidade) 
        {
            var dbArgs = new DynamicParameters();
            foreach (var field in entidade.GetType().GetProperties())
            {
                var obterCampo = field.GetCustomAttribute<ColumnAttribute>()?.Name ?? "";
                var obterValor = field.GetValue(entidade);

                if (!EhBrancoNulo(obterCampo) && !obterValor.ToString().Contains("01/01/0001 12:00:00 AM") && !obterValor.ToString().Equals("0"))
                {
                    if (obterValor is DateTime)
                        dbArgs.Add($"@{field.Name}", EhBrancoNulo(obterValor?.ToString() ?? "") ? null : Convert.ToDateTime(obterValor.ToString()).ToString("yyyy-MM-dd HH:mm:ss"));

                    else if (obterValor is bool)
                        dbArgs.Add($"@{field.Name}", EhBrancoNulo(obterValor?.ToString() ?? "") ? null : Convert.ToBoolean(obterValor?.ToString()));

                    else if (field.GetCustomAttribute<KeyAttribute>() == null)
                        dbArgs.Add($"@{field.Name}", EhBrancoNulo(obterValor?.ToString() ?? "") ? null : obterValor?.ToString());
                }
            }

            return dbArgs;
        }

        private static string ObterValoresInsert<T>() where T : class
        {
            var retorno = new StringBuilder();

            foreach (var field in typeof(T).GetProperties())
            {
                var campo = field.GetCustomAttribute<ColumnAttribute>()?.Name ?? "";
                var chave = field?.Name ?? "";

                if (field.GetCustomAttribute<KeyAttribute>() == null)
                {
                    if (EhBrancoNulo(retorno.ToString().Trim()) && !EhBrancoNulo(campo))
                        retorno.Append($"@{chave}");
                    else if (!EhBrancoNulo(campo.Trim()))
                        retorno.Append($", @{chave}");
                }
            }
            return retorno.ToString().Trim();
        }
        private static string ObterSetUpdate<T>(T entidade) where T : class
        {
            var retorno = new StringBuilder();

            foreach (var field in entidade.GetType().GetProperties())
            {
                var campo = field.GetCustomAttribute<ColumnAttribute>()?.Name ?? "";
                var chave = field?.Name ?? "";
                var valor = field.GetValue(entidade)?.ToString() ?? "";

                if (field.GetCustomAttribute<KeyAttribute>() == null)
                {
                    if (EhBrancoNulo(retorno.ToString().Trim()) && !EhBrancoNulo(campo))
                        retorno.AppendLine($"   SET {campo} = @{chave}");
                    else if (!EhBrancoNulo(campo.Trim()))
                        retorno.AppendLine($"     , {campo} = @{chave}");
                }
            }
            return retorno.ToString();
        }

        #endregion

        #region Métodos Públicos
        public static string RetornaSelect<T>() where T : class
        {
            var sqlPesquisa = new StringBuilder();

            sqlPesquisa.AppendLine($"SELECT {ObterCampoSelect<T>()}");
            sqlPesquisa.AppendLine($"  FROM {ObterNomeTabela<T>()}");

            return sqlPesquisa.ToString();
        }
        public static string RetornaInsert<T>() where T : class
        {
            var sqlInsert = new StringBuilder();
            sqlInsert.AppendLine($"INSERT INTO {ObterNomeTabela<T>()} ({ObterCamposInsert<T>()})");
            sqlInsert.AppendLine($"            VALUES ({ObterValoresInsert<T>()});");

            return sqlInsert.ToString();
        }
        public static string RetornaUpdate<T>(int id, T entidade) where T : class
        {
            var sqlAtualizar = new StringBuilder();

            sqlAtualizar.AppendLine($"UPDATE {ObterNomeTabela<T>()}");
            sqlAtualizar.AppendLine($"{ObterSetUpdate(entidade)}");
            sqlAtualizar.AppendLine($"{ObterId<T>(id)}");

            return sqlAtualizar.ToString();
        }
        public static string RetornaDelete<T>() where T : class
        {
            var sqlInsert = new StringBuilder();
            sqlInsert.AppendLine($"DELETE FROM {ObterNomeTabela<T>()} ");

            return sqlInsert.ToString();
        }
        public static string ObterId<T>(int id)
        {
            var retorno = new StringBuilder();

            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                KeyAttribute attribute = Attribute.GetCustomAttribute(property, typeof(KeyAttribute)) as KeyAttribute;
                if (attribute != null)
                    retorno.AppendLine($" WHERE {property.GetCustomAttribute<ColumnAttribute>()?.Name ?? ""} = {id}");
            }
            return retorno.ToString();
        }
        #endregion
    }
}
