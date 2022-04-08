﻿// <auto-generated />
using System;
using FGC_OnBoarding.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FGC_OnBoarding.Migrations
{
    [DbContext(typeof(FGC_OnBoardingContext))]
    [Migration("20220406171959_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("dbo")
                .HasAnnotation("ProductVersion", "3.1.21")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FGC_OnBoarding.Areas.Identity.Data.FGC_OnBoardingUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("BuisnessName")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("FGC_OnBoarding.Models.Buisness.AuthorizedRepresentative", b =>
                {
                    b.Property<int>("RepresentativeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Address2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("BuisnessProfileId")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("County")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DOB")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Isdefault")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PositionInBuisness")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PositionInComany")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleIncharity")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RepresentativeId");

                    b.HasIndex("BuisnessProfileId");

                    b.ToTable("AuthorizedRepresentative");
                });

            modelBuilder.Entity("FGC_OnBoarding.Models.Buisness.BuisnessAttachemtns", b =>
                {
                    b.Property<int>("DocumentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BuisnessProfileId")
                        .HasColumnType("int");

                    b.Property<int>("BuisnessTypeId")
                        .HasColumnType("int");

                    b.Property<string>("DisplayFilename")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocumentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Filename")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DocumentId");

                    b.ToTable("BuisnessAttachemtns");
                });

            modelBuilder.Entity("FGC_OnBoarding.Models.Buisness.BuisnessDocuments", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BuisnessProfileId")
                        .HasColumnType("int");

                    b.Property<string>("RelationShip")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isPop")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("BuisnessDocuments");
                });

            modelBuilder.Entity("FGC_OnBoarding.Models.Buisness.BuisnessInformation", b =>
                {
                    b.Property<int>("BuisnessInformationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Answer1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Answer2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Answer3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Answer4")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Answer5")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Answer6")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Answer7")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Answer8")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Answer9")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("BuisnessProfileId")
                        .HasColumnType("int");

                    b.Property<int>("BuisnessTypeId")
                        .HasColumnType("int");

                    b.HasKey("BuisnessInformationId");

                    b.HasIndex("BuisnessProfileId");

                    b.ToTable("BuisnessInformation");
                });

            modelBuilder.Entity("FGC_OnBoarding.Models.Buisness.BuisnessProfile", b =>
                {
                    b.Property<int>("BuisnessProfileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BuisnessEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BuisnessName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("BuisnessSectorId")
                        .HasColumnType("int");

                    b.Property<int>("BuisnessTypeId")
                        .HasColumnType("int");

                    b.Property<int?>("BuisnessTypesBuisnessTypeId")
                        .HasColumnType("int");

                    b.Property<string>("BuisnessWebsite")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CharityNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("County")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CurrencyId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CurrentForm")
                        .HasColumnType("int");

                    b.Property<string>("IncorporationNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsComplete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDiscarded")
                        .HasColumnType("bit");

                    b.Property<int?>("NoOfDirectors_Partners")
                        .HasColumnType("int");

                    b.Property<int?>("NoOfTrustees")
                        .HasColumnType("int");

                    b.Property<string>("PostCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("RegisteredAdress")
                        .HasColumnType("bit");

                    b.Property<string>("RegisteredAdresss")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegisteredCity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegisteredCountry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegisteredCounty")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegisteredPostCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("SubmitDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TradeStartingDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UTR")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BuisnessProfileId");

                    b.HasIndex("BuisnessSectorId");

                    b.HasIndex("BuisnessTypesBuisnessTypeId");

                    b.ToTable("BuisnessProfile");
                });

            modelBuilder.Entity("FGC_OnBoarding.Models.Buisness.CharityDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BuisnessProfileId")
                        .HasColumnType("int");

                    b.Property<string>("RelationShip")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isPop")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("CharityDocument");
                });

            modelBuilder.Entity("FGC_OnBoarding.Models.Buisness.DirectorAndShareHolders", b =>
                {
                    b.Property<int>("DirectorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("BuisnessProfileId")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("County")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DOB")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Isdefault")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nationality")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("ShareHolders_percentage")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("DirectorId");

                    b.HasIndex("BuisnessProfileId");

                    b.ToTable("DirectorAndShareHolders");
                });

            modelBuilder.Entity("FGC_OnBoarding.Models.Buisness.FinancialInformation", b =>
                {
                    b.Property<int>("FIId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountCurrency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("AccountDetails")
                        .HasColumnType("bit");

                    b.Property<string>("AccountName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AccountNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("BuisnessProfileId")
                        .HasColumnType("int");

                    b.Property<string>("IBAN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("NoOfPaymentsPerMonth")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("PaymentIncoming")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("PaymentOutgoing")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("PerAnum")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("PerMonth")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("SortCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SwiftCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("TransactionIncoming")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("TransactionOutgoing")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("VolumePermonth")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("FIId");

                    b.HasIndex("BuisnessProfileId");

                    b.ToTable("FinancialInformation");
                });

            modelBuilder.Entity("FGC_OnBoarding.Models.Buisness.OwnerShip", b =>
                {
                    b.Property<int>("OwnerShipID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("BuisnessProfileId")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("County")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DOB")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Isdefault")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nationality")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OwnerShipID");

                    b.HasIndex("BuisnessProfileId");

                    b.ToTable("OwnerShip");
                });

            modelBuilder.Entity("FGC_OnBoarding.Models.Buisness.PersonalDocuments", b =>
                {
                    b.Property<int>("DocumentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BuisnessProfileId")
                        .HasColumnType("int");

                    b.Property<int>("BuisnessTypeId")
                        .HasColumnType("int");

                    b.Property<string>("DisplayFilename")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocumentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DocumentTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Filename")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ISpep")
                        .HasColumnType("bit");

                    b.Property<string>("RelationShip")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DocumentId");

                    b.ToTable("PersonalDocuments");
                });

            modelBuilder.Entity("FGC_OnBoarding.Models.Buisness.SoleDocuments", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BuisnessProfileId")
                        .HasColumnType("int");

                    b.Property<string>("RelationShip")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isPop")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("SoleDocuments");
                });

            modelBuilder.Entity("FGC_OnBoarding.Models.Buisness.Trustees", b =>
                {
                    b.Property<int>("TrusteeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("AppointmentDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("BuisnessProfileId")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("County")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DOB")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Isdefault")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nationality")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TrusteeId");

                    b.HasIndex("BuisnessProfileId");

                    b.ToTable("Trustees");
                });

            modelBuilder.Entity("FGC_OnBoarding.Models.ServiceRequirment.BuisnessSector", b =>
                {
                    b.Property<int>("BuisnessSectorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BuisnessSectorId");

                    b.ToTable("BuisnessSector");
                });

            modelBuilder.Entity("FGC_OnBoarding.Models.ServiceRequirment.BuisnessTypes", b =>
                {
                    b.Property<int>("BuisnessTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BuisnessTypeId");

                    b.ToTable("BuisnessTypes");
                });

            modelBuilder.Entity("FGC_OnBoarding.Models.ServiceRequirment.Currency", b =>
                {
                    b.Property<int>("CurrencyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CurrencyId");

                    b.ToTable("Currency");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("FGC_OnBoarding.Models.Buisness.AuthorizedRepresentative", b =>
                {
                    b.HasOne("FGC_OnBoarding.Models.Buisness.BuisnessProfile", "BuisnessProfile")
                        .WithMany()
                        .HasForeignKey("BuisnessProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FGC_OnBoarding.Models.Buisness.BuisnessInformation", b =>
                {
                    b.HasOne("FGC_OnBoarding.Models.Buisness.BuisnessProfile", "BuisnessProfile")
                        .WithMany()
                        .HasForeignKey("BuisnessProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FGC_OnBoarding.Models.Buisness.BuisnessProfile", b =>
                {
                    b.HasOne("FGC_OnBoarding.Models.ServiceRequirment.BuisnessSector", "BuisnessSector")
                        .WithMany()
                        .HasForeignKey("BuisnessSectorId");

                    b.HasOne("FGC_OnBoarding.Models.ServiceRequirment.BuisnessTypes", "BuisnessTypes")
                        .WithMany()
                        .HasForeignKey("BuisnessTypesBuisnessTypeId");
                });

            modelBuilder.Entity("FGC_OnBoarding.Models.Buisness.DirectorAndShareHolders", b =>
                {
                    b.HasOne("FGC_OnBoarding.Models.Buisness.BuisnessProfile", "BuisnessProfile")
                        .WithMany()
                        .HasForeignKey("BuisnessProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FGC_OnBoarding.Models.Buisness.FinancialInformation", b =>
                {
                    b.HasOne("FGC_OnBoarding.Models.Buisness.BuisnessProfile", "BuisnessProfile")
                        .WithMany()
                        .HasForeignKey("BuisnessProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FGC_OnBoarding.Models.Buisness.OwnerShip", b =>
                {
                    b.HasOne("FGC_OnBoarding.Models.Buisness.BuisnessProfile", "BuisnessProfile")
                        .WithMany()
                        .HasForeignKey("BuisnessProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FGC_OnBoarding.Models.Buisness.Trustees", b =>
                {
                    b.HasOne("FGC_OnBoarding.Models.Buisness.BuisnessProfile", "BuisnessProfile")
                        .WithMany()
                        .HasForeignKey("BuisnessProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("FGC_OnBoarding.Areas.Identity.Data.FGC_OnBoardingUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("FGC_OnBoarding.Areas.Identity.Data.FGC_OnBoardingUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FGC_OnBoarding.Areas.Identity.Data.FGC_OnBoardingUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("FGC_OnBoarding.Areas.Identity.Data.FGC_OnBoardingUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
