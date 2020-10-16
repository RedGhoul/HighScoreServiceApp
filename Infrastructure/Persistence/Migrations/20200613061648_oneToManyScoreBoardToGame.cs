using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class oneToManyScoreBoardToGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ScoreBoards_GameId",
                table: "ScoreBoards");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreBoards_GameId",
                table: "ScoreBoards",
                column: "GameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ScoreBoards_GameId",
                table: "ScoreBoards");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreBoards_GameId",
                table: "ScoreBoards",
                column: "GameId",
                unique: true);
        }
    }
}
