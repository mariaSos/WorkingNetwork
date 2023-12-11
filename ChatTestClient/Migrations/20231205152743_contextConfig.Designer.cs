﻿// <auto-generated />
using System;
using ChatTestClient.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ModelsEx.Migrations
{
    [DbContext(typeof(MessageContext))]
    [Migration("20231205152743_contextConfig")]
    partial class contextConfig
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ModelsEx.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateMessage")
                        .HasColumnType("datetime2");

                    b.Property<int?>("FromUserId")
                        .HasColumnType("int")
                        .HasColumnName("from_user_id");

                    b.Property<bool>("IsReceived")
                        .HasColumnType("bit");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("text");

                    b.Property<int?>("ToUserId")
                        .HasColumnType("int")
                        .HasColumnName("to_user_id");

                    b.HasKey("Id")
                        .HasName("message_pkey");

                    b.HasIndex("FromUserId");

                    b.HasIndex("ToUserId");

                    b.ToTable("Messages", (string)null);
                });

            modelBuilder.Entity("ModelsEx.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("users_pkey");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("ModelsEx.Models.Message", b =>
                {
                    b.HasOne("ModelsEx.Models.User", "FromUser")
                        .WithMany("FromMessages")
                        .HasForeignKey("FromUserId")
                        .HasConstraintName("messages_from_user_id_fkey");

                    b.HasOne("ModelsEx.Models.User", "ToUser")
                        .WithMany("ToMessage")
                        .HasForeignKey("ToUserId")
                        .HasConstraintName("messages_to_user_id_fkey");

                    b.Navigation("FromUser");

                    b.Navigation("ToUser");
                });

            modelBuilder.Entity("ModelsEx.Models.User", b =>
                {
                    b.Navigation("FromMessages");

                    b.Navigation("ToMessage");
                });
#pragma warning restore 612, 618
        }
    }
}
