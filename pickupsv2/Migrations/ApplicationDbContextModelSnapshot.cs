﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using pickupsv2.Data;

namespace pickupsv2.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("pickupsv2.Models.Game", b =>
                {
                    b.Property<Guid>("GameId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ImageExtension");

                    b.Property<string>("Name");

                    b.Property<string>("Rules");

                    b.HasKey("GameId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("pickupsv2.Models.GameAdmin", b =>
                {
                    b.Property<Guid>("GameAdminId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("GameId");

                    b.Property<Guid?>("Id");

                    b.HasKey("GameAdminId");

                    b.HasIndex("GameId");

                    b.HasIndex("Id");

                    b.ToTable("GameAdmin");
                });

            modelBuilder.Entity("pickupsv2.Models.Map", b =>
                {
                    b.Property<Guid>("MapId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("GameId");

                    b.Property<string>("ImageExtension");

                    b.Property<string>("Name");

                    b.HasKey("MapId");

                    b.HasIndex("GameId");

                    b.ToTable("Maps");
                });

            modelBuilder.Entity("pickupsv2.Models.Match", b =>
                {
                    b.Property<Guid>("MatchId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("Admin");

                    b.Property<Guid?>("GameId");

                    b.Property<Guid?>("Host");

                    b.Property<Guid?>("MapId");

                    b.Property<bool>("NeedHost");

                    b.HasKey("MatchId");

                    b.HasIndex("GameId");

                    b.HasIndex("MapId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("pickupsv2.Models.SteamPlayer", b =>
                {
                    b.Property<string>("steamid")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("avatar");

                    b.Property<string>("avatarfull");

                    b.Property<string>("avatarmedium");

                    b.Property<int>("communityvisibilitystate");

                    b.Property<int>("lastlogoff");

                    b.Property<int>("loccityid");

                    b.Property<string>("loccountrycode");

                    b.Property<string>("locstatecode");

                    b.Property<string>("personaname");

                    b.Property<int>("personastate");

                    b.Property<int>("personastateflags");

                    b.Property<string>("primaryclanid");

                    b.Property<int>("profilestate");

                    b.Property<string>("profileurl");

                    b.Property<string>("realname");

                    b.Property<int>("timecreated");

                    b.HasKey("steamid");

                    b.ToTable("SteamPlayer");
                });

            modelBuilder.Entity("pickupsv2.Models.Player", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<byte[]>("Avatar");

                    b.Property<Guid?>("CurMatch");

                    b.Property<string>("DisplayName");

                    b.Property<string>("steamid");

                    b.HasIndex("CurMatch");

                    b.HasIndex("steamid");

                    b.HasDiscriminator().HasValue("Player");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("pickupsv2.Models.GameAdmin", b =>
                {
                    b.HasOne("pickupsv2.Models.Player", "Player")
                        .WithMany("AdminFor")
                        .HasForeignKey("GameId");

                    b.HasOne("pickupsv2.Models.Game", "Game")
                        .WithMany("Admins")
                        .HasForeignKey("Id");
                });

            modelBuilder.Entity("pickupsv2.Models.Map", b =>
                {
                    b.HasOne("pickupsv2.Models.Game")
                        .WithMany("Maps")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("pickupsv2.Models.Match", b =>
                {
                    b.HasOne("pickupsv2.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId");

                    b.HasOne("pickupsv2.Models.Map", "Map")
                        .WithMany()
                        .HasForeignKey("MapId");
                });

            modelBuilder.Entity("pickupsv2.Models.Player", b =>
                {
                    b.HasOne("pickupsv2.Models.Match")
                        .WithMany("Players")
                        .HasForeignKey("CurMatch");

                    b.HasOne("pickupsv2.Models.SteamPlayer", "SteamPlayer")
                        .WithMany()
                        .HasForeignKey("steamid");
                });
#pragma warning restore 612, 618
        }
    }
}
