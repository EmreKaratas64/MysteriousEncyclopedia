using MysteriousEncyclopedia.Models.DTOs.TopicDto;

namespace MysteriousEncyclopedia.Repositories.RepositoryInterface
{
    public interface ITopic : IGeneric<ResultTopicDto>
    {
        Task<List<Tuple<string, int>>> GetTopicsWithNumberOfEventsAsync();
    }
}
