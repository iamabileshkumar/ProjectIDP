using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubscriptionMonitoring.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
            name: "Category",
            newName: "Categories");

            migrationBuilder.RenameTable(
            name: "Subscription",
            newName: "Subscriptons");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.RenameTable(
            name: "Categories",
            newName: "Category");

            migrationBuilder.RenameTable(
            name: "Subscriptons",
            newName: "Subscription");
        }
    }
}
