using AilosContaCorrente.Application.Dtos;
using AilosContaCorrente.Domain.Abstractions;
using AilosContaCorrente.Domain.Entities;
using AilosContaCorrente.Domain.Validation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace AilosContaCorrente.Application.Members.Commands
{
    public class CreateContaCorrenteCommandHandler : IRequestHandler<CreateContaCorrenteCommand, ServiceResult>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;

        public CreateContaCorrenteCommandHandler(IContaCorrenteRepository contaCorrenteRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
        }

        public async Task<ServiceResult> Handle(CreateContaCorrenteCommand request, CancellationToken cancellationToken)
        {
            ServiceResult result = new();

            try
            {
                var conta = await _contaCorrenteRepository.GetFirst(c => c.Numero == request.Numero);

                if (conta != null)
                {
                    result.AddError("Conta já cadastrada.");
                    return result;
                }

                var novaConta = new ContaCorrente(request.Numero, request.Nome, BCrypt.Net.BCrypt.HashPassword(request.Senha), request.Cpf);

                await _contaCorrenteRepository.Insert(novaConta);
            }
            catch(DomainValidation ex)
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
