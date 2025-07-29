using AilosContaCorrente.Domain.Abstractions;
using AilosContaCorrente.Domain.Entities;
using AilosContaCorrente.PostgresDB.Core;

namespace AilosContaCorrente.PostgresDB.Repositories
{
    public class ContaCorrenteRepository : Repository<ContaCorrente>, IContaCorrenteRepository
    {
        public ContaCorrenteRepository(ApplicationDbContext context) :
            base(context)
        {
        }
    }
}
