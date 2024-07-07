using SucessPointCore.Domain.Entities;

namespace SuccessPointCore.Application.Interfaces
{
    public interface IStandardService
    {
        int UpsertStandard(int StandardID, string standardName,int userID);

        IEnumerable<Standard> GetStandardList();
    }
}
