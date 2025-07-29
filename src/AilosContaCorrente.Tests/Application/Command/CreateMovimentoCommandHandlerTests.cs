using AilosContaCorrente.Application.Members.Commands;
using AilosContaCorrente.Domain.Abstractions;
using AilosContaCorrente.Domain.Entities;
using AilosContaCorrente.Domain.Validation;
using Moq;

namespace AilosContaCorrente.Tests.Application.Command
{
    public class CreateMovimentoCommandHandlerTests
    {
        private readonly Mock<IMovimentoRepository> _movimentoRepositoryMock;
        private readonly Mock<IContaCorrenteRepository> _contaCorrenteRepositoryMock;
        private readonly CreateMovimentoCommandHandler _handler;

        public CreateMovimentoCommandHandlerTests()
        {
            _movimentoRepositoryMock = new Mock<IMovimentoRepository>();
            _contaCorrenteRepositoryMock = new Mock<IContaCorrenteRepository>();
            _handler = new CreateMovimentoCommandHandler(_movimentoRepositoryMock.Object, _contaCorrenteRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenContaCorrenteNotFound()
        {
            // Arrange
            var command = new CreateMovimentoCommand
            {
                NumeroContaCorrenteUsuarioLogado = 1,
                TipoMovimento = "C",
                Valor = 100
            };

            _contaCorrenteRepositoryMock
                .Setup(r => r.GetFirst(It.IsAny<System.Linq.Expressions.Expression<Func<ContaCorrente, bool>>>()))
                .ReturnsAsync((ContaCorrente)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors, e => e.Message == "Conta corrente não cadastrada.");
            _movimentoRepositoryMock.Verify(r => r.Insert(It.IsAny<Movimento>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenContaCorrenteIsInactive()
        {
            // Arrange
            var conta = new ContaCorrente(1, "Titular", "senha", "95122746044") { Ativo = false };
            var command = new CreateMovimentoCommand
            {
                NumeroContaCorrenteUsuarioLogado = 1,
                TipoMovimento = "C",
                Valor = 100
            };

            _contaCorrenteRepositoryMock
                .Setup(r => r.GetFirst(It.IsAny<System.Linq.Expressions.Expression<Func<ContaCorrente, bool>>>()))
                .ReturnsAsync(conta);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors, e => e.Message == "A conta corrente informada está inativa.");
            _movimentoRepositoryMock.Verify(r => r.Insert(It.IsAny<Movimento>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenTipoMovimentoIsDebitoAndContaIsNotUsuarioLogado()
        {
            // Arrange
            var conta = new ContaCorrente(2, "Outro Titular", "senha", "95122746044") { Ativo = true };
            var command = new CreateMovimentoCommand
            {
                NumeroContaCorrenteUsuarioLogado = 1,
                NumeroContaCorrenteMovimento = 2,
                TipoMovimento = "D",
                Valor = 100
            };

            _contaCorrenteRepositoryMock
                .Setup(r => r.GetFirst(It.IsAny<System.Linq.Expressions.Expression<Func<ContaCorrente, bool>>>()))
                .ReturnsAsync(conta);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors, e => e.Message == "Para contas com outra titularidade, apenas crédito é permitido.");
            _movimentoRepositoryMock.Verify(r => r.Insert(It.IsAny<Movimento>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldInsertMovimento_WhenValid()
        {
            // Arrange
            var conta = new ContaCorrente(1, "Titular", "senha", "95122746044") { Ativo = true };
            conta.Id = 1;
            var command = new CreateMovimentoCommand
            {
                NumeroContaCorrenteUsuarioLogado = 1,
                TipoMovimento = "C",
                Valor = 100
            };

            _contaCorrenteRepositoryMock
                .Setup(r => r.GetFirst(It.IsAny<System.Linq.Expressions.Expression<Func<ContaCorrente, bool>>>()))
                .ReturnsAsync(conta);

            _movimentoRepositoryMock
                .Setup(r => r.Insert(It.IsAny<Movimento>()))
                .ReturnsAsync(new Movimento("C", 100, conta.Id));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _movimentoRepositoryMock.Verify(r => r.Insert(It.IsAny<Movimento>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnDomainValidationError_WhenDomainValidationExceptionIsThrown()
        {
            // Arrange
            var conta = new ContaCorrente(1, "Titular", "senha", "95122746044") { Ativo = true };
            var command = new CreateMovimentoCommand
            {
                NumeroContaCorrenteUsuarioLogado = 1,
                TipoMovimento = "C",
                Valor = 100
            };

            _contaCorrenteRepositoryMock
                .Setup(r => r.GetFirst(It.IsAny<System.Linq.Expressions.Expression<Func<ContaCorrente, bool>>>()))
                .ReturnsAsync(conta);

            _movimentoRepositoryMock
                .Setup(r => r.Insert(It.IsAny<Movimento>()))
                .ThrowsAsync(new DomainValidation("Conta corrente  não informada."));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors, e => e.Message == "Conta corrente  não informada.");
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenGenericExceptionIsThrown()
        {
            // Arrange
            var conta = new ContaCorrente(1, "Titular", "senha", "95122746044") { Ativo = true };
            conta.Id = 1;
            var command = new CreateMovimentoCommand
            {
                NumeroContaCorrenteUsuarioLogado = 1,
                TipoMovimento = "C",
                Valor = 100
            };

            _contaCorrenteRepositoryMock
                .Setup(r => r.GetFirst(It.IsAny<System.Linq.Expressions.Expression<Func<ContaCorrente, bool>>>()))
                .ReturnsAsync(conta);

            _movimentoRepositoryMock
                .Setup(r => r.Insert(It.IsAny<Movimento>()))
                .ThrowsAsync(new Exception("Erro inesperado"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors, e => e.Message == "Erro inesperado");
        }
    }
}