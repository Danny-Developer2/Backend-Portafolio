using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddProyectSkillTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectSkills",
                table: "ProjectSkills");

            migrationBuilder.DropIndex(
                name: "IX_ProjectSkills_ProjectId",
                table: "ProjectSkills");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ProjectSkills",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectSkills",
                table: "ProjectSkills",
                columns: new[] { "ProjectId", "SkillId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectSkills",
                table: "ProjectSkills");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ProjectSkills",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectSkills",
                table: "ProjectSkills",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSkills_ProjectId",
                table: "ProjectSkills",
                column: "ProjectId");
        }
    }
}
