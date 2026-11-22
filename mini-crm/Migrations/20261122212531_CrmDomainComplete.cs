using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mini_crm.Migrations
{
    /// <inheritdoc />
    public partial class CrmDomainComplete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CampanhaId",
                table: "NotasFiscais",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ContaPagarId",
                table: "NotasFiscais",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ContaReceberId",
                table: "NotasFiscais",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Agencia_Nome",
                table: "EntidadesComerciais",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "EntidadesComerciais",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cnpj",
                table: "EntidadesComerciais",
                type: "varchar(14)",
                maxLength: 14,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PreferenciaIdioma",
                table: "EntidadesComerciais",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Segmento",
                table: "EntidadesComerciais",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "AssinadoEmUtc",
                table: "Contratos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CampanhaId",
                table: "Contratos",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConteudoContrato",
                table: "Contratos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "InfluencerParticipacaoId",
                table: "Contratos",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoCancelamento",
                table: "Contratos",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "StatusCrm",
                table: "Contratos",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Versao",
                table: "Contratos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Campanhas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OportunidadeId = table.Column<long>(type: "bigint", nullable: false),
                    ClienteEntidadeComercialId = table.Column<long>(type: "bigint", nullable: false),
                    ResponsavelId = table.Column<long>(type: "bigint", nullable: false),
                    Frente = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInicio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    TipoTaxa = table.Column<int>(type: "int", nullable: false),
                    Taxa = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalAprovado_CodigoMoeda = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                    TotalAprovado_Valor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campanhas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ContasPagar",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CampanhaId = table.Column<long>(type: "bigint", nullable: false),
                    ParticipacaoCampanhaId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ValorPagoAcumulado = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DataVencimentoUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Valor_CodigoMoeda = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                    Valor_Valor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ValorAprovado_CodigoMoeda = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                    ValorAprovado_Valor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContasPagar", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ContasReceber",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CampanhaId = table.Column<long>(type: "bigint", nullable: false),
                    ValorRecebidoAcumulado = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DataVencimentoUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Valor_CodigoMoeda = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                    Valor_Valor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ValorAprovado_CodigoMoeda = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                    ValorAprovado_Valor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContasReceber", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ContatosClientes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ClienteEntidadeComercialId = table.Column<long>(type: "bigint", nullable: false),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cargo = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefone = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Principal = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContatosClientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContatosClientes_EntidadesComerciais_ClienteEntidadeComercia~",
                        column: x => x.ClienteEntidadeComercialId,
                        principalTable: "EntidadesComerciais",
                        principalColumn: "EntidadeComercialId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EntregasCampanha",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CampanhaId = table.Column<long>(type: "bigint", nullable: false),
                    ParticipacaoCampanhaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Titulo = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataLimiteUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MotivoRejeicao = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EvidenciaPublicacao = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublicadoEmUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntregasCampanha", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LancamentosLiquidacao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ContaReceberId = table.Column<long>(type: "bigint", nullable: false),
                    ContaPagarId = table.Column<long>(type: "bigint", nullable: true),
                    RepasseCreatorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    LiquidadoEmUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Referencia = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Valor_CodigoMoeda = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                    Valor_Valor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LancamentosLiquidacao", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Oportunidades",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteEntidadeComercialId = table.Column<long>(type: "bigint", nullable: false),
                    ResponsavelId = table.Column<long>(type: "bigint", nullable: false),
                    Briefing = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MotivoPerda = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CampanhaId = table.Column<long>(type: "bigint", nullable: true),
                    OrcamentoEstimado_CodigoMoeda = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                    OrcamentoEstimado_Valor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oportunidades", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ParticipacoesCampanha",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CampanhaId = table.Column<long>(type: "bigint", nullable: false),
                    InfluencerEntidadeComercialId = table.Column<long>(type: "bigint", nullable: false),
                    Termos = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RepasseId = table.Column<long>(type: "bigint", nullable: true),
                    CompensacaoAprovada_CodigoMoeda = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                    CompensacaoAprovada_Valor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CompensacaoNegociada_CodigoMoeda = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                    CompensacaoNegociada_Valor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipacoesCampanha", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RepassesCreators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ParticipacaoCampanhaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ContaPagarId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Valor_CodigoMoeda = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                    Valor_Valor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepassesCreators", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VersoesContrato",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ContratoId = table.Column<long>(type: "bigint", nullable: false),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    CriadaEmUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ConteudoSnapshot = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersoesContrato", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VersoesContrato_Contratos_ContratoId",
                        column: x => x.ContratoId,
                        principalTable: "Contratos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_EntidadesComerciais_Cnpj",
                table: "EntidadesComerciais",
                column: "Cnpj",
                unique: true,
                filter: "`Ativo` = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Contratos_CampanhaId_InfluencerParticipacaoId",
                table: "Contratos",
                columns: new[] { "CampanhaId", "InfluencerParticipacaoId" });

            migrationBuilder.CreateIndex(
                name: "IX_Campanhas_ClienteEntidadeComercialId_Status",
                table: "Campanhas",
                columns: new[] { "ClienteEntidadeComercialId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Campanhas_OportunidadeId",
                table: "Campanhas",
                column: "OportunidadeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContasPagar_CampanhaId_Status",
                table: "ContasPagar",
                columns: new[] { "CampanhaId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_ContasReceber_CampanhaId_Status",
                table: "ContasReceber",
                columns: new[] { "CampanhaId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_ContatosClientes_ClienteEntidadeComercialId_Principal",
                table: "ContatosClientes",
                columns: new[] { "ClienteEntidadeComercialId", "Principal" });

            migrationBuilder.CreateIndex(
                name: "IX_EntregasCampanha_CampanhaId_Status",
                table: "EntregasCampanha",
                columns: new[] { "CampanhaId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_LancamentosLiquidacao_ContaReceberId_ContaPagarId",
                table: "LancamentosLiquidacao",
                columns: new[] { "ContaReceberId", "ContaPagarId" });

            migrationBuilder.CreateIndex(
                name: "IX_Oportunidades_CampanhaId",
                table: "Oportunidades",
                column: "CampanhaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Oportunidades_ClienteEntidadeComercialId_Status",
                table: "Oportunidades",
                columns: new[] { "ClienteEntidadeComercialId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_ParticipacoesCampanha_CampanhaId_InfluencerEntidadeComercial~",
                table: "ParticipacoesCampanha",
                columns: new[] { "CampanhaId", "InfluencerEntidadeComercialId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_VersoesContrato_ContratoId_Numero",
                table: "VersoesContrato",
                columns: new[] { "ContratoId", "Numero" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Campanhas");

            migrationBuilder.DropTable(
                name: "ContasPagar");

            migrationBuilder.DropTable(
                name: "ContasReceber");

            migrationBuilder.DropTable(
                name: "ContatosClientes");

            migrationBuilder.DropTable(
                name: "EntregasCampanha");

            migrationBuilder.DropTable(
                name: "LancamentosLiquidacao");

            migrationBuilder.DropTable(
                name: "Oportunidades");

            migrationBuilder.DropTable(
                name: "ParticipacoesCampanha");

            migrationBuilder.DropTable(
                name: "RepassesCreators");

            migrationBuilder.DropTable(
                name: "VersoesContrato");

            migrationBuilder.DropIndex(
                name: "IX_EntidadesComerciais_Cnpj",
                table: "EntidadesComerciais");

            migrationBuilder.DropIndex(
                name: "IX_Contratos_CampanhaId_InfluencerParticipacaoId",
                table: "Contratos");

            migrationBuilder.DropColumn(
                name: "CampanhaId",
                table: "NotasFiscais");

            migrationBuilder.DropColumn(
                name: "ContaPagarId",
                table: "NotasFiscais");

            migrationBuilder.DropColumn(
                name: "ContaReceberId",
                table: "NotasFiscais");

            migrationBuilder.DropColumn(
                name: "Agencia_Nome",
                table: "EntidadesComerciais");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "EntidadesComerciais");

            migrationBuilder.DropColumn(
                name: "Cnpj",
                table: "EntidadesComerciais");

            migrationBuilder.DropColumn(
                name: "PreferenciaIdioma",
                table: "EntidadesComerciais");

            migrationBuilder.DropColumn(
                name: "Segmento",
                table: "EntidadesComerciais");

            migrationBuilder.DropColumn(
                name: "AssinadoEmUtc",
                table: "Contratos");

            migrationBuilder.DropColumn(
                name: "CampanhaId",
                table: "Contratos");

            migrationBuilder.DropColumn(
                name: "ConteudoContrato",
                table: "Contratos");

            migrationBuilder.DropColumn(
                name: "InfluencerParticipacaoId",
                table: "Contratos");

            migrationBuilder.DropColumn(
                name: "MotivoCancelamento",
                table: "Contratos");

            migrationBuilder.DropColumn(
                name: "StatusCrm",
                table: "Contratos");

            migrationBuilder.DropColumn(
                name: "Versao",
                table: "Contratos");
        }
    }
}
