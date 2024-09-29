using SuccessPointCore.Domain.Entities;

namespace SuccessPointCore.Application.Interfaces
{
    public interface IErrorLogService
    {
        bool AddError(CreateErrorLog errorLog);
    }
}
