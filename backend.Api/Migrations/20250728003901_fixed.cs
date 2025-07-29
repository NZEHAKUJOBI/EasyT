using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Api.Migrations
{
    /// <inheritdoc />
    public partial class @fixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RideRequests_Users_RiderId",
                table: "RideRequests");

            migrationBuilder.RenameColumn(
                name: "RiderId",
                table: "RideRequests",
                newName: "PassengerId");

            migrationBuilder.RenameIndex(
                name: "IX_RideRequests_RiderId",
                table: "RideRequests",
                newName: "IX_RideRequests_PassengerId");

            migrationBuilder.AddColumn<double>(
                name: "DropoffLatitude",
                table: "RideRequests",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DropoffLongitude",
                table: "RideRequests",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "PassengerName",
                table: "RideRequests",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "PickupLatitude",
                table: "RideRequests",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PickupLongitude",
                table: "RideRequests",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddForeignKey(
                name: "FK_RideRequests_Users_PassengerId",
                table: "RideRequests",
                column: "PassengerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RideRequests_Users_PassengerId",
                table: "RideRequests");

            migrationBuilder.DropColumn(
                name: "DropoffLatitude",
                table: "RideRequests");

            migrationBuilder.DropColumn(
                name: "DropoffLongitude",
                table: "RideRequests");

            migrationBuilder.DropColumn(
                name: "PassengerName",
                table: "RideRequests");

            migrationBuilder.DropColumn(
                name: "PickupLatitude",
                table: "RideRequests");

            migrationBuilder.DropColumn(
                name: "PickupLongitude",
                table: "RideRequests");

            migrationBuilder.RenameColumn(
                name: "PassengerId",
                table: "RideRequests",
                newName: "RiderId");

            migrationBuilder.RenameIndex(
                name: "IX_RideRequests_PassengerId",
                table: "RideRequests",
                newName: "IX_RideRequests_RiderId");

            migrationBuilder.AddForeignKey(
                name: "FK_RideRequests_Users_RiderId",
                table: "RideRequests",
                column: "RiderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
