﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestWunderMobilityCheckout;

namespace TestWunderMobilityCheckout.Migrations
{
    [DbContext(typeof(TestWunderMobilityCheckoutDBContext))]
    [Migration("20210123081749_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Communication.EventDataService.EventDataModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Completed")
                        .HasColumnType("bit");

                    b.Property<string>("EventBody")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("InputId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("LastSentDateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Outgoing")
                        .HasColumnType("bit");

                    b.Property<long?>("ParentId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("InputId");

                    b.ToTable("EventDataModel");
                });

            modelBuilder.Entity("TestWunderMobilityCheckout.Aggregates.Customers.Models.CustomersList", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("PromotionalDiscount")
                        .HasColumnType("real");

                    b.Property<float>("PromotionalSum")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("CustomersList");
                });

            modelBuilder.Entity("TestWunderMobilityCheckout.Aggregates.Products.Models.ProductList", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<string>("ProductCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("PromotionalPrice")
                        .HasColumnType("real");

                    b.Property<long>("PromotionalQuantity")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("ProductList");
                });
#pragma warning restore 612, 618
        }
    }
}
