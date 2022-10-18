﻿// <auto-generated />
using System;
using BFuel.Api.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BFuel.Api.Migrations
{
    [DbContext(typeof(BFuelContext))]
    [Migration("20220517145321_BancoInicial")]
    partial class BancoInicial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10");

            modelBuilder.Entity("BFuel.Domain.Models.Supply", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FuelName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("InsertedDate")
                        .HasColumnType("TEXT");

                    b.Property<double>("LiterPrice")
                        .HasColumnType("REAL");

                    b.Property<string>("LocalName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("TotalLiters")
                        .HasColumnType("REAL");

                    b.Property<double>("TotalPaid")
                        .HasColumnType("REAL");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Supplies");
                });

            modelBuilder.Entity("BFuel.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BFuel.Domain.Models.Supply", b =>
                {
                    b.HasOne("BFuel.Domain.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
