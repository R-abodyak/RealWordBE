using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RealWord.DB.Migrations
{
    public partial class DBSchema:Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles" ,
                columns: table => new
                {
                    ArticlId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity" ,"1, 1") ,
                    Title = table.Column<string>(nullable: true) ,
                    Description = table.Column<string>(nullable: true) ,
                    Slug = table.Column<string>(nullable: true) ,
                    CreatedAt = table.Column<DateTime>(nullable: false) ,
                    UpdatedAt = table.Column<DateTime>(nullable: true)
                } ,
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles" ,x => x.ArticlId);
                });

            migrationBuilder.CreateTable(
                name: "Folower" ,
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false) ,
                    followerId = table.Column<int>(nullable: false) ,
                    UserId1 = table.Column<string>(nullable: true) ,
                    followerId1 = table.Column<string>(nullable: true)
                } ,
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folower" ,x => new { x.UserId ,x.followerId });
                    table.ForeignKey(
                        name: "FK_Folower_AspNetUsers_UserId1" ,
                        column: x => x.UserId1 ,
                        principalTable: "AspNetUsers" ,
                        principalColumn: "Id" ,
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Folower_AspNetUsers_followerId1" ,
                        column: x => x.followerId1 ,
                        principalTable: "AspNetUsers" ,
                        principalColumn: "Id" ,
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tags" ,
                columns: table => new
                {
                    TagId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity" ,"1, 1") ,
                    Name = table.Column<string>(nullable: true)
                } ,
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags" ,x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "Comments" ,
                columns: table => new
                {
                    ArticleId = table.Column<int>(nullable: false) ,
                    User_id = table.Column<int>(nullable: false) ,
                    CommentMsg = table.Column<string>(nullable: true) ,
                    CreatedAt = table.Column<DateTime>(nullable: false) ,
                    UpdatedAt = table.Column<DateTime>(nullable: true) ,
                    UserId = table.Column<string>(nullable: true)
                } ,
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments" ,x => new { x.ArticleId ,x.User_id });
                    table.ForeignKey(
                        name: "FK_Comments_Articles_ArticleId" ,
                        column: x => x.ArticleId ,
                        principalTable: "Articles" ,
                        principalColumn: "ArticlId" ,
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_UserId" ,
                        column: x => x.UserId ,
                        principalTable: "AspNetUsers" ,
                        principalColumn: "Id" ,
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Likes" ,
                columns: table => new
                {
                    ArticleId = table.Column<int>(nullable: false) ,
                    User_id = table.Column<int>(nullable: false) ,
                    CreatedAt = table.Column<DateTime>(nullable: false) ,
                    UserId = table.Column<string>(nullable: true)
                } ,
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes" ,x => new { x.ArticleId ,x.User_id });
                    table.ForeignKey(
                        name: "FK_Likes_Articles_ArticleId" ,
                        column: x => x.ArticleId ,
                        principalTable: "Articles" ,
                        principalColumn: "ArticlId" ,
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Likes_AspNetUsers_UserId" ,
                        column: x => x.UserId ,
                        principalTable: "AspNetUsers" ,
                        principalColumn: "Id" ,
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleTag" ,
                columns: table => new
                {
                    ArticleId = table.Column<int>(nullable: false) ,
                    TagId = table.Column<int>(nullable: false)
                } ,
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTag" ,x => new { x.ArticleId ,x.TagId });
                    table.ForeignKey(
                        name: "FK_ArticleTag_Articles_ArticleId" ,
                        column: x => x.ArticleId ,
                        principalTable: "Articles" ,
                        principalColumn: "ArticlId" ,
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleTag_Tags_TagId" ,
                        column: x => x.TagId ,
                        principalTable: "Tags" ,
                        principalColumn: "TagId" ,
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTag_TagId" ,
                table: "ArticleTag" ,
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId" ,
                table: "Comments" ,
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Folower_UserId1" ,
                table: "Folower" ,
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Folower_followerId1" ,
                table: "Folower" ,
                column: "followerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserId" ,
                table: "Likes" ,
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleTag");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Folower");

            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Articles");
        }
    }
}
