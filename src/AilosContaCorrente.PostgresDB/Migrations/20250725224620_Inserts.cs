using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AilosContaCorrente.PostgresDB.Migrations
{
    /// <inheritdoc />
    public partial class Inserts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO ""ContaCorrente"" (""Numero"", ""Nome"", ""Ativo"", ""Senha"", ""Cpf"") 
                    VALUES 
                (9999, 'Usuário Teste', true, '$2a$11$ECmUClS0aQcpL6m5TGF7N.pmqvMX1hlwp.RYy6yjZR4UeGza/9ZOm', '311.953.510-97');

                "
             );
        }
    }
}
