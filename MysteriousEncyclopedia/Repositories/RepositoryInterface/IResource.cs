using MysteriousEncyclopedia.Models.DTOs.ReferenceDto;
using MysteriousEncyclopedia.Models.DTOs.ResourceDto;

namespace MysteriousEncyclopedia.Repositories.RepositoryInterface
{
    public interface IResource : IGeneric<ReferencesDto>
    {
        Task<List<ResourcesDto>> GetAllWithEventAndReferenceAsync();
    }
}
