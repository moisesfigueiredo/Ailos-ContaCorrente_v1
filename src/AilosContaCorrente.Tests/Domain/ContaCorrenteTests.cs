using AilosContaCorrente.Domain.Entities;
using AilosContaCorrente.Domain.Validation;

namespace AilosContaCorrente.Tests.Domain
{
    public class ContaCorrenteTests
    {
        [Fact]
        public void Deve_Criar_ContaCorrente_Valida()
        {
            // Arrange
            var numero = 1;
            var nome = "João Silva";
            var senha = "senha123";
            var cpf = "95122746044"; // CPF válido para teste

            // Act
            var conta = new ContaCorrente(numero, nome, senha, cpf);

            // Assert
            Assert.Equal(numero, conta.Numero);
            Assert.Equal(nome, conta.Nome);
            Assert.Equal(senha, conta.Senha);
            Assert.Equal(cpf, conta.Cpf);
            Assert.True(conta.Ativo);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Deve_Lancar_Excecao_Quando_Nome_Nao_Informado(string nomeInvalido)
        {
            // Arrange
            var numero = 1;
            var senha = "senha123";
            var cpf = "95122746044";

            // Act & Assert
            var ex = Assert.Throws<DomainValidation>(() =>
                new ContaCorrente(numero, nomeInvalido, senha, cpf)
            );
            Assert.Equal("Nome não informado.", ex.Message);
        }

        [Fact]
        public void Deve_Lancar_Excecao_Quando_Nome_Menor_Que_3_Caracteres()
        {
            // Arrange
            var numero = 1;
            var nome = "Jo";
            var senha = "senha123";
            var cpf = "95122746044";

            // Act & Assert
            var ex = Assert.Throws<DomainValidation>(() =>
                new ContaCorrente(numero, nome, senha, cpf)
            );
            Assert.Equal("Nome deve ter pelo menos 3 caracteres.", ex.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Deve_Lancar_Excecao_Quando_Cpf_Nao_Informado(string cpfInvalido)
        {
            // Arrange
            var numero = 1;
            var nome = "João Silva";
            var senha = "senha123";

            // Act & Assert
            var ex = Assert.Throws<DomainValidation>(() =>
                new ContaCorrente(numero, nome, senha, cpfInvalido)
            );
            Assert.Equal("CPF não informado.", ex.Message);
        }

        [Fact]
        public void Deve_Lancar_Excecao_Quando_Cpf_Invalido()
        {
            // Arrange
            var numero = 1;
            var nome = "João Silva";
            var senha = "senha123";
            var cpf = "11111111111"; // CPF inválido

            // Act & Assert
            var ex = Assert.Throws<DomainValidation>(() =>
                new ContaCorrente(numero, nome, senha, cpf)
            );
            Assert.Equal("CPF inválido.", ex.Message);
        }

        [Theory]
        [InlineData("95122746044")] // válido
        [InlineData("11111111111")] // inválido
        [InlineData("00000000000")] // inválido
        [InlineData("123")]         // inválido
        public void Deve_Validar_Cpf_Corretamente(string cpf)
        {
            // Act
            var resultado = ContaCorrente.ValidaCpf(cpf);

            // Assert
            if (cpf == "95122746044")
                Assert.True(resultado);
            else
                Assert.False(resultado);
        }
    }
}