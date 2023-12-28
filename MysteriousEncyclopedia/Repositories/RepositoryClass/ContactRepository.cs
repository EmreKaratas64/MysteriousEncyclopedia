using Dapper;
using MysteriousEncyclopedia.Models.DapperContext;
using MysteriousEncyclopedia.Models.DTOs.ContactDto;
using MysteriousEncyclopedia.Repositories.RepositoryInterface;

namespace MysteriousEncyclopedia.Repositories.RepositoryClass
{
    public class ContactRepository : IContact
    {
        private readonly Context _context;

        public ContactRepository(Context context)
        {
            _context = context;
        }

        public async void CreateAsync(ContactsDto entity)
        {
            string query = "Insert Into Contact (ContactTitle,ContactNameSurname,ContactEmail,ContactText,ContactDate) values (@title,@name,@email,@text,@date)";
            var parameters = new DynamicParameters();
            parameters.Add("@title", entity.ContactTitle);
            parameters.Add("@name", entity.ContactNameSurname);
            parameters.Add("@email", entity.ContactEmail);
            parameters.Add("@text", entity.ContactText);
            parameters.Add("@date", entity.ContactDate);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async void DeleteContactAsync(int id)
        {
            string query = "Delete from Contact where ContactID=@contactId";
            var parameters = new DynamicParameters();
            parameters.Add("@contactId", id);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<List<ContactsDto>> GetAllAsync()
        {
            string query = "Select * from Contact order by ContactID desc";
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<ContactsDto>(query);
                return values.ToList();
            }
        }

        public async Task<ContactsDto> GetItemAsync(int id)
        {
            string query = "Select * from Contact where ContactID=@contactId";
            var parameters = new DynamicParameters();
            parameters.Add("@contactId", id);
            using (var connection = _context.CreateConnection())
            {
                var value = await connection.QueryFirstOrDefaultAsync<ContactsDto>(query, parameters);
                return value;
            }
        }

        public void UpdateAsync(ContactsDto entity)
        {
            //string query = "Update Contact set ContactTitle=@title,ContactName=@name,ContactEmail=@email,ContactText=@text,ContactDate=@date where ContactID=@contactId";
            throw new NotImplementedException();
        }
    }
}
