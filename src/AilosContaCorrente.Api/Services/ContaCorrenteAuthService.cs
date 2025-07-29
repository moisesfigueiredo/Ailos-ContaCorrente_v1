using AilosContaCorrente.Application.Dtos;
using AilosContaCorrente.Domain.Abstractions;
using AilosContaCorrente.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AilosContaCorrente.Api.Services
{
    public class ContaCorrenteAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IContaCorrenteRepository _contaCorrenteRepository;

        public ContaCorrenteAuthService(IConfiguration configuration, IContaCorrenteRepository contaCorrenteRepository)
        {
            _configuration = configuration;
            _contaCorrenteRepository = contaCorrenteRepository;
        }

        public async Task<ServiceResult> Autenticar(UserLoginDto userLoginDto)
        {
            ServiceResult<TokenJwtDto> result = new();

            try
            {
                var conta = await _contaCorrenteRepository.GetFirst(c => c.Numero == userLoginDto.NumeroConta);

                if (conta == null)
                {
                    result.AddError("Conta corrente não cadastrada.");
                    return result;
                }

                if (!conta.Ativo)
                {
                    result.AddError("A conta corrente informada está inativa.");
                    return result;
                }

                if (!await ValidarSenhaAsync(userLoginDto.Senha, conta.Senha))
                {
                    var errosDetail = new ErrorResultDetail(ErrorResultDetail.UNAUTHORIZED.Code, "Senha inválida.") { StatusCode = StatusCodes.Status401Unauthorized };

                    result.AddError(errosDetail);
                    return result;
                }

                string token = GenerateJwtToken(conta);

                result.Data = new TokenJwtDto { Token = token };
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }

            return result;
        }

        public string GenerateJwtToken(ContaCorrente conta)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings").Get<JwtSettings>();

            if (string.IsNullOrEmpty(jwtSettings?.SecretKey))
            {
                throw new InvalidOperationException("A SecretKey do JWT não está configurada.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, conta.Nome),
                new Claim("Cpf", conta.Cpf),
                new Claim("ContaCorrente", conta.Numero.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> ValidarSenhaAsync(string senhaFornecida, string senhaHashArmazenada)
        {
            return await Task.FromResult(BCrypt.Net.BCrypt.Verify(senhaFornecida, senhaHashArmazenada));
        }

        public int ExtrairNumeroContaCorrente(string authorizationHeader)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return 0;
            }

            var token = authorizationHeader.Split(' ').LastOrDefault();

            if (string.IsNullOrEmpty(token))
            {
                return 0;
            }

            var handler = new JwtSecurityTokenHandler();

            var jwtToken = handler.ReadJwtToken(token);

            var numeroContaClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "ContaCorrente");

            if (numeroContaClaim == null || string.IsNullOrEmpty(numeroContaClaim.Value))
            {
                return 0;
            }

            if (int.TryParse(numeroContaClaim.Value, out int numeroConta))
            {
                return numeroConta;
            }
            return 0;
        }
    }
}