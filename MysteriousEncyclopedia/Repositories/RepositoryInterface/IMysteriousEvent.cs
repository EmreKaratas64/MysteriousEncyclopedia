using MysteriousEncyclopedia.Models.DTOs.EventDto;

namespace MysteriousEncyclopedia.Repositories.RepositoryInterface
{
    public interface IMysteriousEvent : IGeneric<MysteriousEventDto>
    {
        Task<List<MysteriousEventDto>> GetVisibleEventsAsync();

        Task<MysteriousEventDto> GetVisibleItemAsync(int id);

        Task<List<MysteriousEventDto>> GetVisibleLast6EventsAsync();

        Task<List<MysteriousEventDto>> GetVisibleEventsByTopicAsync(string topic);

        Task<List<MysteriousEventDto>> GetVisibleEventsByName(string eventName);
    }
}
