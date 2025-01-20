using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicManagementSystem_UWU.Migrations
{
    /// <inheritdoc />
    public partial class diagnose : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiagnoseReport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    HeartRate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Diagnose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HealthStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiagnoseReport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiagnoseReport_PatientDetails_PatientId",
                        column: x => x.PatientId,
                        principalTable: "PatientDetails",
                        principalColumn: "PatientDetailsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiagnoseReport_PatientId",
                table: "DiagnoseReport",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiagnoseReport");
        }
    }
}
