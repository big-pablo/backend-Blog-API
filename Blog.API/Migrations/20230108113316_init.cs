using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.API.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TokenEntities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserEntities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostEntities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReadingTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostEntities_UserEntities_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "UserEntities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CommentEntities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ParentCommentId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentEntities_CommentEntities_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalTable: "CommentEntities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CommentEntities_PostEntities_PostId",
                        column: x => x.PostId,
                        principalTable: "PostEntities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CommentEntities_UserEntities_UserId",
                        column: x => x.UserId,
                        principalTable: "UserEntities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TagEntities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostEntityId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TagEntities_PostEntities_PostEntityId",
                        column: x => x.PostEntityId,
                        principalTable: "PostEntities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentEntities_ParentCommentId",
                table: "CommentEntities",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentEntities_PostId",
                table: "CommentEntities",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentEntities_UserId",
                table: "CommentEntities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostEntities_AuthorId",
                table: "PostEntities",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_TagEntities_PostEntityId",
                table: "TagEntities",
                column: "PostEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentEntities");

            migrationBuilder.DropTable(
                name: "TagEntities");

            migrationBuilder.DropTable(
                name: "TokenEntities");

            migrationBuilder.DropTable(
                name: "PostEntities");

            migrationBuilder.DropTable(
                name: "UserEntities");
        }
    }
}
