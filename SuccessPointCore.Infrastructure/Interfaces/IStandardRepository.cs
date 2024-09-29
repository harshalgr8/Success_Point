using SuccessPointCore.Domain.Entities;

namespace SuccessPointCore.Infrastructure.Interfaces
{
    public interface IStandardRepository
    {
        IEnumerable<Standard> GetStandardList();
        int UpsertStandard(int standardId, string standardName, bool active, int createdBy);

        int RemoveStandard(int StandardID);
    }
}
