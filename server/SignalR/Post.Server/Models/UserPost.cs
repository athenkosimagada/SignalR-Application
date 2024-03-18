using Post.Server.Models.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Server.Models
{
    public class UserPost
    {
        [Key]
        public int PostId { get; set; }
        public string UserId { get; set; } = string.Empty;
        [NotMapped]
        public ApplicationUserDto ApplicationUser { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
