using Microsoft.EntityFrameworkCore.Migrations;

namespace Avocado.Services.ProductAPI.Migrations
{
    public partial class ProductAPIStoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Create proc _spCreateProduct
                                    @productId int output,
                                    @name nvarchar(max), @price float, @image nvarchar(max), @category nvarchar(max), @description nvarchar(max)
                                    as
                                    begin
                                    Insert into Products (Name, Price, ImageUrl, CategoryName, Description) 
                                    values (@name, @price, @image, @category, @description);
                                    Select @productId=scope_identity();
                                    end");
            migrationBuilder.Sql(@"Create proc _spUpdateProduct
                                    @id int, @name nvarchar(max), @price float, @image nvarchar(max), @category nvarchar(max), @description nvarchar(max)
                                    as
                                    begin
                                    Update Products set Name=@name, Price=@price, ImageUrl=@image, CategoryName=@category, Description=@description 
                                    where Id=@id
                                    end");
            migrationBuilder.Sql(@"Create proc _spDeleteProduct
                                    @id int
                                    as
                                    begin
                                    Delete from Products 
                                    where Id=@id
                                    end");
            migrationBuilder.Sql(@"Create proc _spGetProductById
                                    @productId int 
                                    as
                                    begin
                                    Select * from Products where Id=@productId
                                    end");
            migrationBuilder.Sql(@"Create proc _spGetAllProducts
                                    as
                                    begin
                                    Select * from Products
                                    end");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
