using System.ComponentModel.DataAnnotations;

namespace Sat.Recruitment.Api
{
    public class UserDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        public string UserType { get; set; } = string.Empty;
        public decimal Money { get; set; } = 0;
    }
}
