using SucessPointCore.Domain.Entities;

namespace SucessPointCore.Infrastructure.Interfaces
{
    public interface IStandardRepository
    {
        IEnumerable<Standard> GetStandardList();
        int UpsertStandard(int standardId, string standardName, int createdBy);
    }
}
