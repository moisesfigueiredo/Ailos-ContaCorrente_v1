using AilosContaCorrente.Domain.Entities;
using AilosContaCorrente.Domain.Validation;

namespace AilosContaCorrente.Tests.Domain
{
    public class MovimentoTests
    {
        [Fact]
        public void Deve_Criar_Movimento_Valido()
        {
            // Arrange
            var tipoMovimento = "C";
            var valor = 100.0m;
            var contaCorrenteId = 1;

            // Act
            var movimento = new Movimento(tipoMovimento, valor, contaCorrenteId);

            // Assert
            Assert.Equal(tipoMovimento, movimento.TipoMovimento);
            Assert.Equal(valor, movimento.Valor);
            Assert.Equal(contaCorrenteId, movimento.ContaCorrenteId);
            Assert.True(movimento.DataMovimento <= DateTime.UtcNow);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Deve_Lancar_Excecao_Quando_TipoMovimento_Nao_Informado(string tipoMovimentoInvalido)
        {
            // Arrange
            var valor = 100.0m;
            var contaCorrenteId = 1;

            // Act & Assert
            var ex = Assert.Throws<DomainValidation>(() =>
                new Movimento(tipoMovimentoInvalido, valor, contaCorrenteId)
            );
            Assert.Equal("Tipo Movimento não informado.", ex.Message);
        }

        [Theory]
        [InlineData("X")]
        [InlineData("A")]
        public void Deve_Lancar_Excecao_Quando_TipoMovimento_Invalido(string tipoMovimentoInvalido)
        {
            // Arrange
            var valor = 100.0m;
            var contaCorrenteId = 1;

            // Act & Assert
            var ex = Assert.Throws<DomainValidation>(() =>
                new Movimento(tipoMovimentoInvalido, valor, contaCorrenteId)
            );
            Assert.Equal("Tipo Movimento inválido.", ex.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void Deve_Lancar_Excecao_Quando_Valor_Menor_Ou_Igual_A_Zero(decimal valorInvalido)
        {
            // Arrange
            var tipoMovimento = "C";
            var contaCorrenteId = 1;

            // Act & Assert
            var ex = Assert.Throws<DomainValidation>(() =>
                new Movimento(tipoMovimento, valorInvalido, contaCorrenteId)
            );
            Assert.Equal("Apenas Valores positivos são válidos.", ex.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Deve_Lancar_Excecao_Quando_ContaCorrenteId_Menor_Ou_Igual_A_Zero(int contaCorrenteIdInvalido)
        {
            // Arrange
            var tipoMovimento = "C";
            var valor = 100.0m;

            // Act & Assert
            var ex = Assert.Throws<DomainValidation>(() =>
                new Movimento(tipoMovimento, valor, contaCorrenteIdInvalido)
            );
            Assert.Equal("Conta corrente  não informada.", ex.Message);
        }
    }
}