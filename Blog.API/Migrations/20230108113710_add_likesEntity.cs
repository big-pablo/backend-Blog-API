using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.API.Migrations
{
    public partial class add_likesEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LikeEntities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikeEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LikeEntities_PostEntities_PostId",
                        column: x => x.PostId,
                        principalTable: "PostEntities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LikeEntities_UserEntities_UserId",
                        column: x => x.UserId,
                        principalTable: "UserEntities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LikeEntities_PostId",
                table: "LikeEntities",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_LikeEntities_UserId",
                table: "LikeEntities",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LikeEntities");
        }
    }
}
