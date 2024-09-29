
using SuccessPointCore.Domain.Entities;

namespace SuccessPointCore.Infrastructure.Interfaces
{
    public interface IErrorLogRepository
    {
        bool AddError(CreateErrorLog errorData);
    }
}
