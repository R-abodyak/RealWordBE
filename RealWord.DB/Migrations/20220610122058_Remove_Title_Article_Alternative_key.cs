using Microsoft.EntityFrameworkCore.Migrations;

namespace RealWord.DB.Migrations
{
    public partial class Remove_Title_Article_Alternative_key:Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Articles_Title" ,
                table: "Articles");



            migrationBuilder.AlterColumn<string>(
                name: "Title" ,
                table: "Articles" ,
                nullable: false ,
                oldClrType: typeof(string) ,
                oldType: "nvarchar(450)");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_AspNetUsers_User_id" ,
                table: "Likes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Likes" ,
                table: "Likes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments" ,
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "LikeId" ,
                table: "Likes" ,
                type: "int" ,
                nullable: false ,
                oldClrType: typeof(int))
                .Annotation("SqlServer:Identity" ,"1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "User_id" ,
                table: "Likes" ,
                type: "nvarchar(450)" ,
                nullable: true ,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "CommentId" ,
                table: "Comments" ,
                type: "int" ,
                nullable: false ,
                oldClrType: typeof(int))
                .Annotation("SqlServer:Identity" ,"1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "User_id" ,
                table: "Comments" ,
                type: "nvarchar(max)" ,
                nullable: true ,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Title" ,
                table: "Articles" ,
                type: "nvarchar(450)" ,
                nullable: false ,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Likes" ,
                table: "Likes" ,
                column: "LikeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments" ,
                table: "Comments" ,
                column: "CommentId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Articles_Title" ,
                table: "Articles" ,
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_ArticleId" ,
                table: "Likes" ,
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ArticleId" ,
                table: "Comments" ,
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_AspNetUsers_User_id" ,
                table: "Likes" ,
                column: "User_id" ,
                principalTable: "AspNetUsers" ,
                principalColumn: "Id" ,
                onDelete: ReferentialAction.Restrict);
        }
    }
}
