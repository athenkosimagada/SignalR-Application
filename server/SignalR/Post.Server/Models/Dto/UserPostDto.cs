using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Server.Models.Dto
{
    public class UserPostDto
    {
        public int PostId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUserDto? ApplicationUser { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
