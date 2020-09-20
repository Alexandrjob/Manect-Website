using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Manect.Migrations.MigrationsProject
{
    public partial class AddLogProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stages_FurnitureProjects_ProjectId",
                table: "Stages");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Stages",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "LogUserActivity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExecutorName = table.Column<string>(nullable: true),
                    ProjectName = table.Column<string>(nullable: true),
                    StageName = table.Column<string>(nullable: true),
                    TimeAction = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogUserActivity", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Stages_FurnitureProjects_ProjectId",
                table: "Stages",
                column: "ProjectId",
                principalTable: "FurnitureProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stages_FurnitureProjects_ProjectId",
                table: "Stages");

            migrationBuilder.DropTable(
                name: "LogUserActivity");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Stages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Stages_FurnitureProjects_ProjectId",
                table: "Stages",
                column: "ProjectId",
                principalTable: "FurnitureProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
