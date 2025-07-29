using AilosContaCorrente.Application.Dtos;
using AilosContaCorrente.Domain.Abstractions;
using AilosContaCorrente.Domain.Entities;
using AilosContaCorrente.Domain.Validation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace AilosContaCorrente.Application.Members.Commands
{
    public class CreateMovimentoCommandHandler : IRequestHandler<CreateMovimentoCommand, ServiceResult>
    {
        private readonly IMovimentoRepository _movimentoRepository;
        private readonly IContaCorrenteRepository _contaCorrenteRepository;

        public CreateMovimentoCommandHandler(IMovimentoRepository movimentoRepository, IContaCorrenteRepository contaCorrenteRepository)
        {
            _movimentoRepository = movimentoRepository;
            _contaCorrenteRepository = contaCorrenteRepository;
        }

        public async Task<ServiceResult> Handle(CreateMovimentoCommand request, CancellationToken cancellationToken)
        {
            ServiceResult result = new();

            try
            {
                ContaCorrente conta;
                Movimento movimento;

                if (request.NumeroContaCorrenteMovimento.HasValue)
                {
                    if (request.TipoMovimento == "D")
                    {
                        if (request.NumeroContaCorrenteUsuarioLogado != request.NumeroContaCorrenteMovimento.Value)
                        {
                            result.AddError("Para contas com outra titularidade, apenas crédito é permitido.");
                            return result;
                        }
                    }

                    conta = await _contaCorrenteRepository.GetFirst(c => c.Numero == request.NumeroContaCorrenteMovimento.Value);
                }
                else
                {
                    if (request.NumeroContaCorrenteUsuarioLogado <= 0)
                    {
                        result.AddError("Não foram informadas contas para efetuar movimentação");
                        return result;
                    }

                    conta = await _contaCorrenteRepository.GetFirst(c => c.Numero == request.NumeroContaCorrenteUsuarioLogado);
                }

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

                movimento = new Movimento(request.TipoMovimento, request.Valor, conta.Id);

                await _movimentoRepository.Insert(movimento);
            }
            catch (DomainValidation ex)
            {
                var errosDetail = new ErrorResultDetail(ErrorResultDetail.INVALID_INPUT.Code, ex.Message) { StatusCode = StatusCodes.Status400BadRequest };
                result.AddError(errosDetail);

            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }

            return result;
        }
    }
}
