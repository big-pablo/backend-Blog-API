using Blog.API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Blog.API.Models.DTOs
{
    public class UserEditDTO
    {
        [Required]
        [MinLength(1)]
        public string Email { get; set; }
        [Required]
        [MinLength(1)]
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        [Required]
        public GenderEnum Gender { get; set; }
        public string PhoneNumber { get; set; }
    }
}
