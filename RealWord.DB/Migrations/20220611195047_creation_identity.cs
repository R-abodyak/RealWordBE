using Microsoft.EntityFrameworkCore.Migrations;

namespace RealWord.DB.Migrations
{
    public partial class creation_identity:Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AlterColumn<int>(
                name: "CommentId" ,
                table: "Comments" ,
                nullable: false ,
                oldClrType: typeof(int) ,
                oldType: "int")
                .Annotation("SqlServer:Identity" ,"1, 1");




            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments" ,
                table: "Comments" ,
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_ArticleId" ,
                table: "Likes" ,
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ArticleId" ,
                table: "Comments" ,
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_User_id" ,
                table: "Comments" ,
                column: "User_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_User_id" ,
                table: "Comments" ,
                column: "User_id" ,
                principalTable: "AspNetUsers" ,
                principalColumn: "Id" ,
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_AspNetUsers_User_id" ,
                table: "Likes" ,
                column: "User_id" ,
                principalTable: "AspNetUsers" ,
                principalColumn: "Id" ,
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
