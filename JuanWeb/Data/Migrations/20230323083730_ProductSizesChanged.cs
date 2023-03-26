using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class ProductSizesChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductSize");

            migrationBuilder.AddColumn<int>(
                name: "SizeId",
                table: "tbl_products_shoes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_products_shoes_SizeId",
                table: "tbl_products_shoes",
                column: "SizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_products_shoes_tbl_product_Sizes_SizeId",
                table: "tbl_products_shoes",
                column: "SizeId",
                principalTable: "tbl_product_Sizes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_products_shoes_tbl_product_Sizes_SizeId",
                table: "tbl_products_shoes");

            migrationBuilder.DropIndex(
                name: "IX_tbl_products_shoes_SizeId",
                table: "tbl_products_shoes");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "tbl_products_shoes");

            migrationBuilder.CreateTable(
                name: "ProductSize",
                columns: table => new
                {
                    ProductsId = table.Column<int>(type: "int", nullable: false),
                    SizesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSize", x => new { x.ProductsId, x.SizesId });
                    table.ForeignKey(
                        name: "FK_ProductSize_tbl_product_Sizes_SizesId",
                        column: x => x.SizesId,
                        principalTable: "tbl_product_Sizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSize_tbl_products_shoes_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "tbl_products_shoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSize_SizesId",
                table: "ProductSize",
                column: "SizesId");
        }
    }
}
