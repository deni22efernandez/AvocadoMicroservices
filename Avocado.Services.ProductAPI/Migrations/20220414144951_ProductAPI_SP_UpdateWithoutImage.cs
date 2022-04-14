using Microsoft.EntityFrameworkCore.Migrations;

namespace Avocado.Services.ProductAPI.Migrations
{
    public partial class ProductAPI_SP_UpdateWithoutImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Create proc _spUpdateProductWithoutImage
                                    @id int, @name nvarchar(max), @price float, @category nvarchar(max), @description nvarchar(max)
                                    as
                                    begin
                                    Update Products set Name=@name, Price=@price, CategoryName=@category, Description=@description 
                                    where Id=@id
                                    end");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
