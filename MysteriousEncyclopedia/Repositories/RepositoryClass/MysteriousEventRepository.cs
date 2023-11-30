using Dapper;
using MysteriousEncyclopedia.Models.DapperContext;
using MysteriousEncyclopedia.Models.DTOs.EventDto;
using MysteriousEncyclopedia.Repositories.RepositoryInterface;

namespace MysteriousEncyclopedia.Repositories.RepositoryClass
{
    public class MysteriousEventRepository : IMysteriousEvent
    {
        private readonly Context _context;

        public MysteriousEventRepository(Context context)
        {
            _context = context;
        }

        public async void CreateAsync(MysteriousEventDto entity)
        {
            string query = "Insert Into MysteriousEvent (EventTitle,EventImage,EventTopics,EventDate,EventModifiedDate,EventLocation,EventContent,EventStatus,EventVisible) values (@title,@image,@topics,@date,@modifiedDate,@location,@content,@status,@visible)";
            var parameters = new DynamicParameters();
            parameters.Add("@title", entity.EventTitle);
            parameters.Add("@image", entity.EventImage);
            parameters.Add("@topics", entity.EventTopics);
            parameters.Add("@date", entity.EventDate);
            parameters.Add("@modifiedDate", entity.EventModifiedDate);
            parameters.Add("@location", entity.EventLocation);
            parameters.Add("@content", entity.EventContent);
            parameters.Add("@status", entity.EventStatus);
            parameters.Add("@visible", entity.EventVisible);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<List<MysteriousEventDto>> GetAllAsync()
        {
            string query = "Select * from MysteriousEvent Order by EventID desc";
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<MysteriousEventDto>(query);
                return values.ToList();
            }
        }

        public async Task<List<MysteriousEventDto>> GetVisibleEventsByTopicAsync(string topic)
        {
            string query = "Select * from MysteriousEvent WHERE EventVisible=1 and EventTopics COLLATE SQL_Latin1_General_CP1_CI_AS LIKE '%' + @Topic + '%' Order by EventID desc";
            var parameters = new DynamicParameters();
            parameters.Add("@Topic", topic);
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<MysteriousEventDto>(query, parameters);
                return values.ToList();
            }
        }

        public async Task<MysteriousEventDto> GetItemAsync(int id)
        {
            string qurey = "Select * from MysteriousEvent where EventID=@eventID";
            var parameters = new DynamicParameters();
            parameters.Add("@eventID", id);
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryFirstOrDefaultAsync<MysteriousEventDto>(qurey, parameters);
                return values;
            }
        }

        public async Task<List<MysteriousEventDto>> GetVisibleLast6EventsAsync()
        {
            string query = "Select top 6 * from MysteriousEvent where EventVisible=1 Order by EventID desc";
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<MysteriousEventDto>(query);
                return values.ToList();
            }
        }

        public async Task<List<MysteriousEventDto>> GetVisibleEventsAsync()
        {
            string query = "Select * from MysteriousEvent where EventVisible=1 Order by EventID desc";
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<MysteriousEventDto>(query);
                return values.ToList();
            }
        }

        public async void UpdateAsync(MysteriousEventDto entity)
        {
            string query = "Update MysteriousEvent Set EventTitle=@title,EventImage=@image,EventTopics=@topics,EventDate=@date,EventModifiedDate=@modifiedDate,EventLocation=@location,EventContent=@content,EventStatus=@status,EventVisible=@visible where EventID=@eventID";
            var parameters = new DynamicParameters();
            parameters.Add("@title", entity.EventTitle);
            parameters.Add("@image", entity.EventImage);
            parameters.Add("@topics", entity.EventTopics);
            parameters.Add("@date", entity.EventDate);
            parameters.Add("@modifiedDate", entity.EventModifiedDate);
            parameters.Add("@location", entity.EventLocation);
            parameters.Add("@content", entity.EventContent);
            parameters.Add("@status", entity.EventStatus);
            parameters.Add("@visible", entity.EventVisible);
            parameters.Add("@eventID", entity.EventID);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<MysteriousEventDto> GetVisibleItemAsync(int id)
        {
            string qurey = "Select* from MysteriousEvent where EventVisible=1 and EventID=@eventID";
            var parameters = new DynamicParameters();
            parameters.Add("@eventID", id);
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryFirstOrDefaultAsync<MysteriousEventDto>(qurey, parameters);
                return values;
            }
        }
    }
}
