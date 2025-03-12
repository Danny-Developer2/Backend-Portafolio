using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using API.Entities;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        // Definición de las tablas
        public DbSet<AppUser> Users { get; set; }
        public DbSet<AppSkill> Skills { get; set; }
        public DbSet<AppExperience> Experiences { get; set; }
        public DbSet<AppConcactMessage> ContactMessages { get; set; }
        public DbSet<AppProyect> Projects { get; set; }



        public DbSet<UserSkill> UserSkills { get; set; }

        public DbSet<ProyectExperience> ProyectExperience { get; set; }

        public DbSet<UserProyect> UserProyects { get; set; }


        public DbSet<ProyectSkill> ProjectSkills { get; set; }

        public DbSet<ActiveSession> ActiveSessions { get; set; }

        



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>()
     .HasMany(u => u.Projects)
     .WithMany(p => p.Users)
     .UsingEntity<UserProyect>(j => j.ToTable("UserProyects"));

            modelBuilder.Entity<UserProyect>()
                .HasKey(up => new { up.UserId, up.ProjectId });

            modelBuilder.Entity<UserProyect>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserProyects)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserProyect>()
                .HasOne(up => up.Project)
                .WithMany(p => p.UserProyects)
                .HasForeignKey(up => up.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserSkill>()
                .HasKey(us => new { us.UserId, us.SkillId });

            modelBuilder.Entity<UserSkill>()
                .HasOne(us => us.User)
                .WithMany(u => u.UserSkills)
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserSkill>()
                .HasOne(us => us.Skill)
                .WithMany(s => s.UserSkills)
                .HasForeignKey(us => us.SkillId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<ProyectSkill>()
.HasKey(ps => new { ps.ProjectId, ps.SkillId });

            modelBuilder.Entity<ProyectSkill>()
                .HasOne(ps => ps.Project)
                .WithMany(p => p.ProyectSkills)
                .HasForeignKey(ps => ps.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProyectSkill>()
                .HasOne(ps => ps.Skill)
                .WithMany(s => s.ProyectSkills)
                .HasForeignKey(ps => ps.SkillId)
                .OnDelete(DeleteBehavior.Cascade);






            base.OnModelCreating(modelBuilder);
        }





    }
}
