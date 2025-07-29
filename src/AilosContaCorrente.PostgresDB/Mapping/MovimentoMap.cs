using AilosContaCorrente.Domain.Entities;
using AilosContaCorrente.PostgresDB.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AilosContaCorrente.PostgresDB.Mapping
{
    public class MovimentoMap : IEntityMap<Movimento>
    {
        public void Configure(EntityTypeBuilder<Movimento> builder)
        {
            builder.Property(x => x.ContaCorrenteId)
             .HasColumnName("ContaCorrente")
             .IsRequired();
            builder.HasOne(x => x.ContaCorrente)
             .WithMany(x => x.Movimentos);
            builder.Property(x => x.TipoMovimento)
             .IsRequired();
            builder.Property(x => x.Valor)
             .IsRequired();
        }
    }
}
