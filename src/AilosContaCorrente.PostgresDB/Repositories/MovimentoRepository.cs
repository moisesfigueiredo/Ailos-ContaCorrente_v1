using AilosContaCorrente.Domain.Abstractions;
using AilosContaCorrente.Domain.Entities;
using AilosContaCorrente.PostgresDB.Core;

namespace AilosContaCorrente.PostgresDB.Repositories
{
    public class MovimentoRepository : Repository<Movimento>, IMovimentoRepository
    {
        public MovimentoRepository(ApplicationDbContext context) :
            base(context)
        {
        }
    }
}
