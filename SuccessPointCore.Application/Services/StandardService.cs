using SuccessPointCore.Application.Interfaces;
using SuccessPointCore.Domain.Entities;
using SuccessPointCore.Infrastructure.Interfaces;
using System;

namespace SuccessPointCore.Application.Services
{
    public class StandardService : IStandardService
    {
        IStandardRepository _repository;
        public StandardService(IStandardRepository standardRepository)
        {
            _repository = standardRepository;
        }

        public int UpsertStandard(int StandardID, string standardName, bool active, int userID)
        {
            return _repository.UpsertStandard(StandardID, standardName,active, userID);
        }

        public IEnumerable<Standard> GetStandardList()
        {
            return _repository.GetStandardList();
        }

        public int RemoveStandard(int StandardID)
        {
            return _repository.RemoveStandard(StandardID);
        }
    }
}
