using SocialMedia.Core.Data;
using SocialMedia.Core.DTOs.Identity;

namespace SocialMedia.Infrastructure.Repositories;

/// <summary>
/// Abstract implementation of the interface IRepository<typeparamref name="T"/><
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly ApplicationDbContext _applicationDbContext;
    private DbSet<T> _entities;

    public BaseRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
        _entities = applicationDbContext.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAsync()
    {
        IEnumerable<T> entities = await _entities.ToListAsync();

        return entities;
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _entities.FindAsync(id);
    }

    public async Task<T?> PostAsync(T entity)
    {
        bool entity_exist = await _entities.AnyAsync(x => x.Id == entity.Id);
        if (entity_exist is true) { return null; }

        EntityEntry? entry = await _entities.AddAsync(entity);
        return (T?)entry.Entity;
    }

    public async Task<T?> UpdateAsync(T entity, int id)
    {
        bool entity_exist = await _entities.AnyAsync(x => x.Id == id);
        if (entity_exist is false) { return null; }

        T entity_updated = _entities.Update(entity).Entity;
        entity_updated.Id = id;

        return await Task.FromResult(entity_updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        T? entity = await _entities.FindAsync(id);
        if (entity == null) { return false; }
        _entities.Remove(entity);

        return true;
    }
}
