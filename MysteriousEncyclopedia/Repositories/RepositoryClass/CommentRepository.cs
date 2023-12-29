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

        public async void DeleteCommentAsync(int id)
        {
            string query = "Delete from Comment where CommentID=@id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<List<CommentDto>> GetAllAsync()
        {
            string query = "Select CommentID,MysteriousEvent.EventTitle,AspNetUsers.UserName,CommentText,CommentDate,CommentStatus from Comment Inner Join MysteriousEvent on Comment.MysteryID = MysteriousEvent.EventID Inner Join AspNetUsers on Comment.UserId = AspNetUsers.Id Order by CommentID desc";
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<CommentDto>(query);
                return values.ToList();
            }
        }

        public async Task<CommentDto> GetItemAsync(int id)
        {
            string query = "Select CommentID,MysteriousEvent.EventTitle,AspNetUsers.UserName,CommentText,CommentStatus from Comment Inner Join MysteriousEvent on Comment.MysteryID = MysteriousEvent.EventID Inner Join AspNetUsers on Comment.UserId = AspNetUsers.Id Where CommentID=@id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            using (var connection = _context.CreateConnection())
            {
                var value = await connection.QueryFirstOrDefaultAsync<CommentDto>(query, parameters);
                return value;
            }
        }

        public async Task<List<CommentVisibleDto>> GetVisibleCommentsAsync(int mysteryId)
        {
            string query = "Select MysteriousEvent.EventTitle,AspNetUsers.UserName,CommentText,CommentDate from Comment Inner Join MysteriousEvent on Comment.MysteryID = MysteriousEvent.EventID Inner Join AspNetUsers on Comment.UserId = AspNetUsers.Id where CommentStatus=1 and MysteryID=@mysteryId Order by CommentID desc";
            var parameters = new DynamicParameters();
            parameters.Add("@mysteryId", mysteryId);
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<CommentVisibleDto>(query, parameters);
                return values.ToList();
            }
        }

        public async Task<int> NumberOfCommentsByEventAsync(int id)
        {
            string query = "Select Count(*) from Comment where MysteryID=@id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            using (var connection = _context.CreateConnection())
            {
                var value = await connection.QuerySingleAsync<int>(query, parameters);
                return value;
            }
        }

        public async void UpdateAsync(CommentDto entity)
        {
            string query = "Update Comment Set CommentStatus=@status where CommentID=@id";
            var parameters = new DynamicParameters();
            parameters.Add("@status", entity.CommentStatus);
            parameters.Add("@id", entity.CommentID);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}
