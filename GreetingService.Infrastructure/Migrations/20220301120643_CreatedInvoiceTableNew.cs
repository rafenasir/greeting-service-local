using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreetingService.Infrastructure.Migrations
{
    public partial class CreatedInvoiceTableNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvoiceId",
                table: "Greetings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "invoices",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    CostPerGreeting = table.Column<int>(type: "int", nullable: false),
                    TotalCost = table.Column<int>(type: "int", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_invoices_user_UserEmail",
                        column: x => x.UserEmail,
                        principalTable: "user",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Greetings_InvoiceId",
                table: "Greetings",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_UserEmail",
                table: "invoices",
                column: "UserEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_Greetings_invoices_InvoiceId",
                table: "Greetings",
                column: "InvoiceId",
                principalTable: "invoices",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Greetings_invoices_InvoiceId",
                table: "Greetings");

            migrationBuilder.DropTable(
                name: "invoices");

            migrationBuilder.DropIndex(
                name: "IX_Greetings_InvoiceId",
                table: "Greetings");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "Greetings");
        }
    }
}
