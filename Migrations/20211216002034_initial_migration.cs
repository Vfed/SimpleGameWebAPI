using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleGameWebAPI.Migrations
{
    public partial class initial_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Score = table.Column<int>(nullable: false),
                    GameSessionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CurrentPlayerId = table.Column<Guid>(nullable: true),
                    GameStatus = table.Column<string>(nullable: true),
                    dateTime = table.Column<DateTime>(nullable: false),
                    Field = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameSessions_Users_CurrentPlayerId",
                        column: x => x.CurrentPlayerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_CurrentPlayerId",
                table: "GameSessions",
                column: "CurrentPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_GameSessionId",
                table: "Users",
                column: "GameSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_GameSessions_GameSessionId",
                table: "Users",
                column: "GameSessionId",
                principalTable: "GameSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameSessions_Users_CurrentPlayerId",
                table: "GameSessions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "GameSessions");
        }
    }
}
