using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicManagementSystem_UWU.Migrations
{
    /// <inheritdoc />
    public partial class Doctoraddedrequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "AppointmentRequest",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentRequest_DoctorId",
                table: "AppointmentRequest",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentRequest_DoctorDetails_DoctorId",
                table: "AppointmentRequest",
                column: "DoctorId",
                principalTable: "DoctorDetails",
                principalColumn: "DoctorDetailsId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentRequest_DoctorDetails_DoctorId",
                table: "AppointmentRequest");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentRequest_DoctorId",
                table: "AppointmentRequest");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "AppointmentRequest");
        }
    }
}
