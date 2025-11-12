using System.ComponentModel.DataAnnotations;
namespace UserCrudRepo.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        [Required, EmailAddress, StringLength(254)]
        [RegularExpression(@"^[^@\s]+@gmail\.com$", ErrorMessage = "Only Gmail addresses are allowed.")]
        public string? Email { get; set; } = string.Empty;
        
        [Range(1,2,ErrorMessage = "Status must be 1 (Active) or 2 (Inactive)")]
        public int Status { get; set; } = 1;
    }
}
