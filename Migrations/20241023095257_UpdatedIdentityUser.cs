using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Registration.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedIdentityUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1bfece3a-f19c-4d90-a671-befdd1bdee6a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4dee879a-aa2a-44ed-b271-242fcba547ea");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2f33c0a0-9895-49d7-97d5-1c5c9017b0f9", null, "Admin", "ADMIN" },
                    { "aaa9a3a1-6586-425a-b22b-acd9719fd375", null, "Regular", "REGULAR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2f33c0a0-9895-49d7-97d5-1c5c9017b0f9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aaa9a3a1-6586-425a-b22b-acd9719fd375");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1bfece3a-f19c-4d90-a671-befdd1bdee6a", null, "Admin", "ADMIN" },
                    { "4dee879a-aa2a-44ed-b271-242fcba547ea", null, "Regular", "REGULAR" }
                });
        }
    }
}
