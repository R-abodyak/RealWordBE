using Microsoft.EntityFrameworkCore.Migrations;

namespace RealWord.DB.Migrations
{
    public partial class Like_relation_and_comment_relation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_AspNetUsers_UserId",
                table: "Likes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Likes",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_UserId",
                table: "Likes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Likes");

            migrationBuilder.AlterColumn<string>(
                name: "User_id",
                table: "Likes",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "LikeId",
                table: "Likes",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "User_id",
                table: "Comments",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CommentId",
                table: "Comments",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Likes",
                table: "Likes",
                column: "LikeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_ArticleId",
                table: "Likes",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_User_id",
                table: "Likes",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ArticleId",
                table: "Comments",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_AspNetUsers_User_id",
                table: "Likes",
                column: "User_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_AspNetUsers_User_id",
                table: "Likes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Likes",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_ArticleId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_User_id",
                table: "Likes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ArticleId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "LikeId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "User_id",
                table: "Likes",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Likes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "User_id",
                table: "Comments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Likes",
                table: "Likes",
                columns: new[] { "ArticleId", "User_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                columns: new[] { "ArticleId", "User_id" });

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserId",
                table: "Likes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_AspNetUsers_UserId",
                table: "Likes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
