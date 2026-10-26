using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace PortalElectoral.Migrations
{
    /// <inheritdoc />
    public partial class CrearEsquemaInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "elecciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    FechaConvocatoria = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaVotacion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Estado = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    EsSimulacion = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_elecciones", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "partidos_politicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    Siglas = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_partidos_politicos", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "provincias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ComunidadAutonoma = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    EscanosCongreso = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provincias", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "propuestas_electorales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Titulo = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Area = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    PartidoPoliticoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_propuestas_electorales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_propuestas_electorales_partidos_politicos_PartidoPoliticoId",
                        column: x => x.PartidoPoliticoId,
                        principalTable: "partidos_politicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "candidaturas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    EleccionId = table.Column<int>(type: "int", nullable: false),
                    ProvinciaId = table.Column<int>(type: "int", nullable: false),
                    PartidoPoliticoId = table.Column<int>(type: "int", nullable: false),
                    Votos = table.Column<int>(type: "int", nullable: false),
                    EscanosObtenidos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_candidaturas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_candidaturas_elecciones_EleccionId",
                        column: x => x.EleccionId,
                        principalTable: "elecciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_candidaturas_partidos_politicos_PartidoPoliticoId",
                        column: x => x.PartidoPoliticoId,
                        principalTable: "partidos_politicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_candidaturas_provincias_ProvinciaId",
                        column: x => x.ProvinciaId,
                        principalTable: "provincias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "candidatos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Apellidos = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    OrdenLista = table.Column<int>(type: "int", nullable: false),
                    EsSuplente = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CandidaturaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_candidatos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_candidatos_candidaturas_CandidaturaId",
                        column: x => x.CandidaturaId,
                        principalTable: "candidaturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_candidatos_CandidaturaId_OrdenLista",
                table: "candidatos",
                columns: new[] { "CandidaturaId", "OrdenLista" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_candidaturas_EleccionId_ProvinciaId_PartidoPoliticoId",
                table: "candidaturas",
                columns: new[] { "EleccionId", "ProvinciaId", "PartidoPoliticoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_candidaturas_PartidoPoliticoId",
                table: "candidaturas",
                column: "PartidoPoliticoId");

            migrationBuilder.CreateIndex(
                name: "IX_candidaturas_ProvinciaId",
                table: "candidaturas",
                column: "ProvinciaId");

            migrationBuilder.CreateIndex(
                name: "IX_partidos_politicos_Siglas",
                table: "partidos_politicos",
                column: "Siglas",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_propuestas_electorales_PartidoPoliticoId",
                table: "propuestas_electorales",
                column: "PartidoPoliticoId");

            migrationBuilder.CreateIndex(
                name: "IX_provincias_Nombre",
                table: "provincias",
                column: "Nombre",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "candidatos");

            migrationBuilder.DropTable(
                name: "propuestas_electorales");

            migrationBuilder.DropTable(
                name: "candidaturas");

            migrationBuilder.DropTable(
                name: "elecciones");

            migrationBuilder.DropTable(
                name: "partidos_politicos");

            migrationBuilder.DropTable(
                name: "provincias");
        }
    }
}
