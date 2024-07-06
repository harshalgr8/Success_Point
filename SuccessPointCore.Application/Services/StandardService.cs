using SuccessPointCore.Application.Interfaces;
using SucessPointCore.Domain.Entities;
using SucessPointCore.Infrastructure.Interfaces;

namespace SuccessPointCore.Application.Services
{
    public class StandardService : IStandardService
    {
        IStandardRepository _repository;
        public StandardService(IStandardRepository standardRepository)
        {
            _repository = standardRepository;
        }

        public int CreateStandard(string standardName, int userID)
        {
            return _repository.CreateStandard(standardName, userID);
        }

        public IEnumerable<Standard> GetStandardList()
        {
            throw new NotImplementedException();
        }
    }
}
