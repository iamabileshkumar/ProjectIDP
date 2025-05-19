using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubscriptionMonitoring.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSubscriptionTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
            migrationBuilder.RenameTable(
            name: "Subscriptons",
            newName: "Subscriptions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.RenameTable(
            name: "Subscriptions",
            newName: "Subscriptons");
        }
    }
}
