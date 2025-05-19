using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubscriptionMonitoring.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeInNotificationState4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Projects_ProjectId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_ProjectId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Feedbacks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectId",
                table: "Feedbacks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_ProjectId",
                table: "Feedbacks",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Projects_ProjectId",
                table: "Feedbacks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId");
        }
    }
}
