using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mini_crm.Migrations
{
    /// <inheritdoc />
    public partial class CrmV1ApiEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CandidatosCuradoria",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SolicitacaoCuradoriaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InfluencerEntidadeComercialId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Avaliacao = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidatosCuradoria", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Leads",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Empresa = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefone = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ResponsavelId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leads", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MarcosCampanha",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CampanhaId = table.Column<long>(type: "bigint", nullable: false),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataLimiteUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Concluido = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarcosCampanha", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NotificacoesCrm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Titulo = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Mensagem = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Lida = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CriadaEmUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificacoesCrm", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Propostas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OportunidadeId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Versao = table.Column<int>(type: "int", nullable: false),
                    ValorTotal_CodigoMoeda = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                    ValorTotal_Valor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Propostas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SolicitacoesCuradoria",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CampanhaId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Briefing = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitacoesCuradoria", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TarefasCrm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Titulo = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ResponsavelId = table.Column<long>(type: "bigint", nullable: true),
                    PrazoUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TarefasCrm", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ItensProposta",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PropostaId = table.Column<long>(type: "bigint", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantidade = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ValorUnitario_CodigoMoeda = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                    ValorUnitario_Valor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensProposta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensProposta_Propostas_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "Propostas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CandidatosCuradoria_SolicitacaoCuradoriaId_InfluencerEntidad~",
                table: "CandidatosCuradoria",
                columns: new[] { "SolicitacaoCuradoriaId", "InfluencerEntidadeComercialId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensProposta_PropostaId",
                table: "ItensProposta",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_MarcosCampanha_CampanhaId_DataLimiteUtc",
                table: "MarcosCampanha",
                columns: new[] { "CampanhaId", "DataLimiteUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_Propostas_OportunidadeId_Versao",
                table: "Propostas",
                columns: new[] { "OportunidadeId", "Versao" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CandidatosCuradoria");

            migrationBuilder.DropTable(
                name: "ItensProposta");

            migrationBuilder.DropTable(
                name: "Leads");

            migrationBuilder.DropTable(
                name: "MarcosCampanha");

            migrationBuilder.DropTable(
                name: "NotificacoesCrm");

            migrationBuilder.DropTable(
                name: "SolicitacoesCuradoria");

            migrationBuilder.DropTable(
                name: "TarefasCrm");

            migrationBuilder.DropTable(
                name: "Propostas");
        }
    }
}
