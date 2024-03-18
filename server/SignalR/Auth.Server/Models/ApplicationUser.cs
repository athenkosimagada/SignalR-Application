using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Auth.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string UserName { get; set; } = "@athenkosi_rsa";
        [Required]
        public string FullName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
