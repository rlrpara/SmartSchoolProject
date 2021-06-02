using Dapper;
using System.Collections.Generic;

namespace SmartSchool.Domain.interfaces.IServices
{
    public interface IBaseService
    {
        T BuscarComJoins<T>(string sqlWhere = null, string join = null, params string[] fields);
        T BuscarTodosPorId<T>(int id) where T : class;
        T BuscarPorQuery<T>(string query);
        IEnumerable<T> BuscarTodosPorQuery<T>(string query = null) where T : class;
        IEnumerable<T> BuscarTodosPorQueryGerador<T>(string sqlWhere = "") where T : class;
        int Adicionar<T>(T entidade) where T : class;
        int Atualizar<T>(int id, T entidade) where T : class;
        int Excluir<T>(int id) where T : class;
        void ExecutarComando(string comandoSql);
        void ExecutarComandoDireto(CommandDefinition command);
        long BuscaMaxItemAsync<T>(string campo, string sqlWhere) where T : class;
    }
}
