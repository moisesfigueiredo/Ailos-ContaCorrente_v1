﻿using AilosContaCorrente.Domain.Validation;

namespace AilosContaCorrente.Domain.Entities
{
    public class ContaCorrente : EntityBase
    {
        public int Numero { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; } = true;
        public string Senha { get; set; }
        public string Cpf { get; set; }
        public virtual List<Movimento>? Movimentos { get; set; }

        public ContaCorrente()
        {
                
        }

        public ContaCorrente(int numero, string nome, string senha, string cpf, List<Movimento>? movimentos = null)
        {
            ValidarDominio(numero, nome, senha, cpf, movimentos);
        }

        public void ValidarDominio(int numero, string nome, string senha, string cpf, List<Movimento>? movimentos)
        {
            DomainValidation.When(string.IsNullOrWhiteSpace(nome), "Nome não informado.");
            DomainValidation.When(string.IsNullOrWhiteSpace(cpf), "CPF não informado.");
            DomainValidation.When(nome.Length < 3, "Nome deve ter pelo menos 3 caracteres.");
            DomainValidation.When(!ValidaCpf(cpf), "CPF inválido.");

            Numero = numero;
            Nome = nome;
            Senha = senha;
            Cpf = cpf;
            Movimentos = movimentos;
        }

        public static bool ValidaCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            if (cpf.Equals("00000000000") ||
              cpf.Equals("11111111111") ||
              cpf.Equals("22222222222") ||
              cpf.Equals("33333333333") ||
              cpf.Equals("44444444444") ||
              cpf.Equals("55555555555") ||
              cpf.Equals("66666666666") ||
              cpf.Equals("77777777777") ||
              cpf.Equals("88888888888") ||
              cpf.Equals("99999999999"))
            {
                return false;
            }

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }
    }
}
