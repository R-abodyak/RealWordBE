using Microsoft.EntityFrameworkCore.Migrations;

namespace RealWord.DB.Migrations
{
    public partial class drop_table_comment:Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_UserId" ,
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_AspNetUsers_User_id" ,
                table: "Likes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Likes" ,
                table: "Likes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments" ,
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId" ,
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserId" ,
                table: "Comments");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_User_id" ,
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_AspNetUsers_User_id" ,
                table: "Likes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Likes" ,
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_ArticleId" ,
                table: "Likes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments" ,
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ArticleId" ,
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_User_id" ,
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "User_id" ,
                table: "Likes" ,
                type: "nvarchar(450)" ,
                nullable: false ,
                oldClrType: typeof(string) ,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LikeId" ,
                table: "Likes" ,
                type: "int" ,
                nullable: false ,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:Identity" ,"1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "User_id" ,
                table: "Comments" ,
                type: "nvarchar(450)" ,
                nullable: false ,
                oldClrType: typeof(string) ,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CommentId" ,
                table: "Comments" ,
                type: "int" ,
                nullable: false ,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:Identity" ,"1, 1");

            migrationBuilder.AddColumn<string>(
                name: "UserId" ,
                table: "Comments" ,
                type: "nvarchar(450)" ,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Likes" ,
                table: "Likes" ,
                columns: new[] { "ArticleId" ,"User_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments" ,
                table: "Comments" ,
                columns: new[] { "ArticleId" ,"User_id" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId" ,
                table: "Comments" ,
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_UserId" ,
                table: "Comments" ,
                column: "UserId" ,
                principalTable: "AspNetUsers" ,
                principalColumn: "Id" ,
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_AspNetUsers_User_id" ,
                table: "Likes" ,
                column: "User_id" ,
                principalTable: "AspNetUsers" ,
                principalColumn: "Id" ,
                onDelete: ReferentialAction.Cascade);
        }
    }
}
