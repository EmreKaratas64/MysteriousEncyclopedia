using Dapper;
using MysteriousEncyclopedia.Models.DapperContext;
using MysteriousEncyclopedia.Models.DTOs.Role;
using MysteriousEncyclopedia.Models.DTOs.User;
using MysteriousEncyclopedia.Repositories.RepositoryInterface;

namespace MysteriousEncyclopedia.Repositories.RepositoryClass
{
    public class UserRepository : IUser
    {
        private readonly Context _context;

        public UserRepository(Context context)
        {
            _context = context;
        }

        public void CreateAsync(UserDto entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            string query = "Select Id,UserName,Email,EmailConfirmed,AccessFailedCount from AspNetUsers";
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<UserDto>(query);
                return values.ToList();
            }
        }

        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            string query = "Select * from AspNetRoles";
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<RoleDto>(query);
                return values.ToList();
            }
        }

        public Task<UserDto> GetItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async void RemoveUserFromRoleAsync(string userId, string roleId)
        {
            string query = "Delete from AspNetUserRoles where UserId=@userid and RoleId=@roleid";
            var parameters = new DynamicParameters();
            parameters.Add("@userid", userId);
            parameters.Add("@roleid", roleId);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public void UpdateAsync(UserDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
