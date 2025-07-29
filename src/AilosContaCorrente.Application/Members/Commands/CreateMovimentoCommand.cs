using AilosContaCorrente.Application.Dtos;
using MediatR;
using System.Text.Json.Serialization;

namespace AilosContaCorrente.Application.Members.Commands
{
    public class CreateMovimentoCommand : IRequest<ServiceResult>
    {
        [JsonIgnore]
        public int NumeroContaCorrenteUsuarioLogado { get; set; }
        public int? NumeroContaCorrenteMovimento { get; set; }
        public decimal Valor { get; set; }
        private string _tipoMovimento;

        public string TipoMovimento
        {
            get => _tipoMovimento.ToUpper();
            set
            {
                _tipoMovimento = value;
            }
        }

    }
}
