using Dapper;
using MysteriousEncyclopedia.Models.DapperContext;
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

        public async void CreateAsync(ResourcesDto entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ResourcesDto>> GetAllAsync()
        {
            string query = "Select ReferenceTitle,EventTitle,ReferenceUrl,ReferenceDescription from MysteriousEventReference Inner Join Reference on MysteriousEventReference.ReferenceID = Reference.ReferenceID Inner Join MysteriousEvent on MysteriousEventReference.EventID = MysteriousEvent.EventID";
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<ResourcesDto>(query);
                return values.ToList();
            }
        }

        public async Task<ResourcesDto> GetItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async void UpdateAsync(ResourcesDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
