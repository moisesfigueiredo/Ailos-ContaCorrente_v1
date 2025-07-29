using AilosContaCorrente.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AilosContaCorrente.PostgresDB.Core
{
    public interface IEntityMap<TEntity> : IEntityTypeConfiguration<TEntity>
       where TEntity : EntityBase
    {
    }
}
