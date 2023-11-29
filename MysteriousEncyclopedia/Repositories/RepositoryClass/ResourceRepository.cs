using Dapper;
using MysteriousEncyclopedia.Models.DapperContext;
using MysteriousEncyclopedia.Models.DTOs.ReferenceDto;
using MysteriousEncyclopedia.Models.DTOs.ResourceDto;
using MysteriousEncyclopedia.Repositories.RepositoryInterface;

namespace MysteriousEncyclopedia.Repositories.RepositoryClass
{
    public class ResourceRepository : IResource
    {
        private readonly Context _context;

        public ResourceRepository(Context context)
        {
            _context = context;
        }

        public async void AddReferenceToTheEventAsync(MysteriousEventReferenceDto mysteriousEventReference)
        {
            string query = "Insert Into MysteriousEventReference (EventID,ReferenceID) values (@eventId,@referenceId)";
            var parameters = new DynamicParameters();
            parameters.Add("@eventId", mysteriousEventReference.EventID);
            parameters.Add("@referenceId", mysteriousEventReference.ReferenceID);
            using (var connections = _context.CreateConnection())
            {
                await connections.ExecuteAsync(query, parameters);
            }
        }

        public async void CreateAsync(ReferencesDto entity)
        {
            string query = "Insert Into Reference (ReferenceTitle,ReferenceUrl,ReferenceDescription) values (@title,@url,@description)";
            var parameters = new DynamicParameters();
            parameters.Add("@title", entity.ReferenceTitle);
            parameters.Add("@url", entity.ReferenceUrl);
            parameters.Add("@description", entity.ReferenceDescription);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<List<ReferencesDto>> GetAllAsync()
        {
            string query = "Select * from Reference";
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<ReferencesDto>(query);
                return values.ToList();
            }
        }

        public async Task<List<ResourcesDto>> GetAllWithEventAndReferenceAsync()
        {
            string query = "Select ReferenceTitle,EventTitle,ReferenceUrl,ReferenceDescription from MysteriousEventReference Inner Join Reference on MysteriousEventReference.ReferenceID = Reference.ReferenceID Inner Join MysteriousEvent on MysteriousEventReference.EventID = MysteriousEvent.EventID";
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<ResourcesDto>(query);
                return values.ToList();
            }
        }

        public async Task<ReferencesDto> GetItemAsync(int id)
        {
            string query = "Select * from Reference where ReferenceID=@referenceId";
            var parameters = new DynamicParameters();
            parameters.Add("@referenceId", id);
            using (var connection = _context.CreateConnection())
            {
                var value = await connection.QueryFirstOrDefaultAsync<ReferencesDto>(query, parameters);
                return value;
            }
        }

        public async Task<List<ResourcesDto>> GetResourcesWithEventAndReferenceByEventIdAsync(int id)
        {
            string query = "Select ReferenceTitle,EventTitle,ReferenceUrl,ReferenceDescription from MysteriousEventReference Inner Join Reference on MysteriousEventReference.ReferenceID = Reference.ReferenceID Inner Join MysteriousEvent on MysteriousEventReference.EventID = MysteriousEvent.EventID where MysteriousEventReference.EventID=@id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<ResourcesDto>(query, parameters);
                return values.ToList();
            }
        }

        public async void UpdateAsync(ReferencesDto entity)
        {
            string query = "Update Reference Set ReferenceTitle=@title,ReferenceUrl=@url,ReferenceDescription=@description where ReferenceID=@referenceId";
            var parameters = new DynamicParameters();
            parameters.Add("@title", entity.ReferenceTitle);
            parameters.Add("@url", entity.ReferenceUrl);
            parameters.Add("@description", entity.ReferenceDescription);
            parameters.Add("@referenceId", entity.ReferenceID);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}
