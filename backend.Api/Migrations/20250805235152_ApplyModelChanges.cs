using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Api.Migrations
{
    /// <inheritdoc />
    public partial class ApplyModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RideRequests_Tenants_TenantId",
                table: "RideRequests");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "RideRequests",
                newName: "TenantNameId");

            migrationBuilder.RenameIndex(
                name: "IX_RideRequests_TenantId",
                table: "RideRequests",
                newName: "IX_RideRequests_TenantNameId");

            migrationBuilder.AddForeignKey(
                name: "FK_RideRequests_Tenants_TenantNameId",
                table: "RideRequests",
                column: "TenantNameId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RideRequests_Tenants_TenantNameId",
                table: "RideRequests");

            migrationBuilder.RenameColumn(
                name: "TenantNameId",
                table: "RideRequests",
                newName: "TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_RideRequests_TenantNameId",
                table: "RideRequests",
                newName: "IX_RideRequests_TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_RideRequests_Tenants_TenantId",
                table: "RideRequests",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
