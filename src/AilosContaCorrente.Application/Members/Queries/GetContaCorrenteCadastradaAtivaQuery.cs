using AilosContaCorrente.Application.Dtos;
using MediatR;

namespace AilosContaCorrente.Application.Members.Queries
{
    public class GetContaCorrenteCadastradaAtivaQuery : IRequest<ServiceResult>
    {
        public int Numero { get; set; }
    }
}
