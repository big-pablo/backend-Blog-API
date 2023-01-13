using Blog.API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Blog.API.Models.DTOs
{
    public class UserRegisterDTO
    {
        [Required]
        [MinLength(1)]
        public string FullName { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        [Required]
        [MinLength(1)]
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        [Required]
        public GenderEnum Gender { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime AccountCreateDate { get; set; }
    }
}
