using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicManagementSystem_UWU.Migrations
{
    /// <inheritdoc />
    public partial class appointmentrequesttbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppointmentRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    CliniId = table.Column<int>(type: "int", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApproveUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestingReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovedReason = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointmentRequest_Clinics_CliniId",
                        column: x => x.CliniId,
                        principalTable: "Clinics",
                        principalColumn: "ClinicId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentRequest_PatientDetails_PatientId",
                        column: x => x.PatientId,
                        principalTable: "PatientDetails",
                        principalColumn: "PatientDetailsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentRequest_CliniId",
                table: "AppointmentRequest",
                column: "CliniId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentRequest_PatientId",
                table: "AppointmentRequest",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentRequest");
        }
    }
}
