using AilosContaCorrente.Application.Dtos;
using MediatR;

namespace AilosContaCorrente.Application.Members.Commands
{
    public class UpdateInativarContaCorrenteCommand : IRequest<ServiceResult>
    {
        public int Numero { get; set; }
    }
}
