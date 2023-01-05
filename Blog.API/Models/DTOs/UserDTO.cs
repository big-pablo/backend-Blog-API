using Blog.API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Blog.API.Models.DTOs
{
    public class UserDTO
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [MinLength(1)]
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        [Required]
        public GenderEnum Gender { get; set; }
        [Required]
        [MinLength(1)]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

    }
}
