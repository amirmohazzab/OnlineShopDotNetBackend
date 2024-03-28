using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class AddFluentApi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PProducts",
                schema: "Base",
                table: "PProducts");

            migrationBuilder.RenameTable(
                name: "PProducts",
                schema: "Base",
                newName: "Products");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "Products",
                newName: "Title");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Products",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.EnsureSchema(
                name: "Base");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "PProducts",
                newSchema: "Base");

            migrationBuilder.RenameColumn(
                name: "Title",
                schema: "Base",
                table: "PProducts",
                newName: "ProductName");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                schema: "Base",
                table: "PProducts",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256)
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PProducts",
                schema: "Base",
                table: "PProducts",
                column: "Id");
        }
    }
}
