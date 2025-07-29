using AilosContaCorrente.Application.Dtos;
using MediatR;

namespace AilosContaCorrente.Application.Members.Commands
{
    public class CreateContaCorrenteCommand : IRequest<ServiceResult>
    {
        public int Numero { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public string Cpf { get; set; }
    }
}
