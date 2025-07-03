using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReportHub.Infrastructure.Migrations
{
	/// <inheritdoc />
	public partial class AddPlanAndPlanItem : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "plans",
				columns: table => new
				{
					id = table.Column<Guid>(type: "uuid", nullable: false),
					title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
					start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					currency_code = table.Column<string>(type: "text", nullable: true),
					organization_id = table.Column<Guid>(type: "uuid", nullable: false),
					created_by = table.Column<Guid>(type: "uuid", nullable: false),
					created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					updated_by = table.Column<Guid>(type: "uuid", nullable: true),
					updated_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					is_deleted = table.Column<bool>(type: "boolean", nullable: false),
					deleted_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
				},
				constraints: table =>
				{
					table.PrimaryKey("pk_plans", x => x.id);
					table.ForeignKey(
						name: "fk_plans_organizations_organization_id",
						column: x => x.organization_id,
						principalTable: "organizations",
						principalColumn: "id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "plan_items",
				columns: table => new
				{
					id = table.Column<Guid>(type: "uuid", nullable: false),
					plan_id = table.Column<Guid>(type: "uuid", nullable: false),
					item_id = table.Column<Guid>(type: "uuid", nullable: false),
					items_count = table.Column<int>(type: "integer", nullable: false),
					price = table.Column<decimal>(type: "numeric", nullable: false),
					currency_code = table.Column<string>(type: "text", nullable: true),
					organization_id = table.Column<Guid>(type: "uuid", nullable: false),
					created_by = table.Column<Guid>(type: "uuid", nullable: false),
					created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					updated_by = table.Column<Guid>(type: "uuid", nullable: true),
					updated_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					is_deleted = table.Column<bool>(type: "boolean", nullable: false),
					deleted_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
				},
				constraints: table =>
				{
					table.PrimaryKey("pk_plan_items", x => x.id);
					table.ForeignKey(
						name: "fk_plan_items_items_item_id",
						column: x => x.item_id,
						principalTable: "items",
						principalColumn: "id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "fk_plan_items_organizations_organization_id",
						column: x => x.organization_id,
						principalTable: "organizations",
						principalColumn: "id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "fk_plan_items_plans_plan_id",
						column: x => x.plan_id,
						principalTable: "plans",
						principalColumn: "id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "ix_plan_items_item_id",
				table: "plan_items",
				column: "item_id");

			migrationBuilder.CreateIndex(
				name: "ix_plan_items_organization_id",
				table: "plan_items",
				column: "organization_id");

			migrationBuilder.CreateIndex(
				name: "ix_plan_items_plan_id",
				table: "plan_items",
				column: "plan_id");

			migrationBuilder.CreateIndex(
				name: "ix_plans_organization_id",
				table: "plans",
				column: "organization_id");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "plan_items");

			migrationBuilder.DropTable(
				name: "plans");
		}
	}
}
