using AilosContaCorrente.Application.Dtos;
using AilosContaCorrente.Domain.Abstractions;
using MediatR;

namespace AilosContaCorrente.Application.Members.Queries
{
    public class GetSaldoContaCorrenteQueryHandler : IRequestHandler<GetSaldoContaCorrenteQuery, ServiceResult>
    {
        private readonly IMovimentoRepository _movimentoRepository;
        private readonly IContaCorrenteRepository _contaCorrenteRepository;

        public GetSaldoContaCorrenteQueryHandler(IMovimentoRepository movimentoRepository, IContaCorrenteRepository contaCorrenteRepository)
        {
            _movimentoRepository = movimentoRepository;
            _contaCorrenteRepository = contaCorrenteRepository;
        }

        public async Task<ServiceResult> Handle(GetSaldoContaCorrenteQuery request, CancellationToken cancellationToken)
        {
            ServiceResult<SaldoDto> result = new();

            try
            {
                var conta = await _contaCorrenteRepository.GetFirst(c => c.Numero == request.Numero);

                if (conta == null)
                {
                    result.AddError("Conta corrente não cadastrada.");
                    return result;
                }

                if (!conta.Ativo)
                {
                    result.AddError("A conta corrente informada está inativa.");
                    return result;
                }

                var movimentos = await _movimentoRepository.GetAll(m => m.ContaCorrenteId == conta.Id);

                result.Data = new SaldoDto
                {
                    Numero = conta.Numero,
                    Nome = conta.Nome,
                    SaldoAtual = movimentos.Any() ? (movimentos.Where(v => v.TipoMovimento == "C").Sum(v => v.Valor) - movimentos.Where(v => v.TipoMovimento == "D").Sum(v => v.Valor)) : 0
                };
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }

            return result;
        }
    }
}
