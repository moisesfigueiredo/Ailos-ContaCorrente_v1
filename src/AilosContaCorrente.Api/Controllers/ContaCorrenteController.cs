using AilosContaCorrente.Api.Services;
using AilosContaCorrente.Application.Dtos;
using AilosContaCorrente.Application.Members.Commands;
using AilosContaCorrente.Application.Members.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AilosContaCorrente.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ContaCorrenteAuthService _authService;

        public ContaCorrenteController(IMediator mediator, ContaCorrenteAuthService authService)
        {
            _mediator = mediator;
            _authService = authService;
        }

        [HttpPost("CadastrarConta")]
        public async Task<IResult> CriaConta(int numero, string nome, string senha, string cpf)
        {
            var comando = new CreateContaCorrenteCommand()
            {
                Numero = numero,
                Nome = nome,
                Senha = senha,
                Cpf = cpf
            };

            return Results.Ok(await _mediator.Send(comando));
        }

        [HttpPost("Logar")]
        public async Task<IResult> Logar(UserLoginDto userLoginDto)
        {
            //return Results.Ok(await Autenticar(userLoginDto));
            return Results.Ok(await _authService.Autenticar(userLoginDto));

        }

        [HttpPost("InativarConta")]
        [Authorize]
        public async Task<IResult> Inativar(UpdateInativarContaCorrenteCommand comando)
        {
            return Results.Ok(await _mediator.Send(comando));
        }

        [HttpPost("MovimentacaoContaCorrente")]
        [Authorize]
        public async Task<IResult> MovimentacaoContaCorrente(int? numContaCorrenteMovimento, decimal valor, string tipoMovimento)
        {
            var comando = new CreateMovimentoCommand
            {
                NumeroContaCorrenteMovimento = numContaCorrenteMovimento,
                Valor = valor,
                TipoMovimento = tipoMovimento,
                NumeroContaCorrenteUsuarioLogado = _authService.ExtrairNumeroContaCorrente(Request.Headers["Authorization"].FirstOrDefault())
            };

            return Results.Ok(await _mediator.Send(comando));
        }

        [HttpGet("SaldoContaCorrente")]
        [Authorize]
        public async Task<IResult> SaldoContaCorrente(int numero)
        {
            var query = new GetSaldoContaCorrenteQuery
            {
                Numero = numero
            };

            return Results.Ok(await _mediator.Send(query));
        }

        [HttpGet("GetContaCorrenteCadastradaAtiva")]
        [Authorize]
        public async Task<IResult> GetContaCorrenteCadastradaAtiva(int numero)
        {
            var query = new GetContaCorrenteCadastradaAtivaQuery
            {
                Numero = numero
            };

            return Results.Ok(await _mediator.Send(query));
        }
    }
}
