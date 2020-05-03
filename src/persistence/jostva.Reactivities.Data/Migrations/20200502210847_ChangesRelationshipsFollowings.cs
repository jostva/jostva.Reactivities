using Microsoft.EntityFrameworkCore.Migrations;

namespace jostva.Reactivities.Data.Migrations
{
    public partial class ChangesRelationshipsFollowings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Followings_AspNetUsers_ObserverId1",
                table: "Followings");

            migrationBuilder.DropIndex(
                name: "IX_Followings_ObserverId1",
                table: "Followings");

            migrationBuilder.DropColumn(
                name: "ObserverId1",
                table: "Followings");

            migrationBuilder.CreateIndex(
                name: "IX_Followings_TargetId",
                table: "Followings",
                column: "TargetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Followings_AspNetUsers_TargetId",
                table: "Followings",
                column: "TargetId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Followings_AspNetUsers_TargetId",
                table: "Followings");

            migrationBuilder.DropIndex(
                name: "IX_Followings_TargetId",
                table: "Followings");

            migrationBuilder.AddColumn<string>(
                name: "ObserverId1",
                table: "Followings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Followings_ObserverId1",
                table: "Followings",
                column: "ObserverId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Followings_AspNetUsers_ObserverId1",
                table: "Followings",
                column: "ObserverId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
