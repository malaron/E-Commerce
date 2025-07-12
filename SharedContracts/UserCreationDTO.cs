using System.ComponentModel.DataAnnotations;

namespace SharedContracts
{
    public record UserCreationRequestDTO(
    
        [Required]
        [EmailAddress]
        string Email,

        [Required]
        string FirstName,

        [Required]
        string LastName,

        [Required]
        [DataType(DataType.Password)]
        string Password,

        [Required]
        [DataType(DataType.Password)]
        string ConfirmPassword
    );

    public record UserCreationResponseDTO(string Token);
}
