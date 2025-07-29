using AilosContaCorrente.Application.Dtos;
using MediatR;

namespace AilosContaCorrente.Application.Members.Queries
{
    public class GetSaldoContaCorrenteQuery : IRequest<ServiceResult>
    {
        public int Numero { get; set; }
    }
}
