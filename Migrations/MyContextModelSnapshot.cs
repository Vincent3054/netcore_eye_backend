﻿// <auto-generated />
using System;
using DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace project.Migrations
{
    [DbContext(typeof(MyContext))]
    partial class MyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Models.AnalysisLogModel", b =>
                {
                    b.Property<string>("A_Id")
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("AnalysisTime")
                        .HasColumnType("datetime");

                    b.Property<string>("B_Id")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Image")
                        .HasColumnType("NvarChar(Max)");

                    b.Property<string>("M_Id")
                        .HasColumnType("varchar(50)");

                    b.HasKey("A_Id");

                    b.HasIndex("B_Id");

                    b.HasIndex("M_Id");

                    b.ToTable("AnalysisLog");

                    b.HasData(
                        new
                        {
                            A_Id = "1",
                            AnalysisTime = new DateTime(2020, 8, 11, 10, 40, 17, 308, DateTimeKind.Local).AddTicks(4981),
                            B_Id = "1",
                            Image = "https://i.imgur.com/PuC21Ma.png",
                            M_Id = "1"
                        });
                });

            modelBuilder.Entity("Models.AnalysisStatusModel", b =>
                {
                    b.Property<int>("AS_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("A_Id")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("S_Id")
                        .HasColumnType("varchar(50)");

                    b.HasKey("AS_Id");

                    b.HasIndex("A_Id");

                    b.HasIndex("S_Id");

                    b.ToTable("AnalysisStatus");

                    b.HasData(
                        new
                        {
                            AS_Id = 1,
                            A_Id = "1",
                            S_Id = "1"
                        });
                });

            modelBuilder.Entity("Models.BeforeAnalysisLogModel", b =>
                {
                    b.Property<string>("B_Id")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("RawImage")
                        .HasColumnType("NvarChar(Max)");

                    b.Property<DateTime>("RawTime")
                        .HasColumnType("datetime");

                    b.HasKey("B_Id");

                    b.ToTable("BeforeAnalysisLog");

                    b.HasData(
                        new
                        {
                            B_Id = "1",
                            RawImage = "https://i.imgur.com/cfeJ9j7.png",
                            RawTime = new DateTime(2020, 8, 11, 10, 40, 17, 310, DateTimeKind.Local).AddTicks(8539)
                        });
                });

            modelBuilder.Entity("Models.StatusModel", b =>
                {
                    b.Property<string>("S_Id")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Message")
                        .HasColumnType("Nvarchar(200)");

                    b.Property<string>("StatusName")
                        .HasColumnType("nvarChar(36)");

                    b.HasKey("S_Id");

                    b.ToTable("Status");

                    b.HasData(
                        new
                        {
                            S_Id = "1",
                            Message = "坐姿過於前傾",
                            StatusName = "坐姿警示"
                        });
                });

            modelBuilder.Entity("Models.UserModel", b =>
                {
                    b.Property<string>("M_Id")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Account")
                        .HasColumnType("varchar(30)");

                    b.Property<string>("AuthCode")
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Password")
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("Role")
                        .HasColumnType("bit");

                    b.Property<string>("Sex")
                        .HasColumnType("varchar(2)");

                    b.HasKey("M_Id");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            M_Id = "1",
                            Account = "admin001",
                            AuthCode = "",
                            BirthDate = new DateTime(2020, 8, 11, 10, 40, 17, 296, DateTimeKind.Local).AddTicks(377),
                            CreateTime = new DateTime(2020, 8, 11, 10, 40, 17, 298, DateTimeKind.Local).AddTicks(6566),
                            Email = "ok96305@gmail.com",
                            Name = "陳建成",
                            Password = "12345",
                            Role = true,
                            Sex = "男"
                        });
                });

            modelBuilder.Entity("Models.AnalysisLogModel", b =>
                {
                    b.HasOne("Models.BeforeAnalysisLogModel", "TheBeforeAnalysisLogModel")
                        .WithMany("AnalysisLog")
                        .HasForeignKey("B_Id");

                    b.HasOne("Models.UserModel", "TheUser")
                        .WithMany("AnalysisLog")
                        .HasForeignKey("M_Id");
                });

            modelBuilder.Entity("Models.AnalysisStatusModel", b =>
                {
                    b.HasOne("Models.AnalysisLogModel", "TheAnalysisLog")
                        .WithMany("AnalysisStatus")
                        .HasForeignKey("A_Id");

                    b.HasOne("Models.StatusModel", "TheStatus")
                        .WithMany("AnalysisStatus")
                        .HasForeignKey("S_Id");
                });
#pragma warning restore 612, 618
        }
    }
}
