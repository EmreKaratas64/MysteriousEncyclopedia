using Dapper;
using MysteriousEncyclopedia.Models.DapperContext;
using MysteriousEncyclopedia.Models.DTOs.Request;
using MysteriousEncyclopedia.Repositories.RepositoryInterface;

namespace MysteriousEncyclopedia.Repositories.RepositoryClass
{
    public class RequestRepository : IRequest
    {
        private readonly Context _context;

        public RequestRepository(Context context)
        {
            _context = context;
        }

        public async void CreateAsync(RequestDto entity)
        {
            string query = "Insert Into Request (RequestNameSurname,RequestUserId,RequestEventId,RequestDescription,RequestDate, RequestStatus) values (@namesurname,@username,@eventTitle,@description,@date,@status)";
            var parameters = new DynamicParameters();
            parameters.Add("@namesurname", entity.RequestNameSurname);
            parameters.Add("@username", entity.RequestUserId);
            parameters.Add("@eventTitle", entity.RequestEventId);
            parameters.Add("@description", entity.RequestDescription);
            parameters.Add("@date", DateTime.Now);
            parameters.Add("@status", entity.RequestStatus);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async void DeleteRequestAsync(int Id)
        {
            string query = "Delete from Request where RequestID=@id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", Id);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<List<RequestDto>> GetAllAsync()
        {
            string query = "Select RequestID,RequestNameSurname,AspNetUsers.UserName,MysteriousEvent.EventTitle,RequestDate,RequestStatus from Request Inner Join AspNetUsers on Request.RequestUserId = AspNetUsers.Id Inner Join MysteriousEvent on Request.RequestEventId = MysteriousEvent.EventID order by RequestID desc";
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<RequestDto>(query);
                return values.ToList();
            }
        }

        public async Task<RequestDto> GetItemAsync(int Id)
        {
            string query = "Select RequestID,RequestNameSurname,AspNetUsers.UserName,MysteriousEvent.EventTitle,RequestDescription,RequestStatus from Request Inner Join AspNetUsers on Request.RequestUserId = AspNetUsers.Id Inner Join MysteriousEvent on Request.RequestEventId = MysteriousEvent.EventID where RequestID=@id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", Id);
            using (var connection = _context.CreateConnection())
            {
                var value = await connection.QueryFirstOrDefaultAsync<RequestDto>(query, parameters);
                return value;
            }
        }

        public async void UpdateAsync(RequestDto entity)
        {
            string query = "Update Request Set RequestStatus=@requestStatus where RequestID=@requestId";
            var parameters = new DynamicParameters();
            parameters.Add("@requestStatus", entity.RequestStatus);
            parameters.Add("@requestId", entity.RequestID);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}
