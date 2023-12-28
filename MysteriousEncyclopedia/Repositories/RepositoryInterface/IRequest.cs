using MysteriousEncyclopedia.Models.DTOs.Request;

namespace MysteriousEncyclopedia.Repositories.RepositoryInterface
{
    public interface IRequest : IGeneric<RequestDto>
    {
        void DeleteRequestAsync(int Id);
    }
}
