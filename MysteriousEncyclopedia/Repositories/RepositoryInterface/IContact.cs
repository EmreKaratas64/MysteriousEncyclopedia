using MysteriousEncyclopedia.Models.DTOs.ContactDto;

namespace MysteriousEncyclopedia.Repositories.RepositoryInterface
{
    public interface IContact : IGeneric<ContactsDto>
    {
        void DeleteContactAsync(int id);
    }
}
