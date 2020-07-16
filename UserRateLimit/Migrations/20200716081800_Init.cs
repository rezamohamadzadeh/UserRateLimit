using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserRateLimit.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tb_RateLimits",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserIP = table.Column<string>(nullable: true),
                    LastLimit = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_RateLimits", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tb_RateLimits");
        }
    }
}
