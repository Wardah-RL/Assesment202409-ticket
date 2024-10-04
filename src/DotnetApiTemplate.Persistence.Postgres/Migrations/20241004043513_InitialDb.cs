﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetApiTemplate.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileRepository",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    UniqueFileName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    FileExtension = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    Source = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Note = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    FileType = table.Column<int>(type: "integer", nullable: false),
                    FileStoreAt = table.Column<int>(type: "integer", nullable: false),
                    IsFileDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    FileDeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    StatusRecord = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileRepository", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MsEvent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CountTicket = table.Column<int>(type: "integer", nullable: false),
                    Location = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    StatusRecord = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MsEvent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MsEventBroker",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CountTicket = table.Column<int>(type: "integer", nullable: false),
                    Location = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    StatusRecord = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MsEventBroker", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Option",
                columns: table => new
                {
                    Key = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    StatusRecord = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Option", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    Code = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    StatusRecord = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    NormalizedUsername = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Salt = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Password = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    LastPasswordChangeAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    StatusRecord = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrBookingTicket",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdBookingTicketBroker = table.Column<Guid>(type: "uuid", nullable: false),
                    IdEvent = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    CountTicket = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    MsEventBrokerId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    StatusRecord = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrBookingTicket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrBookingTicket_MsEventBroker_MsEventBrokerId",
                        column: x => x.MsEventBrokerId,
                        principalTable: "MsEventBroker",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrBookingTicket_MsEvent_IdEvent",
                        column: x => x.IdEvent,
                        principalTable: "MsEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleScope",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RevokedMessage = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    StatusRecord = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleScope", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleScope_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrBookingTicketBroker",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdEventBroker = table.Column<Guid>(type: "uuid", nullable: false),
                    IdUser = table.Column<Guid>(type: "uuid", nullable: false),
                    CountTicket = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    MsEventId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    StatusRecord = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrBookingTicketBroker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrBookingTicketBroker_MsEventBroker_IdEventBroker",
                        column: x => x.IdEventBroker,
                        principalTable: "MsEventBroker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrBookingTicketBroker_MsEvent_MsEventId",
                        column: x => x.MsEventId,
                        principalTable: "MsEvent",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrBookingTicketBroker_User_IdUser",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDevice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    FcmToken = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    FcmTokenExpiredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    StatusRecord = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDevice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDevice_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", maxLength: 100, nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    StatusRecord = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RefreshToken = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ExpiryAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastUpdatedByFullName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedAtServer = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    StatusRecord = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileRepository_CreatedByFullName",
                table: "FileRepository",
                column: "CreatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_FileRepository_CreatedByName",
                table: "FileRepository",
                column: "CreatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_FileRepository_Id",
                table: "FileRepository",
                column: "Id")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_FileRepository_LastUpdatedBy",
                table: "FileRepository",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FileRepository_LastUpdatedByFullName",
                table: "FileRepository",
                column: "LastUpdatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_FileRepository_LastUpdatedByName",
                table: "FileRepository",
                column: "LastUpdatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_FileRepository_UniqueFileName",
                table: "FileRepository",
                column: "UniqueFileName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MsEvent_CreatedByFullName",
                table: "MsEvent",
                column: "CreatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_MsEvent_CreatedByName",
                table: "MsEvent",
                column: "CreatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_MsEvent_Id",
                table: "MsEvent",
                column: "Id")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_MsEvent_LastUpdatedBy",
                table: "MsEvent",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MsEvent_LastUpdatedByFullName",
                table: "MsEvent",
                column: "LastUpdatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_MsEvent_LastUpdatedByName",
                table: "MsEvent",
                column: "LastUpdatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_MsEventBroker_CreatedByFullName",
                table: "MsEventBroker",
                column: "CreatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_MsEventBroker_CreatedByName",
                table: "MsEventBroker",
                column: "CreatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_MsEventBroker_Id",
                table: "MsEventBroker",
                column: "Id")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_MsEventBroker_LastUpdatedBy",
                table: "MsEventBroker",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MsEventBroker_LastUpdatedByFullName",
                table: "MsEventBroker",
                column: "LastUpdatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_MsEventBroker_LastUpdatedByName",
                table: "MsEventBroker",
                column: "LastUpdatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_Option_CreatedByFullName",
                table: "Option",
                column: "CreatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_Option_CreatedByName",
                table: "Option",
                column: "CreatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_Option_Id",
                table: "Option",
                column: "Id")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_Option_LastUpdatedBy",
                table: "Option",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Option_LastUpdatedByFullName",
                table: "Option",
                column: "LastUpdatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_Option_LastUpdatedByName",
                table: "Option",
                column: "LastUpdatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Code",
                table: "Role",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Role_CreatedByFullName",
                table: "Role",
                column: "CreatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_Role_CreatedByName",
                table: "Role",
                column: "CreatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Id",
                table: "Role",
                column: "Id")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_Role_LastUpdatedBy",
                table: "Role",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Role_LastUpdatedByFullName",
                table: "Role",
                column: "LastUpdatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_Role_LastUpdatedByName",
                table: "Role",
                column: "LastUpdatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_RoleScope_CreatedByFullName",
                table: "RoleScope",
                column: "CreatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_RoleScope_CreatedByName",
                table: "RoleScope",
                column: "CreatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_RoleScope_Id",
                table: "RoleScope",
                column: "Id")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_RoleScope_LastUpdatedBy",
                table: "RoleScope",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RoleScope_LastUpdatedByFullName",
                table: "RoleScope",
                column: "LastUpdatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_RoleScope_LastUpdatedByName",
                table: "RoleScope",
                column: "LastUpdatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_RoleScope_RoleId",
                table: "RoleScope",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_TrBookingTicket_CreatedByFullName",
                table: "TrBookingTicket",
                column: "CreatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_TrBookingTicket_CreatedByName",
                table: "TrBookingTicket",
                column: "CreatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_TrBookingTicket_Id",
                table: "TrBookingTicket",
                column: "Id")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_TrBookingTicket_IdEvent",
                table: "TrBookingTicket",
                column: "IdEvent");

            migrationBuilder.CreateIndex(
                name: "IX_TrBookingTicket_LastUpdatedBy",
                table: "TrBookingTicket",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TrBookingTicket_LastUpdatedByFullName",
                table: "TrBookingTicket",
                column: "LastUpdatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_TrBookingTicket_LastUpdatedByName",
                table: "TrBookingTicket",
                column: "LastUpdatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_TrBookingTicket_MsEventBrokerId",
                table: "TrBookingTicket",
                column: "MsEventBrokerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrBookingTicketBroker_CreatedByFullName",
                table: "TrBookingTicketBroker",
                column: "CreatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_TrBookingTicketBroker_CreatedByName",
                table: "TrBookingTicketBroker",
                column: "CreatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_TrBookingTicketBroker_Id",
                table: "TrBookingTicketBroker",
                column: "Id")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_TrBookingTicketBroker_IdEventBroker",
                table: "TrBookingTicketBroker",
                column: "IdEventBroker");

            migrationBuilder.CreateIndex(
                name: "IX_TrBookingTicketBroker_IdUser",
                table: "TrBookingTicketBroker",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_TrBookingTicketBroker_LastUpdatedBy",
                table: "TrBookingTicketBroker",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TrBookingTicketBroker_LastUpdatedByFullName",
                table: "TrBookingTicketBroker",
                column: "LastUpdatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_TrBookingTicketBroker_LastUpdatedByName",
                table: "TrBookingTicketBroker",
                column: "LastUpdatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_TrBookingTicketBroker_MsEventId",
                table: "TrBookingTicketBroker",
                column: "MsEventId");

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatedByFullName",
                table: "User",
                column: "CreatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatedByName",
                table: "User",
                column: "CreatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_User_Id",
                table: "User",
                column: "Id")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_User_LastUpdatedBy",
                table: "User",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_User_LastUpdatedByFullName",
                table: "User",
                column: "LastUpdatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_User_LastUpdatedByName",
                table: "User",
                column: "LastUpdatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_UserDevice_CreatedByFullName",
                table: "UserDevice",
                column: "CreatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_UserDevice_CreatedByName",
                table: "UserDevice",
                column: "CreatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_UserDevice_Id",
                table: "UserDevice",
                column: "Id")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_UserDevice_LastUpdatedBy",
                table: "UserDevice",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserDevice_LastUpdatedByFullName",
                table: "UserDevice",
                column: "LastUpdatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_UserDevice_LastUpdatedByName",
                table: "UserDevice",
                column: "LastUpdatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_UserDevice_UserId",
                table: "UserDevice",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_CreatedByFullName",
                table: "UserRole",
                column: "CreatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_CreatedByName",
                table: "UserRole",
                column: "CreatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_Id",
                table: "UserRole",
                column: "Id")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_LastUpdatedBy",
                table: "UserRole",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_LastUpdatedByFullName",
                table: "UserRole",
                column: "LastUpdatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_LastUpdatedByName",
                table: "UserRole",
                column: "LastUpdatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_CreatedByFullName",
                table: "UserToken",
                column: "CreatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_CreatedByName",
                table: "UserToken",
                column: "CreatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_Id",
                table: "UserToken",
                column: "Id")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_LastUpdatedBy",
                table: "UserToken",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_LastUpdatedByFullName",
                table: "UserToken",
                column: "LastUpdatedByFullName");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_LastUpdatedByName",
                table: "UserToken",
                column: "LastUpdatedByName");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_UserId",
                table: "UserToken",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileRepository");

            migrationBuilder.DropTable(
                name: "Option");

            migrationBuilder.DropTable(
                name: "RoleScope");

            migrationBuilder.DropTable(
                name: "TrBookingTicket");

            migrationBuilder.DropTable(
                name: "TrBookingTicketBroker");

            migrationBuilder.DropTable(
                name: "UserDevice");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "UserToken");

            migrationBuilder.DropTable(
                name: "MsEventBroker");

            migrationBuilder.DropTable(
                name: "MsEvent");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
