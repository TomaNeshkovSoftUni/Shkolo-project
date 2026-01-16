using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shkolo.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeededGradesFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateGiven",
                value: new DateTime(2025, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateGiven",
                value: new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateGiven",
                value: new DateTime(2024, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateGiven",
                value: new DateTime(2026, 1, 11, 21, 30, 11, 294, DateTimeKind.Local).AddTicks(576));

            migrationBuilder.UpdateData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateGiven",
                value: new DateTime(2026, 1, 14, 21, 30, 11, 294, DateTimeKind.Local).AddTicks(1107));

            migrationBuilder.UpdateData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateGiven",
                value: new DateTime(2026, 1, 16, 21, 30, 11, 294, DateTimeKind.Local).AddTicks(1111));
        }
    }
}
