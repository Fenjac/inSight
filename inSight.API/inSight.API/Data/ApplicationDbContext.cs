using Microsoft.EntityFrameworkCore;
using inSight.API.Models;

namespace inSight.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets - tabele u bazi

        // Core entities
        public DbSet<User> Users { get; set; }
        public DbSet<SystemRole> SystemRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectAssignment> ProjectAssignments { get; set; }
        public DbSet<UserSupervisor> UserSupervisors { get; set; }

        // Evaluation entities
        public DbSet<Quarter> Quarters { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<EvaluationCategory> EvaluationCategories { get; set; }
        public DbSet<EvaluationQuestion> EvaluationQuestions { get; set; }
        public DbSet<EvaluationAnswer> EvaluationAnswers { get; set; }
        public DbSet<EvaluationProject> EvaluationProjects { get; set; }

        // Management entities
        public DbSet<ManagementReview> ManagementReviews { get; set; }
        public DbSet<LeaderJournal> LeaderJournals { get; set; }
        public DbSet<HROneOnOne> HROneOnOnes { get; set; }

        // History and configuration
        public DbSet<RankHistory> RankHistories { get; set; }
        public DbSet<QuarterlyScore> QuarterlyScores { get; set; }
        public DbSet<RankConfiguration> RankConfigurations { get; set; }
        public DbSet<EmailNotification> EmailNotifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== USER RELATIONSHIPS =====

            // User -> SystemRole
            modelBuilder.Entity<User>()
                .HasOne(u => u.SystemRole)
                .WithMany(sr => sr.Users)
                .HasForeignKey(u => u.SystemRoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> Role (Job Role)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> Rank
            modelBuilder.Entity<User>()
                .HasOne(u => u.CurrentRank)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.CurrentRankId)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> Team
            modelBuilder.Entity<User>()
                .HasOne(u => u.Team)
                .WithMany(t => t.Members)
                .HasForeignKey(u => u.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            // User evaluations
            modelBuilder.Entity<User>()
                .HasMany(u => u.EvaluationsAsEvaluated)
                .WithOne(e => e.EvaluatedUser)
                .HasForeignKey(e => e.EvaluatedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.EvaluationsAsEvaluator)
                .WithOne(e => e.EvaluatorUser)
                .HasForeignKey(e => e.EvaluatorUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== TEAM RELATIONSHIPS =====

            // Team -> TeamLead
            modelBuilder.Entity<Team>()
                .HasOne(t => t.TeamLead)
                .WithMany(u => u.TeamsLed)
                .HasForeignKey(t => t.TeamLeadId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== RANK RELATIONSHIPS =====

            // Rank -> Role
            modelBuilder.Entity<Rank>()
                .HasOne(r => r.Role)
                .WithMany(r => r.Ranks)
                .HasForeignKey(r => r.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // RankConfiguration -> Rank
            modelBuilder.Entity<RankConfiguration>()
                .HasOne(rc => rc.Rank)
                .WithMany()
                .HasForeignKey(rc => rc.RankId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== PROJECT RELATIONSHIPS =====

            // ProjectAssignment relationships
            modelBuilder.Entity<ProjectAssignment>()
                .HasOne(pa => pa.User)
                .WithMany(u => u.ProjectAssignments)
                .HasForeignKey(pa => pa.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectAssignment>()
                .HasOne(pa => pa.Project)
                .WithMany(p => p.Assignments)
                .HasForeignKey(pa => pa.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== USER SUPERVISOR RELATIONSHIPS =====

            // UserSupervisor - Employee side
            modelBuilder.Entity<UserSupervisor>()
                .HasOne(us => us.User)
                .WithMany(u => u.Supervisors)
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // UserSupervisor - Supervisor side
            modelBuilder.Entity<UserSupervisor>()
                .HasOne(us => us.Supervisor)
                .WithMany(u => u.Subordinates)
                .HasForeignKey(us => us.SupervisorId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== EVALUATION PROJECT RELATIONSHIPS =====

            // EvaluationProject relationships
            modelBuilder.Entity<EvaluationProject>()
                .HasOne(ep => ep.Evaluation)
                .WithMany(e => e.EvaluationProjects)
                .HasForeignKey(ep => ep.EvaluationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EvaluationProject>()
                .HasOne(ep => ep.Project)
                .WithMany(p => p.EvaluationProjects)
                .HasForeignKey(ep => ep.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== OTHER RELATIONSHIPS =====

            // LeaderJournal relationships
            modelBuilder.Entity<LeaderJournal>()
                .HasOne(lj => lj.Leader)
                .WithMany()
                .HasForeignKey(lj => lj.LeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LeaderJournal>()
                .HasOne(lj => lj.Employee)
                .WithMany()
                .HasForeignKey(lj => lj.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // HROneOnOne relationships
            modelBuilder.Entity<HROneOnOne>()
                .HasOne(hr => hr.Employee)
                .WithMany()
                .HasForeignKey(hr => hr.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HROneOnOne>()
                .HasOne(hr => hr.HRRepresentative)
                .WithMany()
                .HasForeignKey(hr => hr.HRRepresentativeId)
                .OnDelete(DeleteBehavior.Restrict);

            // RankHistory relationships
            modelBuilder.Entity<RankHistory>()
                .HasOne(rh => rh.User)
                .WithMany()
                .HasForeignKey(rh => rh.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RankHistory>()
                .HasOne(rh => rh.ChangedBy)
                .WithMany()
                .HasForeignKey(rh => rh.ChangedById)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== INDEXES FOR PERFORMANCE =====

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Quarter>()
                .HasIndex(q => new { q.Year, q.QuarterNumber })
                .IsUnique();

            modelBuilder.Entity<RankConfiguration>()
                .HasIndex(rc => new { rc.RankId, rc.EffectiveFrom });

            modelBuilder.Entity<Team>()
                .HasIndex(t => t.Name)
                .IsUnique();

            modelBuilder.Entity<Project>()
                .HasIndex(p => p.Name);

            modelBuilder.Entity<SystemRole>()
                .HasIndex(sr => sr.Name)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique();

            modelBuilder.Entity<Rank>()
                .HasIndex(r => new { r.RoleId, r.Code })
                .IsUnique();
        }
    }
}