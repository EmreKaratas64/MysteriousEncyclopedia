using Dapper;
using MysteriousEncyclopedia.Models.DapperContext;
using MysteriousEncyclopedia.Models.DTOs.TopicDto;
using MysteriousEncyclopedia.Repositories.RepositoryInterface;

namespace MysteriousEncyclopedia.Repositories.RepositoryClass
{
    public class TopicRepository : ITopic
    {
        private readonly Context _context;

        public TopicRepository(Context context)
        {
            _context = context;
        }

        public async void CreateAsync(ResultTopicDto entity)
        {
            string query = "Insert Into Topic (TopicName,TopicImage) values (@TopicName,@TopicImage)";
            var parameters = new DynamicParameters();
            parameters.Add("@TopicName", entity.TopicName);
            parameters.Add("@TopicImage", entity.TopicImage);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }

        }

        public async Task<List<ResultTopicDto>> GetAllAsync()
        {
            string query = "Select * from Topic";
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<ResultTopicDto>(query);
                return values.ToList();
            }
        }

        public async Task<ResultTopicDto> GetItemAsync(int id)
        {
            string query = "Select * from Topic Where TopicID=@TopicID";
            var parameters = new DynamicParameters();
            parameters.Add("@TopicID", id);
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryFirstOrDefaultAsync<ResultTopicDto>(query, parameters);
                return values;
            }
        }

        public async Task<List<Tuple<string, int>>> GetTopicsWithNumberOfEventsAsync()
        {
            var query = "SELECT t.TopicName, COUNT(me.EventID) as NumberOfTopics FROM Topic t LEFT JOIN MysteriousEvent me ON CHARINDEX(t.TopicName, me.EventTopics) > 0 GROUP BY t.TopicName";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync(query);
                return result.Select(r => Tuple.Create((string)r.TopicName, (int)r.NumberOfTopics)).ToList();
            }
        }

        public async void UpdateAsync(ResultTopicDto entity)
        {
            string query = "Update Topic Set TopicName=@topicName,TopicImage=@topicImage where TopicID=@topicID";
            var parameters = new DynamicParameters();
            parameters.Add("@topicName", entity.TopicName);
            parameters.Add("@topicImage", entity.TopicImage);
            parameters.Add("@topicID", entity.TopicID);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}
