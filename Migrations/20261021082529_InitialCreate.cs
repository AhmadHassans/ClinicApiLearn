using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ClinicApiLearn.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PatientName = table.Column<string>(type: "text", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Prescription = table.Column<string>(type: "text", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TokenHash = table.Column<string>(type: "text", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Code", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("44314cba-01a9-432a-abca-2dbb0fa300a0"), "Appointment.Delete", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Delete Appointment", null },
                    { new Guid("45e38124-5d0b-4e76-9bc1-f4d5c412789c"), "User.Manage", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manage Users", null },
                    { new Guid("6de2406a-df86-4402-a0e9-89901febf595"), "Prescription.Write", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Write Prescription", null },
                    { new Guid("8af052af-0483-4c7b-9686-ec75abc36d8c"), "Appointment.Read", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "View Appointments", null },
                    { new Guid("cbe4c8d7-7a27-45aa-a2de-304dc0555375"), "Appointment.Create", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Create Appointment", null },
                    { new Guid("f76bc9aa-b82c-47b9-8d24-268681967427"), "Appointment.Update", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Update Appointment", null }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("24373e69-d0cd-4806-a02c-3f03272963c1"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Doctor", null },
                    { new Guid("2b876836-9b5a-4658-90f9-e8fe258ac3aa"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Receptionist", null },
                    { new Guid("4443e5b0-6cad-4e14-9448-f7c0836df9bd"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SuperAdmin", null },
                    { new Guid("b70eaa30-8918-45b3-a7c3-8734fb222056"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Admin", null }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("6de2406a-df86-4402-a0e9-89901febf595"), new Guid("24373e69-d0cd-4806-a02c-3f03272963c1") },
                    { new Guid("8af052af-0483-4c7b-9686-ec75abc36d8c"), new Guid("24373e69-d0cd-4806-a02c-3f03272963c1") },
                    { new Guid("8af052af-0483-4c7b-9686-ec75abc36d8c"), new Guid("2b876836-9b5a-4658-90f9-e8fe258ac3aa") },
                    { new Guid("cbe4c8d7-7a27-45aa-a2de-304dc0555375"), new Guid("2b876836-9b5a-4658-90f9-e8fe258ac3aa") },
                    { new Guid("f76bc9aa-b82c-47b9-8d24-268681967427"), new Guid("2b876836-9b5a-4658-90f9-e8fe258ac3aa") },
                    { new Guid("44314cba-01a9-432a-abca-2dbb0fa300a0"), new Guid("4443e5b0-6cad-4e14-9448-f7c0836df9bd") },
                    { new Guid("45e38124-5d0b-4e76-9bc1-f4d5c412789c"), new Guid("4443e5b0-6cad-4e14-9448-f7c0836df9bd") },
                    { new Guid("6de2406a-df86-4402-a0e9-89901febf595"), new Guid("4443e5b0-6cad-4e14-9448-f7c0836df9bd") },
                    { new Guid("8af052af-0483-4c7b-9686-ec75abc36d8c"), new Guid("4443e5b0-6cad-4e14-9448-f7c0836df9bd") },
                    { new Guid("cbe4c8d7-7a27-45aa-a2de-304dc0555375"), new Guid("4443e5b0-6cad-4e14-9448-f7c0836df9bd") },
                    { new Guid("f76bc9aa-b82c-47b9-8d24-268681967427"), new Guid("4443e5b0-6cad-4e14-9448-f7c0836df9bd") },
                    { new Guid("44314cba-01a9-432a-abca-2dbb0fa300a0"), new Guid("b70eaa30-8918-45b3-a7c3-8734fb222056") },
                    { new Guid("45e38124-5d0b-4e76-9bc1-f4d5c412789c"), new Guid("b70eaa30-8918-45b3-a7c3-8734fb222056") },
                    { new Guid("6de2406a-df86-4402-a0e9-89901febf595"), new Guid("b70eaa30-8918-45b3-a7c3-8734fb222056") },
                    { new Guid("8af052af-0483-4c7b-9686-ec75abc36d8c"), new Guid("b70eaa30-8918-45b3-a7c3-8734fb222056") },
                    { new Guid("cbe4c8d7-7a27-45aa-a2de-304dc0555375"), new Guid("b70eaa30-8918-45b3-a7c3-8734fb222056") },
                    { new Guid("f76bc9aa-b82c-47b9-8d24-268681967427"), new Guid("b70eaa30-8918-45b3-a7c3-8734fb222056") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CreatedByUserId",
                table: "Appointments",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Code",
                table: "Permissions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
