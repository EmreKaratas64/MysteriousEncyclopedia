using Dapper;
using MysteriousEncyclopedia.Models.DapperContext;
using MysteriousEncyclopedia.Models.DTOs.Comment;
using MysteriousEncyclopedia.Repositories.RepositoryInterface;

namespace MysteriousEncyclopedia.Repositories.RepositoryClass
{
    public class CommentRepository : IComment
    {
        private readonly Context _context;

        public CommentRepository(Context context)
        {
            _context = context;
        }

        public async void CreateAsync(CommentDto entity)
        {
            string query = "Insert Into Comment (MysteryID,UserId,CommentText,CommentDate,CommentStatus) values (@mysteryId,@UserId,@Text,@Date,@Status)";
            var parameters = new DynamicParameters();
            parameters.Add("@mysteryId", entity.MysteryID);
            parameters.Add("@UserId", entity.UserId);
            parameters.Add("@Text", entity.CommentText);
            parameters.Add("@Date", DateTime.Now);
            parameters.Add("@Status", false);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<List<CommentDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<CommentDto> GetItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async void UpdateAsync(CommentDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
