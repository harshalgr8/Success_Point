using System.ComponentModel.DataAnnotations;

namespace SuccessPointCore.Domain.Entities.Requests
{
    public class EmailAvailableForSignupRequest
    {
        [EmailAddress(ErrorMessage = "Valid Email ID Required.")]
        public string UserEmail { get; set; }
    }
}
