using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCEService.Migrations
{
    /// <inheritdoc />
    public partial class FixFitnessPlanConfigStatusToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Drop indexes that depend on [Status] column (SQL Server blocks DROP COLUMN otherwise)
            migrationBuilder.DropIndex(name: "IX_FitnessPlanConfigs_Goal_Status", table: "FitnessPlanConfigs");

            // Step 2: Add a temporary string column
            migrationBuilder.AddColumn<string>(
                name: "Status_New",
                table: "FitnessPlanConfigs",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Normal");

            // Step 3: Convert existing int values to their string equivalents
            // FitnessStatus enum: 0=Weak, 1=Normal, 2=Hard
            migrationBuilder.Sql(@"
                UPDATE [FitnessPlanConfigs]
                SET [Status_New] = CASE [Status]
                    WHEN 0 THEN 'Weak'
                    WHEN 1 THEN 'Normal'
                    WHEN 2 THEN 'Hard'
                    ELSE 'Normal'
                END
            ");

            // Step 4: Drop the old int column (now safe — index already dropped)
            migrationBuilder.DropColumn(name: "Status", table: "FitnessPlanConfigs");

            // Step 5: Rename the new string column to the original name
            migrationBuilder.RenameColumn(
                name: "Status_New",
                table: "FitnessPlanConfigs",
                newName: "Status");

            // Step 6: Recreate the composite index on the new string column
            migrationBuilder.CreateIndex(
                name: "IX_FitnessPlanConfigs_Goal_Status",
                table: "FitnessPlanConfigs",
                columns: new[] { "Goal", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Step 1: Drop composite index
            migrationBuilder.DropIndex(name: "IX_FitnessPlanConfigs_Goal_Status", table: "FitnessPlanConfigs");

            // Step 2: Add a temp int column
            migrationBuilder.AddColumn<int>(
                name: "Status_Old",
                table: "FitnessPlanConfigs",
                type: "int",
                nullable: false,
                defaultValue: 1);

            // Step 3: Convert string back to int
            migrationBuilder.Sql(@"
                UPDATE [FitnessPlanConfigs]
                SET [Status_Old] = CASE [Status]
                    WHEN 'Weak'   THEN 0
                    WHEN 'Normal' THEN 1
                    WHEN 'Hard'   THEN 2
                    ELSE 1
                END
            ");

            // Step 4: Drop string column
            migrationBuilder.DropColumn(name: "Status", table: "FitnessPlanConfigs");

            // Step 5: Rename back
            migrationBuilder.RenameColumn(
                name: "Status_Old",
                table: "FitnessPlanConfigs",
                newName: "Status");

            // Step 6: Recreate the composite index on the restored int column
            migrationBuilder.CreateIndex(
                name: "IX_FitnessPlanConfigs_Goal_Status",
                table: "FitnessPlanConfigs",
                columns: new[] { "Goal", "Status" });
        }
    }
}
