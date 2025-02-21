﻿// <auto-generated />
using System;
using HouseholdResponsibilityAppServer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HouseholdResponsibilityAppServer.Migrations
{
    [DbContext(typeof(HouseholdResponsibilityAppContext))]
    [Migration("20250220164552_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HouseholdResponsibilityAppServer.Models.Groups.TaskGroup", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("GroupId"));

                    b.Property<int>("HouseholdId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("GroupId");

                    b.HasIndex("HouseholdId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("HouseholdResponsibilityAppServer.Models.Histories.History", b =>
                {
                    b.Property<int>("HistoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("HistoryId"));

                    b.Property<DateTime>("CompletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CompletedById")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("HouseholdId")
                        .HasColumnType("integer");

                    b.Property<bool>("Outcome")
                        .HasColumnType("boolean");

                    b.Property<int>("ScheduledTaskId")
                        .HasColumnType("integer");

                    b.HasKey("HistoryId");

                    b.HasIndex("CompletedById");

                    b.HasIndex("HouseholdId");

                    b.HasIndex("ScheduledTaskId");

                    b.ToTable("Histories");
                });

            modelBuilder.Entity("HouseholdResponsibilityAppServer.Models.Households.Household", b =>
                {
                    b.Property<int>("HouseholdId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("HouseholdId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedByUserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("HouseholdId");

                    b.HasIndex("CreatedByUserId");

                    b.ToTable("Households");
                });

            modelBuilder.Entity("HouseholdResponsibilityAppServer.Models.Invitations.Invitation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("HouseholdId")
                        .HasColumnType("integer");

                    b.Property<string>("InvitedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsAccepted")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Invitations");
                });

            modelBuilder.Entity("HouseholdResponsibilityAppServer.Models.ScheduledTasks.ScheduledTask", b =>
                {
                    b.Property<int>("ScheduledTaskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ScheduledTaskId"));

                    b.Property<string>("AssignedToId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("AtSpecificTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("CreatedById")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("HouseholdTaskTaskId")
                        .HasColumnType("integer");

                    b.Property<string>("Repeat")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ScheduledTaskId");

                    b.HasIndex("AssignedToId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("HouseholdTaskTaskId");

                    b.ToTable("ScheduledTasks");
                });

            modelBuilder.Entity("HouseholdResponsibilityAppServer.Models.Task.HouseholdTask", b =>
                {
                    b.Property<int>("TaskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TaskId"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("CreatedById")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("GroupId")
                        .HasColumnType("integer");

                    b.Property<int>("HouseholdId")
                        .HasColumnType("integer");

                    b.Property<bool>("Priority")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("TaskId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("GroupId");

                    b.HasIndex("HouseholdId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("HouseholdResponsibilityAppServer.Models.Users.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<int?>("HouseholdId")
                        .HasColumnType("integer");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("HouseholdId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("HouseholdResponsibilityAppServer.Models.Groups.TaskGroup", b =>
                {
                    b.HasOne("HouseholdResponsibilityAppServer.Models.Households.Household", "Household")
                        .WithMany("Groups")
                        .HasForeignKey("HouseholdId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Household");
                });

            modelBuilder.Entity("HouseholdResponsibilityAppServer.Models.Histories.History", b =>
                {
                    b.HasOne("HouseholdResponsibilityAppServer.Models.Users.User", "CompletedBy")
                        .WithMany()
                        .HasForeignKey("CompletedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HouseholdResponsibilityAppServer.Models.Households.Household", "Household")
                        .WithMany("Histories")
                        .HasForeignKey("HouseholdId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HouseholdResponsibilityAppServer.Models.ScheduledTasks.ScheduledTask", "ScheduledTask")
                        .WithMany()
                        .HasForeignKey("ScheduledTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CompletedBy");

                    b.Navigation("Household");

                    b.Navigation("ScheduledTask");
                });

            modelBuilder.Entity("HouseholdResponsibilityAppServer.Models.Households.Household", b =>
                {
                    b.HasOne("HouseholdResponsibilityAppServer.Models.Users.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("CreatedByUser");
                });

            modelBuilder.Entity("HouseholdResponsibilityAppServer.Models.ScheduledTasks.ScheduledTask", b =>
                {
                    b.HasOne("HouseholdResponsibilityAppServer.Models.Users.User", "AssignedTo")
                        .WithMany()
                        .HasForeignKey("AssignedToId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("HouseholdResponsibilityAppServer.Models.Users.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HouseholdResponsibilityAppServer.Models.Task.HouseholdTask", "HouseholdTask")
                        .WithMany()
                        .HasForeignKey("HouseholdTaskTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AssignedTo");

                    b.Navigation("CreatedBy");

                    b.Navigation("HouseholdTask");
                });

            modelBuilder.Entity("HouseholdResponsibilityAppServer.Models.Task.HouseholdTask", b =>
                {
                    b.HasOne("HouseholdResponsibilityAppServer.Models.Users.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HouseholdResponsibilityAppServer.Models.Groups.TaskGroup", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HouseholdResponsibilityAppServer.Models.Households.Household", "Household")
                        .WithMany("HouseholdTasks")
                        .HasForeignKey("HouseholdId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");

                    b.Navigation("Group");

                    b.Navigation("Household");
                });

            modelBuilder.Entity("HouseholdResponsibilityAppServer.Models.Users.User", b =>
                {
                    b.HasOne("HouseholdResponsibilityAppServer.Models.Households.Household", "Household")
                        .WithMany("Users")
                        .HasForeignKey("HouseholdId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Household");
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
                    b.HasOne("HouseholdResponsibilityAppServer.Models.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("HouseholdResponsibilityAppServer.Models.Users.User", null)
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

                    b.HasOne("HouseholdResponsibilityAppServer.Models.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("HouseholdResponsibilityAppServer.Models.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HouseholdResponsibilityAppServer.Models.Households.Household", b =>
                {
                    b.Navigation("Groups");

                    b.Navigation("Histories");

                    b.Navigation("HouseholdTasks");

                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
