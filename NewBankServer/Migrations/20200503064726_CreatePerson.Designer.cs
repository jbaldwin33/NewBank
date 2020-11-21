﻿// <auto-generated />
using System;
using NewBankServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NewBankServer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200503064726_CreatePerson")]
    partial class CreatePerson
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NewBankServer.Models.PersonModel", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid?>("SkillID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.HasIndex("SkillID");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("NewBankServer.Models.Skill", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Proficiency")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Skill");
                });

            modelBuilder.Entity("NewBankServer.Models.PersonModel", b =>
                {
                    b.HasOne("NewBankServer.Models.Skill", "Skill")
                        .WithMany()
                        .HasForeignKey("SkillID");
                });
#pragma warning restore 612, 618
        }
    }
}
