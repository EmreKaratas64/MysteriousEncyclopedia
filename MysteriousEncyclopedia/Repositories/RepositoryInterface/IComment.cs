using MysteriousEncyclopedia.Models.DTOs.Comment;

namespace MysteriousEncyclopedia.Repositories.RepositoryInterface
{
    public interface IComment : IGeneric<CommentDto>
    {
        Task<List<CommentVisibleDto>> GetVisibleCommentsAsync(int mysteryId);

        void DeleteCommentAsync(int id);

        Task<int> NumberOfCommentsByEventAsync(int id);
    }
}
