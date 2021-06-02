using SmartSchool.Domain.interfaces.IRepositories;

namespace SmartSchool.Infra.Data.Repositories
{
    public class DisciplinaRepository : BaseRepository
    {
        private readonly IBaseRepository _baseRepository;
        public DisciplinaRepository(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }
    }
}
