using AilosContaCorrente.Application.Dtos;
using AilosContaCorrente.Domain.Abstractions;
using MediatR;

namespace AilosContaCorrente.Application.Members.Queries
{
    public class GetContaCorrenteCadastradaAtivaQueryHandler : IRequestHandler<GetContaCorrenteCadastradaAtivaQuery, ServiceResult>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;

        public GetContaCorrenteCadastradaAtivaQueryHandler(IContaCorrenteRepository contaCorrenteRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
        }

        public async Task<ServiceResult> Handle(GetContaCorrenteCadastradaAtivaQuery request, CancellationToken cancellationToken)
        {
            ServiceResult<bool> result = new();

            try
            {
                var conta = await _contaCorrenteRepository.GetFirst(c => c.Numero == request.Numero);

                if (conta == null)
                {
                    result.Data = false;
                    result.AddError("Conta corrente não cadastrada.");
                    return result;
                }

                if (!conta.Ativo)
                {
                    result.Data = false;
                    result.AddError("A conta corrente informada está inativa.");
                    return result;
                }

                result.Data = true;
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }

            return result;
        }
    }
}
