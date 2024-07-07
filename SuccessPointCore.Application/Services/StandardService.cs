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

        public int UpsertStandard(int StandardID, string standardName, int userID)
        {
            return _repository.UpsertStandard(StandardID, standardName, userID);
        }

        public IEnumerable<Standard> GetStandardList()
        {
            throw new NotImplementedException();
        }
    }
}
