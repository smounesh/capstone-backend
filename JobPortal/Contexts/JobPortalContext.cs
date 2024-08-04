using JobPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Contexts
{
    public class JobPortalContext : DbContext
    {
        public JobPortalContext(DbContextOptions<JobPortalContext> options) : base(options)
        {
        }

        // DbSet properties for each model
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<JobPosting> JobPostings { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<ProfileView> ProfileViews { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring relationships and constraints

            // User and Profile relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<Profile>(p => p.UserID);

            // Profile and Experience relationship
            modelBuilder.Entity<Profile>()
                .HasMany(p => p.Experiences)
                .WithOne(e => e.Profile)
                .HasForeignKey(e => e.ProfileID);

            // Profile and Education relationship
            modelBuilder.Entity<Profile>()
                .HasMany(p => p.Educations)
                .WithOne(e => e.Profile)
                .HasForeignKey(e => e.ProfileID);

            // Profile and Skills relationship
            modelBuilder.Entity<Profile>()
                .HasMany(p => p.Skills)
                .WithOne(s => s.Profile)
                .HasForeignKey(s => s.ProfileID);

            // JobPosting and User relationship
            modelBuilder.Entity<JobPosting>()
                .HasOne(j => j.User)
                .WithMany(u => u.JobPostings)
                .HasForeignKey(j => j.PostedBy);

            // Configure timestamps for Profile
            modelBuilder.Entity<Profile>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Profile>()
                .Property(p => p.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAddOrUpdate();

            // Configure timestamps for Experience
            modelBuilder.Entity<Experience>()
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Experience>()
                .Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAddOrUpdate();

            // Configure timestamps for Education
            modelBuilder.Entity<Education>()
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Education>()
                .Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAddOrUpdate();

            // Configure timestamps for ProfileView
            modelBuilder.Entity<ProfileView>()
                .Property(pv => pv.ViewedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();

            // Configure relationships for ProfileView with NO ACTION on delete
            modelBuilder.Entity<ProfileView>()
                .HasOne(pv => pv.Viewer)
                .WithMany()
                .HasForeignKey(pv => pv.ViewerID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProfileView>()
                .HasOne(pv => pv.Profile)
                .WithMany()
                .HasForeignKey(pv => pv.ProfileID)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure timestamps for ChatMessage
            modelBuilder.Entity<ChatMessage>()
                .Property(cm => cm.SentAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();

            // Configure relationships for ChatMessage
            modelBuilder.Entity<ChatMessage>()
                .HasOne(cm => cm.Sender)
                .WithMany()
                .HasForeignKey(cm => cm.SenderID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChatMessage>()
                .HasOne(cm => cm.Recipient)
                .WithMany()
                .HasForeignKey(cm => cm.RecipientID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure relationships for Resume
            modelBuilder.Entity<Resume>()
                .HasOne(r => r.User)
                .WithMany(u => u.Resumes)
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure relationships for JobApplication
            modelBuilder.Entity<JobApplication>()
                .HasOne(ja => ja.JobPosting)
                .WithMany(jp => jp.JobApplications)
                .HasForeignKey(ja => ja.JobPostingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobApplication>()
                .HasOne(ja => ja.User)
                .WithMany(u => u.JobApplications)
                .HasForeignKey(ja => ja.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure default value for ApplicationDate in JobApplication
            modelBuilder.Entity<JobApplication>()
                .Property(ja => ja.ApplicationDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();
        }
    }
}
