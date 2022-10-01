﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StructureService.Dimain.Realisation.Context;

#nullable disable

namespace StructureService.Dimain.Realisation.Migrations
{
    [DbContext(typeof(StructureContext))]
    partial class StructureContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("StructureService.Domain.Entities.DepartmentEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CheifUserId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CheifUserId")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("StructureService.Domain.Entities.DepartmentUnitEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<int>("PositionId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("PositionId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("DepartmentUnits");
                });

            modelBuilder.Entity("StructureService.Domain.Entities.PositionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("StructureService.Domain.Entities.DepartmentUnitEntity", b =>
                {
                    b.HasOne("StructureService.Domain.Entities.DepartmentEntity", "Department")
                        .WithMany("DepartmentUnits")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StructureService.Domain.Entities.PositionEntity", "Position")
                        .WithMany("DepartmentUnits")
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");

                    b.Navigation("Position");
                });

            modelBuilder.Entity("StructureService.Domain.Entities.DepartmentEntity", b =>
                {
                    b.Navigation("DepartmentUnits");
                });

            modelBuilder.Entity("StructureService.Domain.Entities.PositionEntity", b =>
                {
                    b.Navigation("DepartmentUnits");
                });
#pragma warning restore 612, 618
        }
    }
}
