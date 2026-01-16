using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Shkolo.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeededGrades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Mathematics" },
                    { 2, "History" }
                });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "DateGiven", "StudentId", "SubjectId", "TeacherId", "Value" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 11, 21, 30, 11, 294, DateTimeKind.Local).AddTicks(576), 1, 1, 1, 5.5 },
                    { 2, new DateTime(2026, 1, 14, 21, 30, 11, 294, DateTimeKind.Local).AddTicks(1107), 1, 2, 1, 6.0 },
                    { 3, new DateTime(2026, 1, 16, 21, 30, 11, 294, DateTimeKind.Local).AddTicks(1111), 1, 1, 1, 4.75 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Password", "Role", "Status", "Username" },
                values: new object[] { 4, "123", 1, 1, "bad_user" });
        }
    }
}
