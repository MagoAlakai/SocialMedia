namespace SocialMedia.Core.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    public Task<IEnumerable<T>> GetAsync();
    public Task<T?> GetByIdAsync(int id);
    public Task<T?> PostAsync(T entity);
    public Task<T?> UpdateAsync(T entity, int id);
    public Task<bool> DeleteAsync(int id);
}
