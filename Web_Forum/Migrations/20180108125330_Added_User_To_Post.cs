using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Web_Forum.Migrations
{
    public partial class Added_User_To_Post : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Posts",
                newName: "PosterId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Posts",
                newName: "PostId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfCreation",
                table: "Posts",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PosterId",
                table: "Posts",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PosterId",
                table: "Posts",
                column: "PosterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_PosterId",
                table: "Posts",
                column: "PosterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_PosterId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_PosterId",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "PosterId",
                table: "Posts",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "Posts",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "DateOfCreation",
                table: "Posts",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Posts",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
