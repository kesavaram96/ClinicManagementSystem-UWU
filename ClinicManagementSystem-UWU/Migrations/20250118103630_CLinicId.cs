using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicManagementSystem_UWU.Migrations
{
    /// <inheritdoc />
    public partial class CLinicId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Rooms_RoomId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Rooms_RoomId1",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_RoomId1",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "RoomId1",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "PatientCapablity",
                table: "Clinics",
                newName: "PatientCapability");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Appointments",
                newName: "CliniId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_RoomId",
                table: "Appointments",
                newName: "IX_Appointments_CliniId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Clinics_CliniId",
                table: "Appointments",
                column: "CliniId",
                principalTable: "Clinics",
                principalColumn: "ClinicId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Clinics_CliniId",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "PatientCapability",
                table: "Clinics",
                newName: "PatientCapablity");

            migrationBuilder.RenameColumn(
                name: "CliniId",
                table: "Appointments",
                newName: "RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_CliniId",
                table: "Appointments",
                newName: "IX_Appointments_RoomId");

            migrationBuilder.AddColumn<int>(
                name: "RoomId1",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClinicId = table.Column<int>(type: "int", nullable: false),
                    RoomNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomId);
                    table.ForeignKey(
                        name: "FK_Rooms_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "ClinicId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_RoomId1",
                table: "Appointments",
                column: "RoomId1");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_ClinicId",
                table: "Rooms",
                column: "ClinicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Rooms_RoomId",
                table: "Appointments",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Rooms_RoomId1",
                table: "Appointments",
                column: "RoomId1",
                principalTable: "Rooms",
                principalColumn: "RoomId");
        }
    }
}
