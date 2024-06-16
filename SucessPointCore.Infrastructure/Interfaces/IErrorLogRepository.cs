
using SucessPointCore.Domain.Entities;

namespace SucessPointCore.Infrastructure.Interfaces
{
    public interface IErrorLogRepository
    {
        bool AddError(CreateErrorLog errorData);
    }
}
