namespace MysteriousEncyclopedia.Repositories.RepositoryInterface
{
    public interface IGeneric<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        void CreateAsync(T entity);
        void UpdateAsync(T entity);
        Task<T> GetItemAsync(int id);
    }
}
