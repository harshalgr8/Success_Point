using SuccessPointCore.Application.Interfaces;
using SucessPointCore.Domain.Entities;
using SucessPointCore.Infrastructure.Interfaces;

namespace SuccessPointCore.Application.Services
{
    public class ErrorLogService : IErrorLogService
    {
        IErrorLogRepository _errorLogRepository;
        public ErrorLogService(IErrorLogRepository errorLogRepository)
        {
            _errorLogRepository = errorLogRepository;
        }

        public bool AddError(CreateErrorLog errorLog)
        {
            return _errorLogRepository.AddError(errorLog);
        }
    }
}
