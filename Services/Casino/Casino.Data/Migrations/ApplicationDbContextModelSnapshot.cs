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

            modelBuilder.Entity("Casino.Data.Models.Entities.AccountTransaction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("StateId")
                        .HasColumnType("bigint");

                    b.Property<long>("TypeId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<long>("UserAccountId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserRegisterId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("StateId");

                    b.HasIndex("TypeId");

                    b.HasIndex("UserAccountId");

                    b.HasIndex("UserRegisterId");

                    b.ToTable("AccountTransactions");
                });

            modelBuilder.Entity("Casino.Data.Models.Entities.AccountTransactionState", b =>
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

                    b.ToTable("AccountTransactionStates");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CreatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 992, DateTimeKind.Local).AddTicks(5551),
                            State = "Approved",
                            UpdatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 992, DateTimeKind.Local).AddTicks(6067)
                        },
                        new
                        {
                            Id = 2L,
                            CreatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 992, DateTimeKind.Local).AddTicks(6536),
                            State = "Pending",
                            UpdatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 992, DateTimeKind.Local).AddTicks(6547)
                        },
                        new
                        {
                            Id = 3L,
                            CreatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 992, DateTimeKind.Local).AddTicks(6555),
                            State = "Cancelled",
                            UpdatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 992, DateTimeKind.Local).AddTicks(6556)
                        },
                        new
                        {
                            Id = 4L,
                            CreatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 992, DateTimeKind.Local).AddTicks(6557),
                            State = "Rejected",
                            UpdatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 992, DateTimeKind.Local).AddTicks(6558)
                        });
                });

            modelBuilder.Entity("Casino.Data.Models.Entities.AccountTransactionType", b =>
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

                    b.ToTable("AccountTransactionTypes");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CreatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 993, DateTimeKind.Local).AddTicks(1773),
                            Type = "Deposit",
                            UpdatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 993, DateTimeKind.Local).AddTicks(2244)
                        },
                        new
                        {
                            Id = 2L,
                            CreatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 993, DateTimeKind.Local).AddTicks(2706),
                            Type = "Withdrawal",
                            UpdatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 993, DateTimeKind.Local).AddTicks(2718)
                        },
                        new
                        {
                            Id = 3L,
                            CreatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 993, DateTimeKind.Local).AddTicks(2725),
                            Type = "Bet",
                            UpdatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 993, DateTimeKind.Local).AddTicks(2726)
                        },
                        new
                        {
                            Id = 4L,
                            CreatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 993, DateTimeKind.Local).AddTicks(2728),
                            Type = "Bonus",
                            UpdatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 993, DateTimeKind.Local).AddTicks(2729)
                        });
                });

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

            modelBuilder.Entity("Casino.Data.Models.Entities.RouletteRule", b =>
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

                    b.Property<float>("Pay")
                        .HasColumnType("real");

                    b.Property<long>("RouletteId")
                        .HasColumnType("bigint");

                    b.Property<long>("TypeId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("RouletteId");

                    b.HasIndex("TypeId");

                    b.ToTable("RouletteRules");
                });

            modelBuilder.Entity("Casino.Data.Models.Entities.RouletteRuleType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<float>("DefaultPay")
                        .HasColumnType("real");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("Numbers")
                        .HasColumnType("tinyint");

                    b.Property<long>("TypeId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("RouletteRuleTypes");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            DefaultPay = 1f,
                            Name = "Red",
                            Numbers = (byte)18,
                            TypeId = 1L
                        },
                        new
                        {
                            Id = 2L,
                            DefaultPay = 1f,
                            Name = "Black",
                            Numbers = (byte)18,
                            TypeId = 1L
                        },
                        new
                        {
                            Id = 3L,
                            DefaultPay = 1f,
                            Name = "Odd",
                            Numbers = (byte)18,
                            TypeId = 1L
                        },
                        new
                        {
                            Id = 4L,
                            DefaultPay = 1f,
                            Name = "Even",
                            Numbers = (byte)18,
                            TypeId = 1L
                        },
                        new
                        {
                            Id = 5L,
                            DefaultPay = 1f,
                            Name = "1 to 18",
                            Numbers = (byte)18,
                            TypeId = 1L
                        },
                        new
                        {
                            Id = 6L,
                            DefaultPay = 1f,
                            Name = "19 to 36",
                            Numbers = (byte)18,
                            TypeId = 1L
                        },
                        new
                        {
                            Id = 7L,
                            DefaultPay = 2f,
                            Name = "1 to 12",
                            Numbers = (byte)12,
                            TypeId = 1L
                        },
                        new
                        {
                            Id = 8L,
                            DefaultPay = 2f,
                            Name = "13 to 24",
                            Numbers = (byte)12,
                            TypeId = 1L
                        },
                        new
                        {
                            Id = 9L,
                            DefaultPay = 2f,
                            Name = "25 to 36",
                            Numbers = (byte)12,
                            TypeId = 1L
                        },
                        new
                        {
                            Id = 10L,
                            DefaultPay = 5f,
                            Name = "Six line",
                            Numbers = (byte)6,
                            TypeId = 1L
                        },
                        new
                        {
                            Id = 11L,
                            DefaultPay = 6f,
                            Name = "First five",
                            Numbers = (byte)5,
                            TypeId = 1L
                        },
                        new
                        {
                            Id = 12L,
                            DefaultPay = 8f,
                            Name = "Corner",
                            Numbers = (byte)4,
                            TypeId = 1L
                        },
                        new
                        {
                            Id = 13L,
                            DefaultPay = 11f,
                            Name = "Street",
                            Numbers = (byte)3,
                            TypeId = 1L
                        },
                        new
                        {
                            Id = 14L,
                            DefaultPay = 17f,
                            Name = "Split",
                            Numbers = (byte)2,
                            TypeId = 1L
                        },
                        new
                        {
                            Id = 15L,
                            DefaultPay = 35f,
                            Name = "One",
                            Numbers = (byte)1,
                            TypeId = 1L
                        });
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
                            CreatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 988, DateTimeKind.Local).AddTicks(824),
                            State = "Active",
                            UpdatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 988, DateTimeKind.Local).AddTicks(9083)
                        },
                        new
                        {
                            Id = 2L,
                            CreatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 988, DateTimeKind.Local).AddTicks(9638),
                            State = "Inactive",
                            UpdatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 988, DateTimeKind.Local).AddTicks(9651)
                        },
                        new
                        {
                            Id = 3L,
                            CreatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 988, DateTimeKind.Local).AddTicks(9657),
                            State = "Suspended",
                            UpdatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 988, DateTimeKind.Local).AddTicks(9658)
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
                            CreatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 990, DateTimeKind.Local).AddTicks(3232),
                            Type = "European",
                            UpdatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 990, DateTimeKind.Local).AddTicks(3720)
                        },
                        new
                        {
                            Id = 2L,
                            CreatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 990, DateTimeKind.Local).AddTicks(4197),
                            Type = "American",
                            UpdatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 990, DateTimeKind.Local).AddTicks(4215)
                        });
                });

            modelBuilder.Entity("Casino.Data.Models.Entities.Round", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("ClosedAt")
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

                    b.Property<long?>("UserCloseId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserOpenId")
                        .HasColumnType("bigint");

                    b.Property<string>("WinNumber")
                        .HasColumnType("nvarchar(2)")
                        .HasMaxLength(2);

                    b.HasKey("Id");

                    b.HasIndex("RouletteId");

                    b.HasIndex("StateId");

                    b.HasIndex("UserCloseId");

                    b.HasIndex("UserOpenId");

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
                            CreatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 990, DateTimeKind.Local).AddTicks(8429),
                            State = "Opened",
                            UpdatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 990, DateTimeKind.Local).AddTicks(8905)
                        },
                        new
                        {
                            Id = 2L,
                            CreatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 990, DateTimeKind.Local).AddTicks(9598),
                            State = "Closed",
                            UpdatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 990, DateTimeKind.Local).AddTicks(9610)
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

            modelBuilder.Entity("Casino.Data.Models.Entities.UserAccount", b =>
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

                    b.Property<long>("StateId")
                        .HasColumnType("bigint");

                    b.Property<long>("TypeId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<long>("UserOwnerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("StateId");

                    b.HasIndex("TypeId");

                    b.HasIndex("UserOwnerId");

                    b.ToTable("UserAccounts");
                });

            modelBuilder.Entity("Casino.Data.Models.Entities.UserAccountState", b =>
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

                    b.ToTable("UserAccountStates");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CreatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 991, DateTimeKind.Local).AddTicks(4576),
                            State = "Active",
                            UpdatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 991, DateTimeKind.Local).AddTicks(5051)
                        },
                        new
                        {
                            Id = 2L,
                            CreatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 991, DateTimeKind.Local).AddTicks(5535),
                            State = "Inactive",
                            UpdatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 991, DateTimeKind.Local).AddTicks(5546)
                        },
                        new
                        {
                            Id = 3L,
                            CreatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 991, DateTimeKind.Local).AddTicks(5554),
                            State = "Suspended",
                            UpdatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 991, DateTimeKind.Local).AddTicks(5556)
                        });
                });

            modelBuilder.Entity("Casino.Data.Models.Entities.UserAccountType", b =>
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

                    b.ToTable("UserAccountTypes");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CreatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 991, DateTimeKind.Local).AddTicks(9699),
                            Type = "Free",
                            UpdatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 992, DateTimeKind.Local).AddTicks(170)
                        },
                        new
                        {
                            Id = 2L,
                            CreatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 992, DateTimeKind.Local).AddTicks(639),
                            Type = "Real",
                            UpdatedAt = new DateTime(2020, 4, 5, 21, 45, 41, 992, DateTimeKind.Local).AddTicks(650)
                        });
                });

            modelBuilder.Entity("Casino.Data.Models.Entities.AccountTransaction", b =>
                {
                    b.HasOne("Casino.Data.Models.Entities.AccountTransactionState", "State")
                        .WithMany()
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Casino.Data.Models.Entities.AccountTransactionType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Casino.Data.Models.Entities.UserAccount", "UserAccount")
                        .WithMany("Transactions")
                        .HasForeignKey("UserAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Casino.Data.Models.Entities.User", "UserRegister")
                        .WithMany()
                        .HasForeignKey("UserRegisterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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

            modelBuilder.Entity("Casino.Data.Models.Entities.RouletteRule", b =>
                {
                    b.HasOne("Casino.Data.Models.Entities.Roulette", "Roulette")
                        .WithMany()
                        .HasForeignKey("RouletteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Casino.Data.Models.Entities.RouletteRuleType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Casino.Data.Models.Entities.RouletteRuleType", b =>
                {
                    b.HasOne("Casino.Data.Models.Entities.RouletteType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Casino.Data.Models.Entities.Round", b =>
                {
                    b.HasOne("Casino.Data.Models.Entities.Roulette", "Roulette")
                        .WithMany("Rounds")
                        .HasForeignKey("RouletteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Casino.Data.Models.Entities.RoundState", "State")
                        .WithMany()
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Casino.Data.Models.Entities.User", "UserClose")
                        .WithMany()
                        .HasForeignKey("UserCloseId");

                    b.HasOne("Casino.Data.Models.Entities.User", "UserOpen")
                        .WithMany()
                        .HasForeignKey("UserOpenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Casino.Data.Models.Entities.UserAccount", b =>
                {
                    b.HasOne("Casino.Data.Models.Entities.UserAccountState", "State")
                        .WithMany()
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Casino.Data.Models.Entities.UserAccountType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Casino.Data.Models.Entities.User", "UserOwner")
                        .WithMany("Accounts")
                        .HasForeignKey("UserOwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
