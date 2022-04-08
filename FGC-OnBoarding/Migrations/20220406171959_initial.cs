using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FGC_OnBoarding.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    BuisnessName = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BuisnessAttachemtns",
                schema: "dbo",
                columns: table => new
                {
                    DocumentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Filename = table.Column<string>(nullable: true),
                    DisplayFilename = table.Column<string>(nullable: true),
                    BuisnessProfileId = table.Column<int>(nullable: false),
                    BuisnessTypeId = table.Column<int>(nullable: false),
                    DocumentType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuisnessAttachemtns", x => x.DocumentId);
                });

            migrationBuilder.CreateTable(
                name: "BuisnessDocuments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isPop = table.Column<bool>(nullable: false),
                    RelationShip = table.Column<string>(nullable: true),
                    BuisnessProfileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuisnessDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BuisnessSector",
                schema: "dbo",
                columns: table => new
                {
                    BuisnessSectorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuisnessSector", x => x.BuisnessSectorId);
                });

            migrationBuilder.CreateTable(
                name: "BuisnessTypes",
                schema: "dbo",
                columns: table => new
                {
                    BuisnessTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuisnessTypes", x => x.BuisnessTypeId);
                });

            migrationBuilder.CreateTable(
                name: "CharityDocument",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isPop = table.Column<bool>(nullable: false),
                    RelationShip = table.Column<string>(nullable: true),
                    BuisnessProfileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharityDocument", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                schema: "dbo",
                columns: table => new
                {
                    CurrencyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.CurrencyId);
                });

            migrationBuilder.CreateTable(
                name: "PersonalDocuments",
                schema: "dbo",
                columns: table => new
                {
                    DocumentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Filename = table.Column<string>(nullable: true),
                    DisplayFilename = table.Column<string>(nullable: true),
                    BuisnessProfileId = table.Column<int>(nullable: false),
                    BuisnessTypeId = table.Column<int>(nullable: false),
                    DocumentType = table.Column<string>(nullable: true),
                    ISpep = table.Column<bool>(nullable: false),
                    RelationShip = table.Column<string>(nullable: true),
                    DocumentTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalDocuments", x => x.DocumentId);
                });

            migrationBuilder.CreateTable(
                name: "SoleDocuments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isPop = table.Column<bool>(nullable: false),
                    RelationShip = table.Column<string>(nullable: true),
                    BuisnessProfileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoleDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                schema: "dbo",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BuisnessProfile",
                schema: "dbo",
                columns: table => new
                {
                    BuisnessProfileId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuisnessName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    County = table.Column<string>(nullable: true),
                    PostCode = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    BuisnessWebsite = table.Column<string>(nullable: true),
                    BuisnessEmail = table.Column<string>(nullable: true),
                    UTR = table.Column<string>(nullable: true),
                    CharityNumber = table.Column<string>(nullable: true),
                    IncorporationNumber = table.Column<string>(nullable: true),
                    NoOfDirectors_Partners = table.Column<int>(nullable: true),
                    NoOfTrustees = table.Column<int>(nullable: true),
                    RegistrationDate = table.Column<DateTime>(nullable: false),
                    TradeStartingDate = table.Column<DateTime>(nullable: false),
                    RegisteredAdress = table.Column<bool>(nullable: false),
                    RegisteredAdresss = table.Column<string>(nullable: true),
                    RegisteredCity = table.Column<string>(nullable: true),
                    RegisteredCounty = table.Column<string>(nullable: true),
                    RegisteredPostCode = table.Column<string>(nullable: true),
                    RegisteredCountry = table.Column<string>(nullable: true),
                    CurrencyId = table.Column<string>(nullable: true),
                    BuisnessTypesBuisnessTypeId = table.Column<int>(nullable: true),
                    BuisnessTypeId = table.Column<int>(nullable: false),
                    BuisnessSectorId = table.Column<int>(nullable: true),
                    IsComplete = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    SubmitDate = table.Column<DateTime>(nullable: false),
                    IsDiscarded = table.Column<bool>(nullable: false),
                    CurrentForm = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuisnessProfile", x => x.BuisnessProfileId);
                    table.ForeignKey(
                        name: "FK_BuisnessProfile_BuisnessSector_BuisnessSectorId",
                        column: x => x.BuisnessSectorId,
                        principalSchema: "dbo",
                        principalTable: "BuisnessSector",
                        principalColumn: "BuisnessSectorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BuisnessProfile_BuisnessTypes_BuisnessTypesBuisnessTypeId",
                        column: x => x.BuisnessTypesBuisnessTypeId,
                        principalSchema: "dbo",
                        principalTable: "BuisnessTypes",
                        principalColumn: "BuisnessTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuthorizedRepresentative",
                schema: "dbo",
                columns: table => new
                {
                    RepresentativeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Address1 = table.Column<string>(nullable: true),
                    Address2 = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    PostCode = table.Column<string>(nullable: true),
                    County = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    DOB = table.Column<DateTime>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PositionInBuisness = table.Column<string>(nullable: true),
                    RoleIncharity = table.Column<string>(nullable: true),
                    PositionInComany = table.Column<string>(nullable: true),
                    BuisnessProfileId = table.Column<int>(nullable: false),
                    Isdefault = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizedRepresentative", x => x.RepresentativeId);
                    table.ForeignKey(
                        name: "FK_AuthorizedRepresentative_BuisnessProfile_BuisnessProfileId",
                        column: x => x.BuisnessProfileId,
                        principalSchema: "dbo",
                        principalTable: "BuisnessProfile",
                        principalColumn: "BuisnessProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BuisnessInformation",
                schema: "dbo",
                columns: table => new
                {
                    BuisnessInformationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Answer1 = table.Column<string>(nullable: true),
                    Answer2 = table.Column<string>(nullable: true),
                    Answer3 = table.Column<string>(nullable: true),
                    Answer4 = table.Column<string>(nullable: true),
                    Answer5 = table.Column<string>(nullable: true),
                    Answer6 = table.Column<string>(nullable: true),
                    Answer7 = table.Column<string>(nullable: true),
                    Answer8 = table.Column<string>(nullable: true),
                    Answer9 = table.Column<string>(nullable: true),
                    BuisnessProfileId = table.Column<int>(nullable: false),
                    BuisnessTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuisnessInformation", x => x.BuisnessInformationId);
                    table.ForeignKey(
                        name: "FK_BuisnessInformation_BuisnessProfile_BuisnessProfileId",
                        column: x => x.BuisnessProfileId,
                        principalSchema: "dbo",
                        principalTable: "BuisnessProfile",
                        principalColumn: "BuisnessProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DirectorAndShareHolders",
                schema: "dbo",
                columns: table => new
                {
                    DirectorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    PostCode = table.Column<string>(nullable: true),
                    County = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Nationality = table.Column<string>(nullable: true),
                    DOB = table.Column<DateTime>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    ShareHolders_percentage = table.Column<decimal>(nullable: true),
                    BuisnessProfileId = table.Column<int>(nullable: false),
                    Isdefault = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectorAndShareHolders", x => x.DirectorId);
                    table.ForeignKey(
                        name: "FK_DirectorAndShareHolders_BuisnessProfile_BuisnessProfileId",
                        column: x => x.BuisnessProfileId,
                        principalSchema: "dbo",
                        principalTable: "BuisnessProfile",
                        principalColumn: "BuisnessProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinancialInformation",
                schema: "dbo",
                columns: table => new
                {
                    FIId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PerMonth = table.Column<decimal>(nullable: true),
                    PerAnum = table.Column<decimal>(nullable: true),
                    PaymentIncoming = table.Column<decimal>(nullable: true),
                    PaymentOutgoing = table.Column<decimal>(nullable: true),
                    TransactionIncoming = table.Column<decimal>(nullable: true),
                    TransactionOutgoing = table.Column<decimal>(nullable: true),
                    NoOfPaymentsPerMonth = table.Column<decimal>(nullable: true),
                    VolumePermonth = table.Column<decimal>(nullable: true),
                    AccountDetails = table.Column<bool>(nullable: false),
                    BankName = table.Column<string>(nullable: true),
                    BankAddress = table.Column<string>(nullable: true),
                    SortCode = table.Column<string>(nullable: true),
                    AccountName = table.Column<string>(nullable: true),
                    AccountNumber = table.Column<string>(nullable: true),
                    IBAN = table.Column<string>(nullable: true),
                    SwiftCode = table.Column<string>(nullable: true),
                    AccountCurrency = table.Column<string>(nullable: true),
                    BuisnessProfileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialInformation", x => x.FIId);
                    table.ForeignKey(
                        name: "FK_FinancialInformation_BuisnessProfile_BuisnessProfileId",
                        column: x => x.BuisnessProfileId,
                        principalSchema: "dbo",
                        principalTable: "BuisnessProfile",
                        principalColumn: "BuisnessProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OwnerShip",
                schema: "dbo",
                columns: table => new
                {
                    OwnerShipID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    PostCode = table.Column<string>(nullable: true),
                    County = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Nationality = table.Column<string>(nullable: true),
                    DOB = table.Column<DateTime>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    BuisnessProfileId = table.Column<int>(nullable: false),
                    Isdefault = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerShip", x => x.OwnerShipID);
                    table.ForeignKey(
                        name: "FK_OwnerShip_BuisnessProfile_BuisnessProfileId",
                        column: x => x.BuisnessProfileId,
                        principalSchema: "dbo",
                        principalTable: "BuisnessProfile",
                        principalColumn: "BuisnessProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trustees",
                schema: "dbo",
                columns: table => new
                {
                    TrusteeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    PostCode = table.Column<string>(nullable: true),
                    County = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Nationality = table.Column<string>(nullable: true),
                    DOB = table.Column<DateTime>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    AppointmentDate = table.Column<DateTime>(nullable: false),
                    BuisnessProfileId = table.Column<int>(nullable: false),
                    Isdefault = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trustees", x => x.TrusteeId);
                    table.ForeignKey(
                        name: "FK_Trustees_BuisnessProfile_BuisnessProfileId",
                        column: x => x.BuisnessProfileId,
                        principalSchema: "dbo",
                        principalTable: "BuisnessProfile",
                        principalColumn: "BuisnessProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "dbo",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "dbo",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "dbo",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "dbo",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "dbo",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "dbo",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "dbo",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizedRepresentative_BuisnessProfileId",
                schema: "dbo",
                table: "AuthorizedRepresentative",
                column: "BuisnessProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_BuisnessInformation_BuisnessProfileId",
                schema: "dbo",
                table: "BuisnessInformation",
                column: "BuisnessProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_BuisnessProfile_BuisnessSectorId",
                schema: "dbo",
                table: "BuisnessProfile",
                column: "BuisnessSectorId");

            migrationBuilder.CreateIndex(
                name: "IX_BuisnessProfile_BuisnessTypesBuisnessTypeId",
                schema: "dbo",
                table: "BuisnessProfile",
                column: "BuisnessTypesBuisnessTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectorAndShareHolders_BuisnessProfileId",
                schema: "dbo",
                table: "DirectorAndShareHolders",
                column: "BuisnessProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialInformation_BuisnessProfileId",
                schema: "dbo",
                table: "FinancialInformation",
                column: "BuisnessProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerShip_BuisnessProfileId",
                schema: "dbo",
                table: "OwnerShip",
                column: "BuisnessProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Trustees_BuisnessProfileId",
                schema: "dbo",
                table: "Trustees",
                column: "BuisnessProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AuthorizedRepresentative",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BuisnessAttachemtns",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BuisnessDocuments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BuisnessInformation",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CharityDocument",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Currency",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DirectorAndShareHolders",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FinancialInformation",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "OwnerShip",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PersonalDocuments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SoleDocuments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Trustees",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUsers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BuisnessProfile",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BuisnessSector",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BuisnessTypes",
                schema: "dbo");
        }
    }
}
