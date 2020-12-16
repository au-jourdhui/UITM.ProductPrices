﻿// <auto-generated />
using System;
using Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Application.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20201205173454_FixPrice")]
    partial class FixPrice
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Application.Shared.Contractor", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Contractors");
                });

            modelBuilder.Entity("Application.Shared.ContractorProductPrice", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ContractorID")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<int?>("ProductID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("ContractorID");

                    b.HasIndex("ProductID");

                    b.ToTable("ContractorProductPrices");
                });

            modelBuilder.Entity("Application.Shared.Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<int?>("ProductTypeID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("ProductTypeID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Application.Shared.ProductType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("ProductTypes");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Name = "Goods"
                        },
                        new
                        {
                            ID = 2,
                            Name = "Service"
                        });
                });

            modelBuilder.Entity("Application.Shared.ContractorProductPrice", b =>
                {
                    b.HasOne("Application.Shared.Contractor", "Contractor")
                        .WithMany()
                        .HasForeignKey("ContractorID");

                    b.HasOne("Application.Shared.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID");

                    b.Navigation("Contractor");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Application.Shared.Product", b =>
                {
                    b.HasOne("Application.Shared.ProductType", "ProductType")
                        .WithMany("Products")
                        .HasForeignKey("ProductTypeID");

                    b.Navigation("ProductType");
                });

            modelBuilder.Entity("Application.Shared.ProductType", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
