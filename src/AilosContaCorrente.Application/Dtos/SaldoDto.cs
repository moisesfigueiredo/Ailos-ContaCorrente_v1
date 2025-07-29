namespace AilosContaCorrente.Application.Dtos
{
    public class SaldoDto
    {
        public int Numero { get; set; }
        public string Nome { get; set; }
        public DateTime DataHoraConsulta { get; set; } = DateTime.Now;
        public decimal SaldoAtual { get; set; }
    }
}
