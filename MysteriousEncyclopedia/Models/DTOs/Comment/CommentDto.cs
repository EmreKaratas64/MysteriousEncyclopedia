using System.ComponentModel.DataAnnotations;

namespace MysteriousEncyclopedia.Models.DTOs.Comment
{
    public class CommentDto
    {
        [Required(ErrorMessage = "Mystery value cannot be empty!")]
        public int MysteryID { get; set; }

        public string? UserId { get; set; }

        [Required(ErrorMessage = "Comment cannot be empty!")]
        [MaxLength(500, ErrorMessage = "Comment cannot be empty!")]
        public string CommentText { get; set; }

        public bool CommentStatus { get; set; }
    }
}
