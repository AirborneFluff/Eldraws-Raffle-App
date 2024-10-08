﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RaffleApi.Data;

#nullable disable

namespace RaffleApi.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("RaffleApi.Entities.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("RaffleApi.Entities.Clan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal?>("DiscordChannelId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique();

                    b.HasIndex("OwnerId");

                    b.ToTable("Clans", (string)null);
                });

            modelBuilder.Entity("RaffleApi.Entities.ClanMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClanId")
                        .HasColumnType("int");

                    b.Property<string>("MemberId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ClanId");

                    b.HasIndex("MemberId");

                    b.ToTable("ClanMembers", (string)null);
                });

            modelBuilder.Entity("RaffleApi.Entities.Entrant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("ClanId")
                        .HasColumnType("int");

                    b.Property<string>("Gamertag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedGamertag")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("TotalDonations")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClanId")
                        .HasDatabaseName("IX_Entrants_ClanId");

                    b.HasIndex("NormalizedGamertag")
                        .HasDatabaseName("IX_Entrants_NormalizedGamertag");

                    b.HasIndex("Active", "NormalizedGamertag")
                        .HasDatabaseName("IX_Entrants_Active_NormalizedGamertag");

                    b.HasIndex("Active", "TotalDonations")
                        .HasDatabaseName("IX_Entrants_Active_TotalDonations");

                    b.HasIndex("ClanId", "NormalizedGamertag")
                        .IsUnique();

                    b.ToTable("Entrants", (string)null);
                });

            modelBuilder.Entity("RaffleApi.Entities.Raffle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClanId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CloseDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DiscordChannelId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("DiscordMessageId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<DateTime>("DrawDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("EntryCost")
                        .HasColumnType("int");

                    b.Property<string>("HostId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("OpenDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalDonations")
                        .HasColumnType("int");

                    b.Property<int>("TotalTickets")
                        .HasColumnType("int");

                    b.Property<string>("_additionalMessageIds")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("AdditionalMessages");

                    b.HasKey("Id");

                    b.HasIndex("ClanId");

                    b.HasIndex("HostId");

                    b.ToTable("Raffles", (string)null);
                });

            modelBuilder.Entity("RaffleApi.Entities.RaffleEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Complimentary")
                        .HasColumnType("bit");

                    b.Property<int>("Donation")
                        .HasColumnType("int");

                    b.Property<int>("EntrantId")
                        .HasColumnType("int");

                    b.Property<int>("HighTicket")
                        .HasColumnType("int");

                    b.Property<DateTime>("InputDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("LowTicket")
                        .HasColumnType("int");

                    b.Property<int>("RaffleId")
                        .HasColumnType("int");

                    b.Property<string>("_tickets")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Tickets");

                    b.HasKey("Id");

                    b.HasIndex("EntrantId");

                    b.HasIndex("RaffleId");

                    b.ToTable("Entries", (string)null);
                });

            modelBuilder.Entity("RaffleApi.Entities.RafflePrize", b =>
                {
                    b.Property<int>("Place")
                        .HasColumnType("int");

                    b.Property<int>("RaffleId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("DonationPercentage")
                        .HasColumnType("real");

                    b.Property<int?>("WinnerId")
                        .HasColumnType("int");

                    b.Property<int?>("WinningTicketNumber")
                        .HasColumnType("int");

                    b.HasKey("Place", "RaffleId");

                    b.HasIndex("RaffleId");

                    b.HasIndex("WinnerId");

                    b.ToTable("Prizes", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("RaffleApi.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("RaffleApi.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RaffleApi.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("RaffleApi.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RaffleApi.Entities.Clan", b =>
                {
                    b.HasOne("RaffleApi.Entities.AppUser", "Owner")
                        .WithMany("OwnedClans")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("RaffleApi.Entities.ClanMember", b =>
                {
                    b.HasOne("RaffleApi.Entities.Clan", "Clan")
                        .WithMany("Members")
                        .HasForeignKey("ClanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RaffleApi.Entities.AppUser", "Member")
                        .WithMany("Clans")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Clan");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("RaffleApi.Entities.Entrant", b =>
                {
                    b.HasOne("RaffleApi.Entities.Clan", "Clan")
                        .WithMany("Entrants")
                        .HasForeignKey("ClanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Clan");
                });

            modelBuilder.Entity("RaffleApi.Entities.Raffle", b =>
                {
                    b.HasOne("RaffleApi.Entities.Clan", "Clan")
                        .WithMany("Raffles")
                        .HasForeignKey("ClanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RaffleApi.Entities.AppUser", "Host")
                        .WithMany("HostedRaffles")
                        .HasForeignKey("HostId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Clan");

                    b.Navigation("Host");
                });

            modelBuilder.Entity("RaffleApi.Entities.RaffleEntry", b =>
                {
                    b.HasOne("RaffleApi.Entities.Entrant", "Entrant")
                        .WithMany("Entries")
                        .HasForeignKey("EntrantId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("RaffleApi.Entities.Raffle", "Raffle")
                        .WithMany("Entries")
                        .HasForeignKey("RaffleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Entrant");

                    b.Navigation("Raffle");
                });

            modelBuilder.Entity("RaffleApi.Entities.RafflePrize", b =>
                {
                    b.HasOne("RaffleApi.Entities.Raffle", "Raffle")
                        .WithMany("Prizes")
                        .HasForeignKey("RaffleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RaffleApi.Entities.Entrant", "Winner")
                        .WithMany("Prizes")
                        .HasForeignKey("WinnerId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Raffle");

                    b.Navigation("Winner");
                });

            modelBuilder.Entity("RaffleApi.Entities.AppUser", b =>
                {
                    b.Navigation("Clans");

                    b.Navigation("HostedRaffles");

                    b.Navigation("OwnedClans");
                });

            modelBuilder.Entity("RaffleApi.Entities.Clan", b =>
                {
                    b.Navigation("Entrants");

                    b.Navigation("Members");

                    b.Navigation("Raffles");
                });

            modelBuilder.Entity("RaffleApi.Entities.Entrant", b =>
                {
                    b.Navigation("Entries");

                    b.Navigation("Prizes");
                });

            modelBuilder.Entity("RaffleApi.Entities.Raffle", b =>
                {
                    b.Navigation("Entries");

                    b.Navigation("Prizes");
                });
#pragma warning restore 612, 618
        }
    }
}
