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
            string query = "Insert Into Request (RequestNameSurname,RequestUserName,RequestEventTitle,RequestDescription,RequestDate, RequestStatus) values (@namesurname,@username,@eventTitle,@description,@date,@status)";
            var parameters = new DynamicParameters();
            parameters.Add("@namesurname", entity.RequestNameSurname);
            parameters.Add("@username", entity.RequestUserName);
            parameters.Add("@eventTitle", entity.RequestEventTitle);
            parameters.Add("@description", entity.RequestDescription);
            parameters.Add("@date", entity.RequestDate);
            parameters.Add("@status", entity.RequestStatus);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public Task<List<RequestDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RequestDto> GetItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateAsync(RequestDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
