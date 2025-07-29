using AilosContaCorrente.Domain.Validation;

namespace AilosContaCorrente.Domain.Entities
{
    public class Movimento : EntityBase
    {
        public DateTime DataMovimento { get; set; } = DateTime.UtcNow;
        public string TipoMovimento { get; set; } // C ou D
        public decimal Valor { get; set; }
        public virtual int ContaCorrenteId { get; set; }
        public virtual ContaCorrente ContaCorrente { get; set; }

        public Movimento()
        {
                
        }
        public Movimento(string tipoMovimento, decimal valor, int contaCorrenteId)
        {
            ValidarDominio(tipoMovimento, valor, contaCorrenteId);
        }

        public void ValidarDominio(string tipoMovimento, decimal valor, int contaCorrenteId)
        {
            DomainValidation.When(string.IsNullOrWhiteSpace(tipoMovimento), "Tipo Movimento não informado.");
            DomainValidation.When(!(tipoMovimento == "D" || tipoMovimento == "C"), "Tipo Movimento inválido.");
            DomainValidation.When(valor <= 0, "Apenas Valores positivos são válidos.");
            DomainValidation.When(contaCorrenteId <= 0, "Conta corrente  não informada.");

            TipoMovimento = tipoMovimento.ToUpper();
            Valor = valor;
            ContaCorrenteId = contaCorrenteId;
        }
    }
}
