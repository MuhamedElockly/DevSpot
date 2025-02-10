namespace DevSpot.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task AddAysnc(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);

    }
}
