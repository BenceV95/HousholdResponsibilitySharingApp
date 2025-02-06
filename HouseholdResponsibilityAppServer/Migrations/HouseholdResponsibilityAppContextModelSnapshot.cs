﻿// <auto-generated />
using System;
using HouseholdResponsibilityAppServer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HouseholdResponsibilityAppServer.Migrations
{
    [DbContext(typeof(HouseholdResponsibilityAppContext))]
    partial class HouseholdResponsibilityAppContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<int>("CompletedById")
                        .HasColumnType("integer");

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

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("HouseholdId");

                    b.HasIndex("CreatedBy");

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

                    b.Property<int>("AssignedToId")
                        .HasColumnType("integer");

                    b.Property<bool>("AtSpecificTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<int>("CreatedById")
                        .HasColumnType("integer");

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("HouseholdTaskId")
                        .HasColumnType("integer");

                    b.Property<string>("Repeat")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ScheduledTaskId");

                    b.HasIndex("AssignedToId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("HouseholdTaskId");

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

                    b.Property<int>("CreatedById")
                        .HasColumnType("integer");

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
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("HouseholdId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("HouseholdId");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
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
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.SetNull);

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
                        .HasForeignKey("HouseholdTaskId")
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
