using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Web_Forum.Migrations
{
    public partial class Added_Thread_Class : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ThreadId",
                table: "Posts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Threads",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AmountOfReplies = table.Column<int>(nullable: false),
                    AmountOfViews = table.Column<int>(nullable: false),
                    DateOfCreation = table.Column<string>(nullable: true),
                    DateOfLastUpdate = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Threads", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Threads");

            migrationBuilder.DropColumn(
                name: "ThreadId",
                table: "Posts");
        }
    }
}
