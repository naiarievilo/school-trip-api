using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolTripApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Domain_Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_PostalCode",
                table: "Guardians");

            migrationBuilder.RenameColumn(
                name: "EmergencyContact_PhoneNumber",
                table: "Guardians",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "EmergencyContact_Name",
                table: "Guardians",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Address_StreetNumber",
                table: "Guardians",
                newName: "StreetNumber");

            migrationBuilder.RenameColumn(
                name: "Address_Street",
                table: "Guardians",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "Address_State",
                table: "Guardians",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "Address_Neighborhood",
                table: "Guardians",
                newName: "Neighborhood");

            migrationBuilder.RenameColumn(
                name: "Address_Country",
                table: "Guardians",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "Address_City",
                table: "Guardians",
                newName: "City");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiresAt",
                table: "RefreshTokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 10, 12, 19, 4, 51, 500, DateTimeKind.Utc).AddTicks(8353),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 8, 8, 15, 51, 55, 652, DateTimeKind.Utc).AddTicks(5438));

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Guardians",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                table: "Guardians",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(14)",
                oldMaxLength: 14,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Guardians",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(21)",
                oldMaxLength: 21,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Guardians",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StreetNumber",
                table: "Guardians",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(16)",
                oldMaxLength: 16,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Street",
                table: "Guardians",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "Guardians",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Neighborhood",
                table: "Guardians",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Guardians",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Guardians",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cep",
                table: "Guardians",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AgreementTemplate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    RichTextContent = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgreementTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GradeLevel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    GradeLevelCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeLevel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GuardianId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<int>(type: "integer", nullable: false),
                    PaymentMethod = table.Column<int>(type: "integer", nullable: false),
                    PaymentStatus = table.Column<int>(type: "integer", nullable: false),
                    TransactionId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payment_Guardians_GuardianId",
                        column: x => x.GuardianId,
                        principalTable: "Guardians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "School",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Cnpj = table.Column<string>(type: "text", nullable: false),
                    Street = table.Column<string>(type: "text", nullable: false),
                    StreetNumber = table.Column<string>(type: "text", nullable: false),
                    Neighborhood = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    Cep = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_School", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GradeLevelSchool",
                columns: table => new
                {
                    GradesId = table.Column<int>(type: "integer", nullable: false),
                    SchoolsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeLevelSchool", x => new { x.GradesId, x.SchoolsId });
                    table.ForeignKey(
                        name: "FK_GradeLevelSchool_GradeLevel_GradesId",
                        column: x => x.GradesId,
                        principalTable: "GradeLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeLevelSchool_School_SchoolsId",
                        column: x => x.SchoolsId,
                        principalTable: "School",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuardianSchool",
                columns: table => new
                {
                    GuardiansId = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuardianSchool", x => new { x.GuardiansId, x.SchoolsId });
                    table.ForeignKey(
                        name: "FK_GuardianSchool_Guardians_GuardiansId",
                        column: x => x.GuardiansId,
                        principalTable: "Guardians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuardianSchool_School_SchoolsId",
                        column: x => x.SchoolsId,
                        principalTable: "School",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolTrip",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    AgreementTemplateId = table.Column<int>(type: "integer", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: false),
                    MinimumRequired = table.Column<int>(type: "integer", nullable: false),
                    MaximumAllowed = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<int>(type: "integer", nullable: false),
                    SaleStartsAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    SaleEndsAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Street = table.Column<string>(type: "text", nullable: false),
                    StreetNumber = table.Column<string>(type: "text", nullable: false),
                    Neighborhood = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    Cep = table.Column<string>(type: "text", nullable: false),
                    DepartureAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ReturnAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolTrip", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolTrip_AgreementTemplate_AgreementTemplateId",
                        column: x => x.AgreementTemplateId,
                        principalTable: "AgreementTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolTrip_School_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "School",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GuardianId = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    GradeLevelId = table.Column<int>(type: "integer", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Cpf = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    GradeClass = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Student_GradeLevel_GradeLevelId",
                        column: x => x.GradeLevelId,
                        principalTable: "GradeLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Student_Guardians_GuardianId",
                        column: x => x.GuardianId,
                        principalTable: "Guardians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Student_School_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "School",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Agreement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolTripId = table.Column<int>(type: "integer", nullable: false),
                    AgreementTemplateId = table.Column<int>(type: "integer", nullable: false),
                    GuardianId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsSigned = table.Column<bool>(type: "boolean", nullable: false),
                    SignedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agreement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agreement_AgreementTemplate_AgreementTemplateId",
                        column: x => x.AgreementTemplateId,
                        principalTable: "AgreementTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Agreement_Guardians_GuardianId",
                        column: x => x.GuardianId,
                        principalTable: "Guardians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Agreement_SchoolTrip_SchoolTripId",
                        column: x => x.SchoolTripId,
                        principalTable: "SchoolTrip",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GradeLevelSchoolTrip",
                columns: table => new
                {
                    GradesId = table.Column<int>(type: "integer", nullable: false),
                    TripsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeLevelSchoolTrip", x => new { x.GradesId, x.TripsId });
                    table.ForeignKey(
                        name: "FK_GradeLevelSchoolTrip_GradeLevel_GradesId",
                        column: x => x.GradesId,
                        principalTable: "GradeLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeLevelSchoolTrip_SchoolTrip_TripsId",
                        column: x => x.TripsId,
                        principalTable: "SchoolTrip",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    GuardianId = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolTripId = table.Column<int>(type: "integer", nullable: false),
                    TripRating = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rating_Guardians_GuardianId",
                        column: x => x.GuardianId,
                        principalTable: "Guardians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rating_SchoolTrip_SchoolTripId",
                        column: x => x.SchoolTripId,
                        principalTable: "SchoolTrip",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enrollment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolTripId = table.Column<int>(type: "integer", nullable: false),
                    GuardianId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enrollment_Guardians_GuardianId",
                        column: x => x.GuardianId,
                        principalTable: "Guardians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollment_Payment_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enrollment_SchoolTrip_SchoolTripId",
                        column: x => x.SchoolTripId,
                        principalTable: "SchoolTrip",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollment_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Guardians_Cpf",
                table: "Guardians",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agreement_AgreementTemplateId",
                table: "Agreement",
                column: "AgreementTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Agreement_GuardianId",
                table: "Agreement",
                column: "GuardianId");

            migrationBuilder.CreateIndex(
                name: "IX_Agreement_SchoolTripId",
                table: "Agreement",
                column: "SchoolTripId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_GuardianId",
                table: "Enrollment",
                column: "GuardianId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_PaymentId",
                table: "Enrollment",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_SchoolTripId",
                table: "Enrollment",
                column: "SchoolTripId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_StudentId",
                table: "Enrollment",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeLevelSchool_SchoolsId",
                table: "GradeLevelSchool",
                column: "SchoolsId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeLevelSchoolTrip_TripsId",
                table: "GradeLevelSchoolTrip",
                column: "TripsId");

            migrationBuilder.CreateIndex(
                name: "IX_GuardianSchool_SchoolsId",
                table: "GuardianSchool",
                column: "SchoolsId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_GuardianId",
                table: "Payment",
                column: "GuardianId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_GuardianId",
                table: "Rating",
                column: "GuardianId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_SchoolTripId",
                table: "Rating",
                column: "SchoolTripId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTrip_AgreementTemplateId",
                table: "SchoolTrip",
                column: "AgreementTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTrip_SchoolId",
                table: "SchoolTrip",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Student_Cpf",
                table: "Student",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_GradeLevelId",
                table: "Student",
                column: "GradeLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Student_GuardianId",
                table: "Student",
                column: "GuardianId");

            migrationBuilder.CreateIndex(
                name: "IX_Student_SchoolId",
                table: "Student",
                column: "SchoolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Agreement");

            migrationBuilder.DropTable(
                name: "Enrollment");

            migrationBuilder.DropTable(
                name: "GradeLevelSchool");

            migrationBuilder.DropTable(
                name: "GradeLevelSchoolTrip");

            migrationBuilder.DropTable(
                name: "GuardianSchool");

            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "SchoolTrip");

            migrationBuilder.DropTable(
                name: "GradeLevel");

            migrationBuilder.DropTable(
                name: "AgreementTemplate");

            migrationBuilder.DropTable(
                name: "School");

            migrationBuilder.DropIndex(
                name: "IX_Guardians_Cpf",
                table: "Guardians");

            migrationBuilder.DropColumn(
                name: "Cep",
                table: "Guardians");

            migrationBuilder.RenameColumn(
                name: "StreetNumber",
                table: "Guardians",
                newName: "Address_StreetNumber");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Guardians",
                newName: "Address_Street");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Guardians",
                newName: "Address_State");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Guardians",
                newName: "EmergencyContact_PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "Neighborhood",
                table: "Guardians",
                newName: "Address_Neighborhood");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Guardians",
                newName: "EmergencyContact_Name");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Guardians",
                newName: "Address_Country");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Guardians",
                newName: "Address_City");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiresAt",
                table: "RefreshTokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 8, 8, 15, 51, 55, 652, DateTimeKind.Utc).AddTicks(5438),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 10, 12, 19, 4, 51, 500, DateTimeKind.Utc).AddTicks(8353));

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Guardians",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                table: "Guardians",
                type: "character varying(14)",
                maxLength: 14,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address_StreetNumber",
                table: "Guardians",
                type: "character varying(16)",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address_Street",
                table: "Guardians",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address_State",
                table: "Guardians",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmergencyContact_PhoneNumber",
                table: "Guardians",
                type: "character varying(21)",
                maxLength: 21,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address_Neighborhood",
                table: "Guardians",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmergencyContact_Name",
                table: "Guardians",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address_Country",
                table: "Guardians",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address_City",
                table: "Guardians",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_PostalCode",
                table: "Guardians",
                type: "character varying(9)",
                maxLength: 9,
                nullable: true);
        }
    }
}
