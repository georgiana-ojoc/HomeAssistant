using System;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

#nullable disable

namespace Shared
{
    public class HomeAssistantContext : DbContext
    {
        public HomeAssistantContext()
        {
        }

        public HomeAssistantContext(DbContextOptions<HomeAssistantContext> options)
            : base(options)
        {
        }

        public DbSet<House> Houses { get; set; }
        public DbSet<CheckoutOffer> CheckoutOffer { get; set; }
        public DbSet<UserCheckoutOffer> UserCheckoutOffer { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<LightBulb> LightBulbs { get; set; }
        public DbSet<Door> Doors { get; set; }
        public DbSet<Thermostat> Thermostats { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<LightBulbCommand> LightBulbCommands { get; set; }
        public DbSet<DoorCommand> DoorCommands { get; set; }
        public DbSet<ThermostatCommand> ThermostatCommands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<House>(entity =>
            {
                entity.HasKey(h => h.Id)
                    .HasName("houses_pk")
                    .IsClustered(false);

                entity.ToTable("houses");

                entity.HasIndex(h => h.Id, "houses_id_uindex")
                    .IsUnique();

                entity.HasIndex(h => new {h.Name, h.Email}, "houses_email_name_uindex")
                    .IsUnique();

                entity.Property(h => h.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(h => h.Email)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("email");

                entity.Property(h => h.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("name");
            });
            
            
            modelBuilder.Entity<UserCheckoutOffer>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("user_checkout_offers_pk")
                    .IsClustered(false);

                entity.ToTable("user_checkout_offers");

                entity.HasIndex(e => e.Id, "user_checkout_offers_id")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "user_checkout_offers_email_uindex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("email");
                
                entity.Property(e => e.CheckoutOffersId).HasColumnName("checkout_offers_id");

                entity.HasOne(e => e.CheckoutOffer)
                    .WithMany(e => e.UserCheckoutOffers)
                    .HasForeignKey(d => d.CheckoutOffersId)
                    .HasConstraintName("users_checkout_offers_checkout_offers_id_fk");
            });

            modelBuilder.Entity<CheckoutOffer>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("user_checkout_offers_pk")
                    .IsClustered(false);

                entity.ToTable("checkout_offers");

                entity.HasIndex(e => e.Id, "user_checkout_offers_id")
                    .IsUnique();


                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");


                entity.Property(e => e.OfferName).HasColumnName("offer_name");

                entity.Property(e => e.OfferDescription).HasColumnName("offer_description");

                entity.Property(e => e.OfferValue).HasColumnName("offer_value");

                entity.Property(e => e.HouseLimit).HasColumnName("house_limit");

                entity.Property(e => e.RoomLimit).HasColumnName("room_limit");

                entity.Property(e => e.DoorLimit).HasColumnName("door_limit");

                entity.Property(e => e.LightBulbLimit).HasColumnName("light_bulb_limit");

                entity.Property(e => e.ThermostatLimit).HasColumnName("thermostat_limit");

                entity.Property(e => e.ScheduleLimit).HasColumnName("schedule_limit");

                entity.Property(e => e.CommandLimit).HasColumnName("command_limit");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(r => r.Id)
                    .HasName("rooms_pk")
                    .IsClustered(false);

                entity.ToTable("rooms");

                entity.HasIndex(r => r.Id, "rooms_id_uindex")
                    .IsUnique();

                entity.HasIndex(r => new {r.HouseId, r.Name}, "rooms_house_id_name_uindex")
                    .IsUnique();

                entity.Property(r => r.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(r => r.HouseId).HasColumnName("house_id");

                entity.Property(r => r.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("name");

                entity.HasOne(r => r.House)
                    .WithMany(h => h.Rooms)
                    .HasForeignKey(r => r.HouseId)
                    .HasConstraintName("rooms_houses_id_fk");
            });

            modelBuilder.Entity<LightBulb>(entity =>
            {
                entity.HasKey(lb => lb.Id)
                    .HasName("light_bulb_pk")
                    .IsClustered(false);

                entity.ToTable("light_bulbs");

                entity.HasIndex(lb => lb.Id, "light_bulb_id_uindex")
                    .IsUnique();

                entity.HasIndex(lb => new {lb.RoomId, lb.Name}, "light_bulbs_room_id_name_uindex")
                    .IsUnique();

                entity.Property(lb => lb.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(lb => lb.Color).HasColumnName("color");

                entity.Property(lb => lb.Intensity).HasColumnName("intensity");

                entity.Property(lb => lb.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("name");

                entity.Property(lb => lb.RoomId).HasColumnName("room_id");

                entity.Property(lb => lb.Status).HasDefaultValue(false).HasColumnName("status");

                entity.HasOne(lb => lb.Room)
                    .WithMany(r => r.LightBulbs)
                    .HasForeignKey(lb => lb.RoomId)
                    .HasConstraintName("light_bulb_rooms_id_fk");
            });

            modelBuilder.Entity<Door>(entity =>
            {
                entity.HasKey(d => d.Id)
                    .HasName("doors_pk")
                    .IsClustered(false);

                entity.ToTable("doors");

                entity.HasIndex(d => d.Id, "doors_id_uindex")
                    .IsUnique();

                entity.HasIndex(d => new {d.RoomId, d.Name}, "doors_room_id_name_uindex")
                    .IsUnique();

                entity.Property(d => d.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(d => d.Locked).HasColumnName("locked");

                entity.Property(d => d.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("name");

                entity.Property(d => d.RoomId).HasColumnName("room_id");

                entity.Property(d => d.Status).HasDefaultValue(false).HasColumnName("status");

                entity.HasOne(d => d.Room)
                    .WithMany(r => r.Doors)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("doors_rooms_id_fk");
            });

            modelBuilder.Entity<Thermostat>(entity =>
            {
                entity.HasKey(t => t.Id)
                    .HasName("thermostats_pk")
                    .IsClustered(false);

                entity.ToTable("thermostats");

                entity.HasIndex(t => t.Id, "thermostats_id_uindex")
                    .IsUnique();

                entity.HasIndex(t => t.Id, "thermostats_room_id_name_uindex")
                    .IsUnique();

                entity.HasCheckConstraint("thermostats_temperature_ck",
                    "temperature >= 7.0 and temperature <= 30.0");

                entity.Property(t => t.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("name");

                entity.Property(t => t.RoomId).HasColumnName("room_id");

                entity.Property(t => t.Status).HasColumnName("status");

                entity.Property(t => t.Temperature)
                    .HasColumnType("decimal(3, 1)")
                    .HasColumnName("temperature");

                entity.HasOne(t => t.Room)
                    .WithMany(r => r.Thermostats)
                    .HasForeignKey(t => t.RoomId)
                    .HasConstraintName("thermostats_rooms_id_fk");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasKey(s => s.Id)
                    .HasName("schedules_pk")
                    .IsClustered(false);

                entity.ToTable("schedules");

                entity.HasIndex(s => s.Id, "schedules_id_uindex")
                    .IsUnique();

                entity.HasIndex(s => new {s.Email, s.Name}, "schedules_email_name_uindex")
                    .IsUnique();

                entity.HasCheckConstraint("schedules_days_ck",
                    "days > 0 and days <= 127");

                entity.Property(s => s.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(s => s.Days)
                    .IsRequired()
                    .HasColumnName("days");

                entity.Property(s => s.Email)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("email");

                entity.Property(s => s.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("name");

                entity.Property(s => s.Time)
                    .HasColumnName("time")
                    .HasConversion(value => TimeSpan.Parse(value),
                        value => value.ToString(@"hh\:mm"));
            });

            modelBuilder.Entity<LightBulbCommand>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("light_bulb_commands_pk")
                    .IsClustered(false);

                entity.ToTable("light_bulb_commands");

                entity.HasIndex(lbc => lbc.Id, "light_bulb_commands_id_uindex")
                    .IsUnique();

                entity.HasIndex(lbc => new {lbc.LightBulbId, lbc.ScheduleId},
                        "light_bulb_commands_light_bulb_id_schedule_id_uindex")
                    .IsUnique();

                entity.Property(lbc => lbc.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(lbc => lbc.Color).HasColumnName("color");

                entity.Property(lbc => lbc.Intensity).HasColumnName("intensity");

                entity.Property(lbc => lbc.LightBulbId).HasColumnName("light_bulb_id");

                entity.Property(lbc => lbc.ScheduleId).HasColumnName("schedule_id");

                entity.HasOne(lbc => lbc.LightBulb)
                    .WithMany(lb => lb.LightBulbCommands)
                    .HasForeignKey(lbc => lbc.LightBulbId)
                    .HasConstraintName("light_bulb_commands_light_bulbs_id_fk");

                entity.HasOne(lbc => lbc.Schedule)
                    .WithMany(s => s.LightBulbCommands)
                    .HasForeignKey(lbc => lbc.ScheduleId)
                    .HasConstraintName("light_bulb_commands_schedules_id_fk");
            });

            modelBuilder.Entity<DoorCommand>(entity =>
            {
                entity.HasKey(dc => dc.Id)
                    .HasName("door_commands_pk")
                    .IsClustered(false);

                entity.ToTable("door_commands");

                entity.HasIndex(dc => dc.Id, "door_commands_id_uindex")
                    .IsUnique();

                entity.HasIndex(dc => new {dc.DoorId, dc.ScheduleId},
                        "door_commands_door_id_schedule_id_uindex")
                    .IsUnique();

                entity.Property(dc => dc.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(dc => dc.DoorId).HasColumnName("door_id");

                entity.Property(dc => dc.Locked).HasColumnName("locked");

                entity.Property(dc => dc.ScheduleId).HasColumnName("schedule_id");

                entity.HasOne(dc => dc.Door)
                    .WithMany(d => d.DoorCommands)
                    .HasForeignKey(dc => dc.DoorId)
                    .HasConstraintName("door_commands_doors_id_fk");

                entity.HasOne(dc => dc.Schedule)
                    .WithMany(s => s.DoorCommands)
                    .HasForeignKey(dc => dc.ScheduleId)
                    .HasConstraintName("door_commands_schedules_id_fk");
            });

            modelBuilder.Entity<ThermostatCommand>(entity =>
            {
                entity.HasKey(tc => tc.Id)
                    .HasName("thermostat_commands_pk")
                    .IsClustered(false);

                entity.ToTable("thermostat_commands");

                entity.HasIndex(tc => tc.Id, "thermostat_commands_id_uindex")
                    .IsUnique();

                entity.HasIndex(tc => new {tc.ThermostatId, tc.ScheduleId},
                        "thermostat_commands_thermostat_id_schedule_id_uindex")
                    .IsUnique();

                entity.HasCheckConstraint("thermostat_commands_temperature_ck",
                    "temperature >= 7.0 and temperature <= 30.0");

                entity.Property(tc => tc.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(tc => tc.ScheduleId).HasColumnName("schedule_id");

                entity.Property(tc => tc.Temperature)
                    .HasColumnType("decimal(3, 1)")
                    .HasColumnName("temperature");

                entity.Property(tc => tc.ThermostatId).HasColumnName("thermostat_id");

                entity.HasOne(tc => tc.Thermostat)
                    .WithMany(t => t.ThermostatCommands)
                    .HasForeignKey(tc => tc.ThermostatId)
                    .HasConstraintName("thermostat_commands_thermostats_id_fk");

                entity.HasOne(tc => tc.Schedule)
                    .WithMany(s => s.ThermostatCommands)
                    .HasForeignKey(tc => tc.ScheduleId)
                    .HasConstraintName("thermostat_commands_schedules_id_fk");
            });
        }
    }
}