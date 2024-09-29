using SuccessPointCore.Domain.Entities;

namespace SuccessPointCore.Application.Interfaces
{
    public interface IStandardService
    {
        int UpsertStandard(int StandardID, string standardName, bool active, int userID);

        IEnumerable<Standard> GetStandardList();

        int RemoveStandard(int StandardID);
    }
}
