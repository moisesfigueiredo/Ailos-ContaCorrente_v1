using AilosContaCorrente.Domain;
using AilosContaCorrente.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AilosContaCorrente.PostgresDB.Core
{
    public abstract class Repository<T> : IRepository<T>
   where T : EntityBase
    {
        protected readonly ApplicationDbContext _context;

        private readonly DbSet<T> dbSet = null;

        protected Repository(ApplicationDbContext context)
        {
            _context = context;

            dbSet = _context.Set<T>();
        }

        protected IQueryable<T> Queryable(bool stateless = true)
        {
            return Queryable<T>(stateless);
        }


        protected IQueryable<TEntity> Queryable<TEntity>(bool stateless = true)
             where TEntity : EntityBase
        {
            return stateless
                ? _context.Set<TEntity>().AsNoTracking()
                : _context.Set<TEntity>().AsQueryable();
        }

        public async Task<List<T>> GetAll()
        {
            var result = await Queryable()
                .ToListAsync();
            return result;
        }

        public async Task<T> GetFirst(object id)
        {
            var result = await Queryable()
                .Where(x => x.Id.Equals(id))
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<T> GetFirst(Expression<Func<T, bool>> predicate)
        {
            var result = await Queryable()
                .Where(predicate)
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>> predicate)
        {
            var result = await Queryable()
                .Where(predicate)
                .ToListAsync();
            return result;
        }

        public async Task<List<T>> Insert(List<T> entities)
        {
            var result = new List<T>();

            foreach (var entity in entities)
                result.Add(await Insert(entity));

            return result;
        }

        public async Task<T> Insert(T entity)
        {
            await dbSet.AddAsync(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<List<T>> Update(List<T> entities)
        {
            var result = new List<T>();

            foreach (var entity in entities)
                result.Add(AttachUpdate(entity));

            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<T> Update(T entity)
        {
            AttachUpdate(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        private T AttachUpdate(T entity)
        {
            var local = dbSet.Local.FirstOrDefault(p => p.Id == entity.Id);

            if (local != null)
                _context.Entry(local).State = EntityState.Detached;

            dbSet.Attach(entity);

            _context.Entry(entity).State = EntityState.Modified;

            return entity;
        }

        public async Task<List<T>> Disable(List<T> entities)
        {
            var result = new List<T>();

            foreach (var entity in entities)
                result.Add(AttachDisable(entity));

            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<T> Disable(T entity)
        {
            AttachDisable(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        private T AttachDisable(T entity)
        {
            var local = dbSet.Local.FirstOrDefault(p => p.Id == entity.Id);

            if (local != null)
                _context.Entry(local).State = EntityState.Detached;

            dbSet.Attach(entity);

            _context.Entry(entity).State = EntityState.Modified;

            return entity;
        }

        public Task Delete(object id)
        {
            T existing = dbSet.Find(id);
            dbSet.Remove(existing);

            return _context.SaveChangesAsync();
        }

        public Task DeleteRange(object[] ids)
        {
            var existing = dbSet.Where(x => ids.Contains(x.Id));
            dbSet.RemoveRange(existing);

            return _context.SaveChangesAsync();
        }
    }
}