using Microsoft.EntityFrameworkCore.Migrations;

namespace Avocado.Services.ProductAPI.Migrations
{
    public partial class tableNameUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("AvocadoProductAPI,MultipleActiveResultSets=true");
        }
    }
}
