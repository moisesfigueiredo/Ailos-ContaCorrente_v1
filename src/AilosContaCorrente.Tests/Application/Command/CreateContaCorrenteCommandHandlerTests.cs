using AilosContaCorrente.Application.Members.Commands;
using AilosContaCorrente.Domain.Abstractions;
using AilosContaCorrente.Domain.Entities;
using AilosContaCorrente.Domain.Validation;
using Moq;

namespace AilosContaCorrente.Tests.Application.Command
{
    public class CreateContaCorrenteCommandHandlerTests
    {
        private readonly Mock<IContaCorrenteRepository> _repositoryMock;
        private readonly CreateContaCorrenteCommandHandler _handler;

        public CreateContaCorrenteCommandHandlerTests()
        {
            _repositoryMock = new Mock<IContaCorrenteRepository>();
            _handler = new CreateContaCorrenteCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenContaAlreadyExists()
        {
            // Arrange
            var command = new CreateContaCorrenteCommand
            {
                Numero = 123,
                Nome = "Test",
                Senha = "senha",
                Cpf = "95122746044"
            };

            _repositoryMock
                .Setup(r => r.GetFirst(It.IsAny<System.Linq.Expressions.Expression<Func<ContaCorrente, bool>>>()))
                .ReturnsAsync(new ContaCorrente());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors, e => e.Message == "Conta já cadastrada.");
            _repositoryMock.Verify(r => r.Insert(It.IsAny<ContaCorrente>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldInsertConta_WhenContaDoesNotExist()
        {
            // Arrange
            var command = new CreateContaCorrenteCommand
            {
                Numero = 123,
                Nome = "Test",
                Senha = "senha",
                Cpf = "95122746044"
            };

            _repositoryMock
                .Setup(r => r.GetFirst(It.IsAny<System.Linq.Expressions.Expression<Func<ContaCorrente, bool>>>()))
                .ReturnsAsync((ContaCorrente)null);

            _repositoryMock
                .Setup(r => r.Insert(It.IsAny<ContaCorrente>()))
                .ReturnsAsync(new ContaCorrente());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _repositoryMock.Verify(r => r.Insert(It.IsAny<ContaCorrente>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnDomainValidationError_WhenDomainValidationExceptionIsThrown()
        {
            // Arrange
            var command = new CreateContaCorrenteCommand
            {
                Numero = 123,
                Nome = "Test",
                Senha = "senha",
                Cpf = "95122746044"
            };

            _repositoryMock
                .Setup(r => r.GetFirst(It.IsAny<System.Linq.Expressions.Expression<Func<ContaCorrente, bool>>>()))
                .ReturnsAsync((ContaCorrente)null);

            _repositoryMock
                .Setup(r => r.Insert(It.IsAny<ContaCorrente>()))
                .ThrowsAsync(new DomainValidation("Erro inesperado"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors, e => e.Message == "Erro inesperado");
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenGenericExceptionIsThrown()
        {
            // Arrange
            var command = new CreateContaCorrenteCommand
            {
                Numero = 123,
                Nome = "Test",
                Senha = "senha",
                Cpf = "95122746044"
            };

            _repositoryMock
                .Setup(r => r.GetFirst(It.IsAny<System.Linq.Expressions.Expression<Func<ContaCorrente, bool>>>()))
                .ReturnsAsync((ContaCorrente)null);

            _repositoryMock
                .Setup(r => r.Insert(It.IsAny<ContaCorrente>()))
                .ThrowsAsync(new Exception("Erro inesperado"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors, e => e.Message == "Erro inesperado");
        }
    }
}