﻿// <auto-generated />
using System;
using Casino.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Casino.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Casino.Data.Models.Entities.Roulette", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<long>("StateId")
                        .HasColumnType("bigint");

                    b.Property<long>("TypeId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<long>("UserRegisterId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("StateId");

                    b.HasIndex("TypeId");

                    b.HasIndex("UserRegisterId");

                    b.ToTable("Roulettes");
                });

            modelBuilder.Entity("Casino.Data.Models.Entities.RouletteState", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<DateTime?>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("RouletteStates");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CreatedAt = new DateTime(2020, 4, 3, 1, 54, 4, 288, DateTimeKind.Local).AddTicks(6962),
                            State = "Active",
                            UpdatedAt = new DateTime(2020, 4, 3, 1, 54, 4, 289, DateTimeKind.Local).AddTicks(5025)
                        },
                        new
                        {
                            Id = 2L,
                            CreatedAt = new DateTime(2020, 4, 3, 1, 54, 4, 289, DateTimeKind.Local).AddTicks(5570),
                            State = "Inactive",
                            UpdatedAt = new DateTime(2020, 4, 3, 1, 54, 4, 289, DateTimeKind.Local).AddTicks(5580)
                        },
                        new
                        {
                            Id = 3L,
                            CreatedAt = new DateTime(2020, 4, 3, 1, 54, 4, 289, DateTimeKind.Local).AddTicks(5587),
                            State = "Suspended",
                            UpdatedAt = new DateTime(2020, 4, 3, 1, 54, 4, 289, DateTimeKind.Local).AddTicks(5588)
                        });
                });

            modelBuilder.Entity("Casino.Data.Models.Entities.RouletteType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<DateTime?>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("RouletteTypes");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CreatedAt = new DateTime(2020, 4, 3, 1, 54, 4, 290, DateTimeKind.Local).AddTicks(9183),
                            Type = "American",
                            UpdatedAt = new DateTime(2020, 4, 3, 1, 54, 4, 290, DateTimeKind.Local).AddTicks(9988)
                        },
                        new
                        {
                            Id = 2L,
                            CreatedAt = new DateTime(2020, 4, 3, 1, 54, 4, 291, DateTimeKind.Local).AddTicks(458),
                            Type = "European",
                            UpdatedAt = new DateTime(2020, 4, 3, 1, 54, 4, 291, DateTimeKind.Local).AddTicks(472)
                        });
                });

            modelBuilder.Entity("Casino.Data.Models.Entities.Round", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CloseAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("RouletteId")
                        .HasColumnType("bigint");

                    b.Property<long>("StateId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<string>("WinNumber")
                        .HasColumnType("nvarchar(2)")
                        .HasMaxLength(2);

                    b.HasKey("Id");

                    b.HasIndex("RouletteId");

                    b.HasIndex("StateId");

                    b.ToTable("Rounds");
                });

            modelBuilder.Entity("Casino.Data.Models.Entities.RoundState", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<DateTime?>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("RoundStates");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CreatedAt = new DateTime(2020, 4, 3, 1, 54, 4, 291, DateTimeKind.Local).AddTicks(4643),
                            State = "Opened",
                            UpdatedAt = new DateTime(2020, 4, 3, 1, 54, 4, 291, DateTimeKind.Local).AddTicks(5190)
                        },
                        new
                        {
                            Id = 2L,
                            CreatedAt = new DateTime(2020, 4, 3, 1, 54, 4, 291, DateTimeKind.Local).AddTicks(5665),
                            State = "Closed",
                            UpdatedAt = new DateTime(2020, 4, 3, 1, 54, 4, 291, DateTimeKind.Local).AddTicks(5677)
                        });
                });

            modelBuilder.Entity("Casino.Data.Models.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CloudIdentityId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Casino.Data.Models.Entities.Roulette", b =>
                {
                    b.HasOne("Casino.Data.Models.Entities.RouletteState", "State")
                        .WithMany()
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Casino.Data.Models.Entities.RouletteType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Casino.Data.Models.Entities.User", "UserRegister")
                        .WithMany()
                        .HasForeignKey("UserRegisterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Casino.Data.Models.Entities.Round", b =>
                {
                    b.HasOne("Casino.Data.Models.Entities.Roulette", "Roulette")
                        .WithMany()
                        .HasForeignKey("RouletteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Casino.Data.Models.Entities.RoundState", "State")
                        .WithMany()
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
