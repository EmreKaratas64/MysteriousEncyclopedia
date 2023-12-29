namespace MysteriousEncyclopedia.Models.DTOs.Comment
{
    public class CommentVisibleDto
    {
        public string? EventTitle { get; set; }

        public string? UserName { get; set; }

        public string CommentText { get; set; }

        public DateTime CommentDate { get; set; }
    }
}
