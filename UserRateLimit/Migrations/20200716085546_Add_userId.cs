using Microsoft.EntityFrameworkCore.Migrations;

namespace UserRateLimit.Migrations
{
    public partial class Add_userId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Tb_RateLimits",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tb_RateLimits");
        }
    }
}
