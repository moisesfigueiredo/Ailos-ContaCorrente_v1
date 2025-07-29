using AilosContaCorrente.Application.Dtos;
using AilosContaCorrente.Domain.Abstractions;
using MediatR;

namespace AilosContaCorrente.Application.Members.Commands
{
    public class UpdateInativarContaCorrenteCommandHandler : IRequestHandler<UpdateInativarContaCorrenteCommand, ServiceResult>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;

        public UpdateInativarContaCorrenteCommandHandler(IContaCorrenteRepository contaCorrenteRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
        }

        public async Task<ServiceResult> Handle(UpdateInativarContaCorrenteCommand request, CancellationToken cancellationToken)
        {
            ServiceResult result = new();

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

                conta.Ativo = false;

                await _contaCorrenteRepository.Update(conta);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }

            return result;
        }
    }
}
