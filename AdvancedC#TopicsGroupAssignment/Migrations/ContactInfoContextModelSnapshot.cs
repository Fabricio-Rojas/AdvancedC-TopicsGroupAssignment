﻿// <auto-generated />
using System;
using AdvancedC_TopicsGroupAssignment.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AdvancedC_TopicsGroupAssignment.Migrations
{
    [DbContext(typeof(ContactInfoContext))]
    partial class ContactInfoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AdvancedC_TopicsGroupAssignment.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("BusinessId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("nvarchar(7)");

                    b.Property<string>("StreetName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("StreetNumber")
                        .HasColumnType("int");

                    b.Property<int?>("UnitNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BusinessId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("AdvancedC_TopicsGroupAssignment.Models.Business", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Businesses");
                });

            modelBuilder.Entity("AdvancedC_TopicsGroupAssignment.Models.BusinessPerson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BusinessId")
                        .HasColumnType("int");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BusinessId");

                    b.HasIndex("PersonId");

                    b.ToTable("BusinessPersons");
                });

            modelBuilder.Entity("AdvancedC_TopicsGroupAssignment.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("AdvancedC_TopicsGroupAssignment.Models.PersonAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("PersonId");

                    b.ToTable("PersonAddresses");
                });

            modelBuilder.Entity("AdvancedC_TopicsGroupAssignment.Models.Address", b =>
                {
                    b.HasOne("AdvancedC_TopicsGroupAssignment.Models.Business", "Business")
                        .WithMany("Addresses")
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Business");
                });

            modelBuilder.Entity("AdvancedC_TopicsGroupAssignment.Models.BusinessPerson", b =>
                {
                    b.HasOne("AdvancedC_TopicsGroupAssignment.Models.Business", "Business")
                        .WithMany("People")
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AdvancedC_TopicsGroupAssignment.Models.Person", "Person")
                        .WithMany("Businesses")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Business");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("AdvancedC_TopicsGroupAssignment.Models.PersonAddress", b =>
                {
                    b.HasOne("AdvancedC_TopicsGroupAssignment.Models.Address", "Address")
                        .WithMany("People")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AdvancedC_TopicsGroupAssignment.Models.Person", "Person")
                        .WithMany("Addresses")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("AdvancedC_TopicsGroupAssignment.Models.Address", b =>
                {
                    b.Navigation("People");
                });

            modelBuilder.Entity("AdvancedC_TopicsGroupAssignment.Models.Business", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("People");
                });

            modelBuilder.Entity("AdvancedC_TopicsGroupAssignment.Models.Person", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("Businesses");
                });
#pragma warning restore 612, 618
        }
    }
}