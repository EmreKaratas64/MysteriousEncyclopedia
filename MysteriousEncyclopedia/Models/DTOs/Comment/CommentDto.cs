using System.ComponentModel.DataAnnotations;

namespace MysteriousEncyclopedia.Models.DTOs.Comment
{
    public class CommentDto
    {
        public int CommentID { get; set; }

        [Required(ErrorMessage = "Mystery value cannot be empty!")]
        public int MysteryID { get; set; }

        public string? EventTitle { get; set; }

        public string? UserId { get; set; }

        public string? UserName { get; set; }


        [Required(ErrorMessage = "Comment cannot be empty!")]
        [MaxLength(500, ErrorMessage = "Comment cannot be empty!")]
        public string CommentText { get; set; }

        public DateTime CommentDate { get; set; }

        public bool CommentStatus { get; set; }
    }
}
