using SmartSchool.Domain.interfaces.IRepositories;

namespace SmartSchool.Infra.Data.Repositories
{
    public class PessoaRepository : BaseRepository
    {
        private readonly IBaseRepository _baseRepository;
        public PessoaRepository(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }
    }
}
