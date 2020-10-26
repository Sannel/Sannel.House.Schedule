using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Sannel.House.Schedule.Data.Migrations.PostgreSQL.Migrations
{
    public partial class inital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    ScheduleId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScheduleKey = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    DefaultMinValue = table.Column<double>(type: "double precision", nullable: false),
                    DefaultMaxValue = table.Column<double>(type: "double precision", nullable: true),
                    MinimumDifference = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.ScheduleId);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleProperties",
                columns: table => new
                {
                    SchedulePropertyId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScheduleId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Value = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleProperties", x => x.SchedulePropertyId);
                    table.ForeignKey(
                        name: "FK_ScheduleProperties_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleStarts",
                columns: table => new
                {
                    ScheduleStartId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScheduleId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Start = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    DurationMinutes = table.Column<int>(type: "integer", nullable: true),
                    ValueMin = table.Column<double>(type: "double precision", nullable: false),
                    ValueMax = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleStarts", x => x.ScheduleStartId);
                    table.ForeignKey(
                        name: "FK_ScheduleStarts_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleProperties_Name",
                table: "ScheduleProperties",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleProperties_ScheduleId_Name",
                table: "ScheduleProperties",
                columns: new[] { "ScheduleId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_ScheduleKey",
                table: "Schedules",
                column: "ScheduleKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleStarts_ScheduleId",
                table: "ScheduleStarts",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleStarts_Start_Type",
                table: "ScheduleStarts",
                columns: new[] { "Start", "Type" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduleProperties");

            migrationBuilder.DropTable(
                name: "ScheduleStarts");

            migrationBuilder.DropTable(
                name: "Schedules");
        }
    }
}
