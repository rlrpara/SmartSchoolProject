using SmartSchool.Domain.interfaces.IRepositories;

namespace SmartSchool.Infra.Data.Repositories
{
    public class UsuarioRepository : BaseRepository
    {
        private readonly IBaseRepository _baseRepository;
        public UsuarioRepository(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }
    }
}
