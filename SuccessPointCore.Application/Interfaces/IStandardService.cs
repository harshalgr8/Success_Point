using SucessPointCore.Domain.Entities;

namespace SuccessPointCore.Application.Interfaces
{
    public interface IStandardService
    {
        int CreateStandard(string standardName,int userID);

        IEnumerable<Standard> GetStandardList();
    }
}
