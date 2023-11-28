using System.ComponentModel.DataAnnotations;

namespace Login_System.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress] 
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
