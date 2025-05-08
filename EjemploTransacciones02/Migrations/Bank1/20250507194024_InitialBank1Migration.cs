using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EjemploTransacciones02.Migrations.Bank1
{
    /// <inheritdoc />
    public partial class InitialBank1Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTransactions", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AccountTransactions",
                columns: new[] { "Id", "AccountNumber", "Credit", "Debit" },
                values: new object[] { 1, "10", 400m, 0m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountTransactions");
        }
    }
}
