using Dapper;
using Dapper.Contrib.Extensions;
using SmartSchool.Domain.interfaces.IRepositories;
using SmartSchool.Infra.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmartSchool.Infra.Data.Repositories
{
    public class BaseRepository : IBaseRepository, IDisposable
    {
        #region Propriedades Privadas
        private IDbTransaction sqlTransaction;
        public IDbConnection Conexao { get; private set; }
        #endregion

        #region Construtores
        public BaseRepository()
        {
            Conexao = ConnectionConfiguration.ObterConexao();
            sqlTransaction = Conexao.BeginTransaction();
        }
        #endregion

        #region Métodos Privados
        private string ObterNomeTabela<T>()
        {
            SqlMapperExtensions.TableNameMapper = TableNameMapper;
            return TableNameMapper(typeof(T));
        }
        private static string TableNameMapper(Type type)
        {
            dynamic tableattr = type.GetCustomAttributes(false).SingleOrDefault(attr => attr.GetType().Name == "TableAttribute");
            return (tableattr != null ? tableattr.Name : string.Empty);
        }
        #endregion

        #region Métodos Públicos
        public void OpenConnection()
        {
            CloseConnection();

            if (Conexao.State == ConnectionState.Closed && Conexao.State != ConnectionState.Open)
                Conexao.Open();
        }
        public void CloseConnection()
        {
            if (Conexao.State == ConnectionState.Open && Conexao.State != ConnectionState.Closed)
                Conexao.Close();
        }
        public int Adicionar<T>(T entidade) where T : class
        {
            try
            {
                OpenConnection();
                return Conexao.Execute(GeradorDapper.RetornaInsert<T>(), GeradorDapper.ObterParametros(entidade));
            }
            catch
            {
                return default(dynamic);
            }
        }

        public int Atualizar<T>(int id, T entidade) where T : class
        {
            try
            {
                OpenConnection();
                return Conexao.Execute(GeradorDapper.RetornaUpdate(id, entidade), GeradorDapper.ObterParametros(entidade));
            }
            catch
            {
                return default(dynamic);
            }
        }

        public long BuscaMaxItemAsync<T>(string campo, string sqlWhere) where T : class
        {
            try
            {
                OpenConnection();
                var sqlPesquisa = new StringBuilder()
                    .AppendLine($"SELECT ISNULL(MAX({campo}),0) AS MAX")
                    .AppendLine($"FROM {ObterNomeTabela<T>()}")
                    .AppendLine($"WHERE {sqlWhere}");

                return SqlMapper.QueryFirstOrDefault<long>(Conexao, sqlPesquisa.ToString(), transaction: sqlTransaction) + 1;
            }
            catch
            {
                return default(dynamic);
            }
        }

        public T BuscarComJoins<T>(string sqlWhere = null, string join = null, params string[] fields)
        {
            try
            {
                OpenConnection();
                var sqlPesquisa = new StringBuilder()
                    .AppendLine($"SELECT {string.Join(", ", fields)}")
                    .AppendLine($"FROM {ObterNomeTabela<T>()} {join} {sqlWhere}");

                return SqlMapper.QueryFirstOrDefault<T>(Conexao, sqlPesquisa.ToString(), null, transaction: sqlTransaction, 80000000);
            }
            catch
            {
                return default(dynamic);
            }
        }

        public T BuscarPorQuery<T>(string query)
        {
            try
            {
                OpenConnection();
                return Conexao.QueryFirstOrDefault<T>(query, transaction: sqlTransaction, commandTimeout: 80000000, commandType: CommandType.Text);
            }
            catch
            {
                return default(dynamic);
            }
        }

        public T BuscarTodosPorId<T>(int id) where T : class
        {
            try
            {
                OpenConnection();
                return Conexao.QueryFirstOrDefault<T>($"{GeradorDapper.RetornaSelect<T>()} {GeradorDapper.ObterId<T>(id)}", transaction: sqlTransaction, commandTimeout: 80000000, commandType: CommandType.Text);
            }
            catch
            {
                return default(dynamic);
            }
        }

        public IEnumerable<T> BuscarTodosPorQuery<T>(string query = null) where T : class
        {
            try
            {
                OpenConnection();
                var sqlPesquisa = new StringBuilder();

                if (string.IsNullOrEmpty(query))
                    sqlPesquisa.AppendLine($"{GeradorDapper.RetornaSelect<T>()}");
                else
                    sqlPesquisa.AppendLine(query.Trim());

                return Conexao.Query<T>(sqlPesquisa.ToString(), transaction: sqlTransaction, commandTimeout: 80000000, commandType: CommandType.Text);
            }
            catch
            {
                return default(dynamic);
            }
        }

        public IEnumerable<T> BuscarTodosPorQueryGerador<T>(string sqlWhere = "") where T : class
        {
            try
            {
                OpenConnection();
                var sqlPesquisa = new StringBuilder();

                sqlPesquisa.AppendLine($"{GeradorDapper.RetornaSelect<T>()}");
                if(!string.IsNullOrEmpty(sqlWhere)) sqlPesquisa.AppendLine(sqlWhere);

                return Conexao.Query<T>(sqlPesquisa.ToString(), transaction: sqlTransaction, commandTimeout: 80000000, commandType: CommandType.Text);
            }
            catch
            {
                return default(dynamic);
            }
        }

        public int Excluir<T>(int id) where T : class
        {
            try
            {
                OpenConnection();
                return Conexao.Execute($"{GeradorDapper.RetornaDelete<T>()} {GeradorDapper.ObterId<T>(id)}");
            }
            catch
            {
                return default(dynamic);
            }
        }

        public void ExecutarComando(string comandoSql)
        {
            if (string.IsNullOrEmpty(comandoSql.Trim())) return;

            OpenConnection();
            var comando = Conexao.CreateCommand();
            comando.Connection = Conexao;
            comando.CommandText = comandoSql.Trim();
            comando.CommandTimeout = 0;
            comando.ExecuteNonQuery();
        }

        public void ExecutarComandoDireto(CommandDefinition command)
        {
            Conexao.Execute(command);
        }

        public void Dispose()
        {
            CloseConnection();
            Conexao.Dispose();
        }
        #endregion
    }
}
