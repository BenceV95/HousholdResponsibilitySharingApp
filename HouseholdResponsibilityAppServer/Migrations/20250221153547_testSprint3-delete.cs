using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseholdResponsibilityAppServer.Migrations
{
    /// <inheritdoc />
    public partial class testSprint3delete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Households_AspNetUsers_CreatedByUserId",
                table: "Households");

            migrationBuilder.AddForeignKey(
                name: "FK_Households_AspNetUsers_CreatedByUserId",
                table: "Households",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Households_AspNetUsers_CreatedByUserId",
                table: "Households");

            migrationBuilder.AddForeignKey(
                name: "FK_Households_AspNetUsers_CreatedByUserId",
                table: "Households",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
