using AilosContaCorrente.Domain.Entities;
using AilosContaCorrente.PostgresDB.Core;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AilosContaCorrente.PostgresDB.Mapping
{
    public class ContaCorrenteMap : IEntityMap<ContaCorrente>
    {
        public void Configure(EntityTypeBuilder<ContaCorrente> builder)
        {
            builder.Property(x => x.Numero)
                   .IsRequired();
            builder.Property(x => x.Cpf)
                  .IsRequired();
            builder.Property(x => x.Ativo)
                  .IsRequired();
            builder.Property(x => x.Nome)
                  .IsRequired();
            builder.HasMany(x => x.Movimentos);
        }
    }
}
