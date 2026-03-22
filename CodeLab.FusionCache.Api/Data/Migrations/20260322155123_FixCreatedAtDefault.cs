using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeLab.FusionCache.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixCreatedAtDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "dbo",
                table: "Todos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2026, 3, 21, 19, 56, 48, 960, DateTimeKind.Utc).AddTicks(1501));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "dbo",
                table: "Todos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2026, 3, 21, 19, 56, 48, 960, DateTimeKind.Utc).AddTicks(1501),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");
        }
    }
}
