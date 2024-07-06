using SucessPointCore.Domain.Entities;

namespace SucessPointCore.Infrastructure.Interfaces
{
    public interface IStandardRepository
    {
        IEnumerable<Standard> GetStandardList();
        int CreateStandard(string standardName, int createdBy);
    }
}
