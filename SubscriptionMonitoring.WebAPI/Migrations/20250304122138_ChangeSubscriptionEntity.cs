using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubscriptionMonitoring.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSubscriptionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Subscriptions");
        }
    }
}
