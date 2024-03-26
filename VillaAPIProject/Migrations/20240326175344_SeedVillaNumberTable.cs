using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VillaAPIProject.Migrations
{
    /// <inheritdoc />
    public partial class SeedVillaNumberTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "VillaNo",
                keyValue: 101,
                column: "CreatedDate",
                value: new DateTime(2024, 3, 26, 13, 53, 44, 369, DateTimeKind.Local).AddTicks(3884));

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "VillaNo",
                keyValue: 102,
                column: "CreatedDate",
                value: new DateTime(2024, 3, 26, 13, 53, 44, 369, DateTimeKind.Local).AddTicks(3891));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 3, 26, 13, 53, 44, 369, DateTimeKind.Local).AddTicks(3558));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 3, 26, 13, 53, 44, 369, DateTimeKind.Local).AddTicks(3619));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "VillaNo",
                keyValue: 101,
                column: "CreatedDate",
                value: new DateTime(2024, 3, 26, 13, 50, 58, 961, DateTimeKind.Local).AddTicks(5112));

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "VillaNo",
                keyValue: 102,
                column: "CreatedDate",
                value: new DateTime(2024, 3, 26, 13, 50, 58, 961, DateTimeKind.Local).AddTicks(5119));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 3, 26, 13, 50, 58, 961, DateTimeKind.Local).AddTicks(4818));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 3, 26, 13, 50, 58, 961, DateTimeKind.Local).AddTicks(4875));
        }
    }
}
