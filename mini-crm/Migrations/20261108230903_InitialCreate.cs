using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mini_crm.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Agentes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Contato = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agentes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Bancos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bancos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EntidadesComerciais",
                columns: table => new
                {
                    EntidadeComercialId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Tipo = table.Column<string>(type: "varchar(21)", maxLength: 21, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Agencia_Id = table.Column<int>(type: "int", nullable: true),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Codigo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Influencer_Id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Intermediario_Id = table.Column<int>(type: "int", nullable: true),
                    Intermediario_Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntidadesComerciais", x => x.EntidadeComercialId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FormatosNotaFiscal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormatosNotaFiscal", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RedesSociais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedesSociais", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StatusNotasFiscais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusNotasFiscais", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ContasBancarias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BancoId = table.Column<int>(type: "int", nullable: false),
                    Agencia = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Conta = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContasBancarias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContasBancarias_Bancos_BancoId",
                        column: x => x.BancoId,
                        principalTable: "Bancos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AgenciaAgentes",
                columns: table => new
                {
                    AgenciaEntidadeComercialId = table.Column<long>(type: "bigint", nullable: false),
                    AgenteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgenciaAgentes", x => new { x.AgenciaEntidadeComercialId, x.AgenteId });
                    table.ForeignKey(
                        name: "FK_AgenciaAgentes_Agentes_AgenteId",
                        column: x => x.AgenteId,
                        principalTable: "Agentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgenciaAgentes_EntidadesComerciais_AgenciaEntidadeComercialId",
                        column: x => x.AgenciaEntidadeComercialId,
                        principalTable: "EntidadesComerciais",
                        principalColumn: "EntidadeComercialId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NotasFiscais",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FormatoNotaFiscalId = table.Column<int>(type: "int", nullable: false),
                    RazaoSocial = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Enviada = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotasFiscais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotasFiscais_FormatosNotaFiscal_FormatoNotaFiscalId",
                        column: x => x.FormatoNotaFiscalId,
                        principalTable: "FormatosNotaFiscal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InfluencerRedesSociais",
                columns: table => new
                {
                    InfluencerEntidadeComercialId = table.Column<long>(type: "bigint", nullable: false),
                    RedeSocialId = table.Column<int>(type: "int", nullable: false),
                    Link = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfluencerRedesSociais", x => new { x.InfluencerEntidadeComercialId, x.RedeSocialId });
                    table.ForeignKey(
                        name: "FK_InfluencerRedesSociais_EntidadesComerciais_InfluencerEntidad~",
                        column: x => x.InfluencerEntidadeComercialId,
                        principalTable: "EntidadesComerciais",
                        principalColumn: "EntidadeComercialId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InfluencerRedesSociais_RedesSociais_RedeSocialId",
                        column: x => x.RedeSocialId,
                        principalTable: "RedesSociais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InfluencerContasBancarias",
                columns: table => new
                {
                    InfluencerEntidadeComercialId = table.Column<long>(type: "bigint", nullable: false),
                    ContaBancariaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfluencerContasBancarias", x => new { x.InfluencerEntidadeComercialId, x.ContaBancariaId });
                    table.ForeignKey(
                        name: "FK_InfluencerContasBancarias_ContasBancarias_ContaBancariaId",
                        column: x => x.ContaBancariaId,
                        principalTable: "ContasBancarias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InfluencerContasBancarias_EntidadesComerciais_InfluencerEnti~",
                        column: x => x.InfluencerEntidadeComercialId,
                        principalTable: "EntidadesComerciais",
                        principalColumn: "EntidadeComercialId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Contratos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClienteEntidadeComercialId = table.Column<long>(type: "bigint", nullable: false),
                    AgenciaEntidadeComercialId = table.Column<long>(type: "bigint", nullable: false),
                    AgenteId = table.Column<int>(type: "int", nullable: false),
                    Divisao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Moeda = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InfluencerEntidadeComercialId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contratos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contratos_AgenciaAgentes_AgenciaEntidadeComercialId_AgenteId",
                        columns: x => new { x.AgenciaEntidadeComercialId, x.AgenteId },
                        principalTable: "AgenciaAgentes",
                        principalColumns: new[] { "AgenciaEntidadeComercialId", "AgenteId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contratos_EntidadesComerciais_ClienteEntidadeComercialId",
                        column: x => x.ClienteEntidadeComercialId,
                        principalTable: "EntidadesComerciais",
                        principalColumn: "EntidadeComercialId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contratos_EntidadesComerciais_InfluencerEntidadeComercialId",
                        column: x => x.InfluencerEntidadeComercialId,
                        principalTable: "EntidadesComerciais",
                        principalColumn: "EntidadeComercialId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AgendamentosNotaFiscal",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NotaFiscalId = table.Column<long>(type: "bigint", nullable: false),
                    DataPrazoRepasse = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataPrazoRepasseObservacao = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataPagamento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    StatusNotaFiscalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgendamentosNotaFiscal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgendamentosNotaFiscal_NotasFiscais_NotaFiscalId",
                        column: x => x.NotaFiscalId,
                        principalTable: "NotasFiscais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgendamentosNotaFiscal_StatusNotasFiscais_StatusNotaFiscalId",
                        column: x => x.StatusNotaFiscalId,
                        principalTable: "StatusNotasFiscais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Despesas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ContratoId = table.Column<long>(type: "bigint", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Observacao = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Despesas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Despesas_Contratos_ContratoId",
                        column: x => x.ContratoId,
                        principalTable: "Contratos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pagamentos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DespesaId = table.Column<long>(type: "bigint", nullable: true),
                    ContratoId = table.Column<long>(type: "bigint", nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    EntidadeComercialId = table.Column<long>(type: "bigint", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    NotaFiscalId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagamentos_Contratos_ContratoId",
                        column: x => x.ContratoId,
                        principalTable: "Contratos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Pagamentos_Despesas_DespesaId",
                        column: x => x.DespesaId,
                        principalTable: "Despesas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Pagamentos_EntidadesComerciais_EntidadeComercialId",
                        column: x => x.EntidadeComercialId,
                        principalTable: "EntidadesComerciais",
                        principalColumn: "EntidadeComercialId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pagamentos_NotasFiscais_NotaFiscalId",
                        column: x => x.NotaFiscalId,
                        principalTable: "NotasFiscais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AgenciaAgentes_AgenteId",
                table: "AgenciaAgentes",
                column: "AgenteId");

            migrationBuilder.CreateIndex(
                name: "IX_AgendamentosNotaFiscal_NotaFiscalId",
                table: "AgendamentosNotaFiscal",
                column: "NotaFiscalId");

            migrationBuilder.CreateIndex(
                name: "IX_AgendamentosNotaFiscal_StatusNotaFiscalId",
                table: "AgendamentosNotaFiscal",
                column: "StatusNotaFiscalId");

            migrationBuilder.CreateIndex(
                name: "IX_ContasBancarias_BancoId",
                table: "ContasBancarias",
                column: "BancoId");

            migrationBuilder.CreateIndex(
                name: "IX_Contratos_AgenciaEntidadeComercialId_AgenteId",
                table: "Contratos",
                columns: new[] { "AgenciaEntidadeComercialId", "AgenteId" });

            migrationBuilder.CreateIndex(
                name: "IX_Contratos_ClienteEntidadeComercialId",
                table: "Contratos",
                column: "ClienteEntidadeComercialId");

            migrationBuilder.CreateIndex(
                name: "IX_Contratos_InfluencerEntidadeComercialId",
                table: "Contratos",
                column: "InfluencerEntidadeComercialId");

            migrationBuilder.CreateIndex(
                name: "IX_Despesas_ContratoId",
                table: "Despesas",
                column: "ContratoId");

            migrationBuilder.CreateIndex(
                name: "IX_InfluencerContasBancarias_ContaBancariaId",
                table: "InfluencerContasBancarias",
                column: "ContaBancariaId");

            migrationBuilder.CreateIndex(
                name: "IX_InfluencerRedesSociais_RedeSocialId",
                table: "InfluencerRedesSociais",
                column: "RedeSocialId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasFiscais_FormatoNotaFiscalId",
                table: "NotasFiscais",
                column: "FormatoNotaFiscalId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_ContratoId",
                table: "Pagamentos",
                column: "ContratoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_DespesaId",
                table: "Pagamentos",
                column: "DespesaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_EntidadeComercialId",
                table: "Pagamentos",
                column: "EntidadeComercialId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_NotaFiscalId",
                table: "Pagamentos",
                column: "NotaFiscalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgendamentosNotaFiscal");

            migrationBuilder.DropTable(
                name: "InfluencerContasBancarias");

            migrationBuilder.DropTable(
                name: "InfluencerRedesSociais");

            migrationBuilder.DropTable(
                name: "Pagamentos");

            migrationBuilder.DropTable(
                name: "StatusNotasFiscais");

            migrationBuilder.DropTable(
                name: "ContasBancarias");

            migrationBuilder.DropTable(
                name: "RedesSociais");

            migrationBuilder.DropTable(
                name: "Despesas");

            migrationBuilder.DropTable(
                name: "NotasFiscais");

            migrationBuilder.DropTable(
                name: "Bancos");

            migrationBuilder.DropTable(
                name: "Contratos");

            migrationBuilder.DropTable(
                name: "FormatosNotaFiscal");

            migrationBuilder.DropTable(
                name: "AgenciaAgentes");

            migrationBuilder.DropTable(
                name: "Agentes");

            migrationBuilder.DropTable(
                name: "EntidadesComerciais");
        }
    }
}
