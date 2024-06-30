using System.ComponentModel.DataAnnotations;

namespace SucessPointCore.Domain.Entities.Requests
{
    public class EmailAvailableForSignupRequest
    {
        [EmailAddress(ErrorMessage = "Valid Email ID Required.")]
        public string UserEmail { get; set; }
    }
}
