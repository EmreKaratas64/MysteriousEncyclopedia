using MysteriousEncyclopedia.Models.DTOs.Role;
using MysteriousEncyclopedia.Models.DTOs.User;

namespace MysteriousEncyclopedia.Repositories.RepositoryInterface
{
    public interface IUser : IGeneric<UserDto>
    {
        Task<List<RoleDto>> GetAllRolesAsync();

        void RemoveUserFromRoleAsync(string userId, string roleId);
    }
}
