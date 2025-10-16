using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolTripApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Rg_Property_To_Guardian_and_Student : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Rg",
                table: "Students",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiresAt",
                table: "RefreshTokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 10, 13, 19, 24, 34, 703, DateTimeKind.Utc).AddTicks(4410),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 10, 13, 17, 27, 56, 430, DateTimeKind.Utc).AddTicks(6224));

            migrationBuilder.AddColumn<string>(
                name: "Rg",
                table: "Guardians",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rg",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Rg",
                table: "Guardians");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiresAt",
                table: "RefreshTokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 10, 13, 17, 27, 56, 430, DateTimeKind.Utc).AddTicks(6224),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 10, 13, 19, 24, 34, 703, DateTimeKind.Utc).AddTicks(4410));
        }
    }
}
