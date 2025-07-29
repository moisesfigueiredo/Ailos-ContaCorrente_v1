using AilosContaCorrente.Domain.Entities;

namespace AilosContaCorrente.Domain.Abstractions
{
    public interface IMovimentoRepository : IBaseRepository, IRepository<Movimento>
    {
    }
}
