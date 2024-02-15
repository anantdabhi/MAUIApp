﻿// <auto-generated />
using ChatAppApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ChatAppApi.Migrations
{
    [DbContext(typeof(ChatAppContext))]
    [Migration("20230327102401_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ChatAppApi.Entities.tblusers", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("avatarfile")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("isonline")
                        .HasColumnType("boolean");

                    b.Property<string>("lastlogonday")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("lastlogontime")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("loginid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("TblUsers");
                });
#pragma warning restore 612, 618
        }
    }
}