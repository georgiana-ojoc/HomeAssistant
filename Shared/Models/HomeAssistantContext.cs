using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Shared.Models
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
        public DbSet<Room> Rooms { get; set; }
        public DbSet<LightBulb> LightBulbs { get; set; }
        public DbSet<Door> Doors { get; set; }
        public DbSet<Thermostat> Thermostats { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<LightBulbCommand> LightBulbCommands { get; set; }
        public DbSet<DoorCommand> DoorCommands { get; set; }
        public DbSet<ThermostatCommand> ThermostatCommands { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionService.Connection,
                    builder => builder.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<House>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("houses_pk")
                    .IsClustered(false);

                entity.ToTable("houses");

                entity.HasIndex(e => e.Id, "houses_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("rooms_pk")
                    .IsClustered(false);

                entity.ToTable("rooms");

                entity.HasIndex(e => e.Id, "rooms_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.HouseId).HasColumnName("house_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("name");

                entity.HasOne(d => d.House)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.HouseId)
                    .HasConstraintName("rooms_houses_id_fk");
            });

            modelBuilder.Entity<LightBulb>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("light_bulb_pk")
                    .IsClustered(false);

                entity.ToTable("light_bulbs");

                entity.HasIndex(e => e.Id, "light_bulb_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Color).HasColumnName("color");

                entity.Property(e => e.Intensity).HasColumnName("intensity");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("name");

                entity.Property(e => e.RoomId).HasColumnName("room_id");

                entity.Property(e => e.Status).HasDefaultValue(false).HasColumnName("status");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.LightBulbs)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("light_bulb_rooms_id_fk");
            });

            modelBuilder.Entity<Door>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("doors_pk")
                    .IsClustered(false);

                entity.ToTable("doors");

                entity.HasIndex(e => e.Id, "doors_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("name");

                entity.Property(e => e.RoomId).HasColumnName("room_id");

                entity.Property(e => e.Status).HasDefaultValue(false).HasColumnName("status");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Doors)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("doors_rooms_id_fk");
            });

            modelBuilder.Entity<Thermostat>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("thermostats_pk")
                    .IsClustered(false);

                entity.ToTable("thermostats");

                entity.HasIndex(e => e.Id, "thermostats_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("name");

                entity.Property(e => e.RoomId).HasColumnName("room_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Temperature)
                    .HasColumnType("decimal(3, 1)")
                    .HasColumnName("temperature");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Thermostats)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("thermostats_rooms_id_fk");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("schedules_pk")
                    .IsClustered(false);

                entity.ToTable("schedules");

                entity.HasIndex(e => e.Id, "schedules_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Frequency).HasColumnName("frequency");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("name");

                entity.Property(e => e.Time).HasColumnName("time");
            });

            modelBuilder.Entity<LightBulbCommand>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("light_bulb_commands_pk")
                    .IsClustered(false);

                entity.ToTable("light_bulb_commands");

                entity.HasIndex(e => e.Id, "light_bulb_commands_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Color).HasColumnName("color");

                entity.Property(e => e.Intensity).HasColumnName("intensity");

                entity.Property(e => e.LightBulbId).HasColumnName("light_bulb_id");

                entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");

                entity.HasOne(d => d.LightBulb)
                    .WithMany(p => p.LightBulbCommands)
                    .HasForeignKey(d => d.LightBulbId)
                    .HasConstraintName("light_bulb_commands_light_bulbs_id_fk");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.LightBulbCommands)
                    .HasForeignKey(d => d.ScheduleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("light_bulb_commands_schedules_id_fk");
            });

            modelBuilder.Entity<DoorCommand>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("door_commands_pk")
                    .IsClustered(false);

                entity.ToTable("door_commands");

                entity.HasIndex(e => e.Id, "door_commands_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.DoorId).HasColumnName("door_id");

                entity.Property(e => e.Locked).HasColumnName("locked");

                entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");

                entity.HasOne(d => d.Door)
                    .WithMany(p => p.DoorCommands)
                    .HasForeignKey(d => d.DoorId)
                    .HasConstraintName("door_commands_doors_id_fk");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.DoorCommands)
                    .HasForeignKey(d => d.ScheduleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("door_commands_schedules_id_fk");
            });

            modelBuilder.Entity<ThermostatCommand>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("thermostat_commands_pk")
                    .IsClustered(false);

                entity.ToTable("thermostat_commands");

                entity.HasIndex(e => e.Id, "thermostat_commands_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");

                entity.Property(e => e.Temperature)
                    .HasColumnType("decimal(3, 1)")
                    .HasColumnName("temperature");

                entity.Property(e => e.ThermostatId).HasColumnName("thermostat_id");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.ThermostatCommands)
                    .HasForeignKey(d => d.ScheduleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("thermostat_commands_schedules_id_fk");

                entity.HasOne(d => d.Thermostat)
                    .WithMany(p => p.ThermostatCommands)
                    .HasForeignKey(d => d.ThermostatId)
                    .HasConstraintName("thermostat_commands_thermostats_id_fk");
            });
        }
    }
}