
using SharedContracts.Enum;
using System.ComponentModel.DataAnnotations;

namespace SharedContracts
{
    public record UserLoginRequestDTO(
        [Required]
        [EmailAddress]
        string Email,

        [Required]
        [DataType(DataType.Password)]
        string Password,

        [Required]
        [Display(Name = "Remember Me")]
        bool RememberMe
    );

    public record UserLoginResultDTO(
        LoginResult LoginResult,
        string Token
    );
}
