using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

namespace SmartRealms.API.Infrastructure.Context
{    
    public class DevicesContext : DbContext
    {
        public DevicesContext(DbContextOptions<DevicesContext> options) : base(options)
        {
        }

        public DbSet<Device> Devices { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<ScheduleBase> Schedules { get; set; }
        public DbSet<Scenario> Scenarios { get; set; }
        public DbSet<ScenarioAction> ScenarioActions { get; set; }
        public DbSet<ScenarioMetric> ScenarioMetrics { get; set; }
        public DbSet<GroupScenario> GroupScenarios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new DeviceEntityTypeConfiguration());
            builder.ApplyConfiguration(new DevicesInGroupsEntityTypeConfiguration());
            builder.Entity<Group>()
                        .HasOne(x => x.Parent)
                        .WithMany(x => x.Children)
                        .HasForeignKey(x => x.ParentId);
            builder.Entity<ScheduleBase>()
                .HasMany(s => s.Groups)
                .WithOne(g => g.Schedule)
                .HasForeignKey(g => g.ScheduleId);
            builder.Entity<ScheduleBase>()
                .HasMany(s => s.Devices)
                .WithOne(d => d.Schedule)
                .HasForeignKey(d => d.ScheduleId);

            builder.Entity<WeeklySchedule>();
            builder.Entity<YearlySchedule>();
            builder.Entity<GroupScenario>()
                .HasKey(gs => new { gs.GroupId, gs.ScenarioId });
            builder.Entity<GroupScenario>()
            .HasOne(gs => gs.Group)
            .WithMany(g => g.Scenarios)
            .HasForeignKey(gs => gs.GroupId);

            builder.Entity<GroupScenario>()
                .HasOne(gs => gs.Scenario)
                .WithMany(s => s.Groups)
                .HasForeignKey(gs => gs.ScenarioId);
        }
    }
