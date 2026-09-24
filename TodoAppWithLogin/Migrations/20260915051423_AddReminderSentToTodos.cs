using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoAppWithLogin.Migrations
{
    /// <inheritdoc />
    public partial class AddReminderSentToTodos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ReminderSent",
                table: "Todos",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReminderSent",
                table: "Todos");
        }
    }
}
