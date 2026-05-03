using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VendorProcessManagerV1.Models;


namespace VendorProcessManagerV1.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        // public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<AppUser>(options)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
        public DbSet<ProcessTask> ProcessTasks { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ProcessInstance> ProcessInstances { get; set; }
        public DbSet<ProcessTemplate> ProcessTemplates { get; set; }
        public DbSet<ProcessTemplateTask> ProcessTemplatesTasks { get; set; }
        public DbSet<TemplateDependency> TemplateDependencies { get; set; }
        //public DbSet<User> Users {  get; set; }
        public DbSet<VendorCandidate> VendorCandidates { get; set; }
        public DbSet<ProcessTaskTransition> ProcessTaskTransitions { get; set; } = default!;
        public DbSet<ProcessTemplateTransition> ProcessTemplateTransitions { get; set; } = default!;

        // public DbSet<AppUser> AppUsers { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //from task - starting point of the transition
            modelBuilder.Entity<ProcessTemplateTransition>()
                .HasOne(t => t.FromProcessTemplateTask)
                .WithMany(t => t.Transitions)
                .HasForeignKey(t => t.FromProcessTemplateTaskId)
                .OnDelete(DeleteBehavior.Cascade); //delete trnastions when task deleted

            //to task - destination task
            modelBuilder.Entity<ProcessTemplateTransition>()
                .HasOne(t => t.ToProcessTemplateTask)
                .WithMany()
                .HasForeignKey(t => t.ToProcessTemplateTaskId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProcessTemplateTransition>()
                .HasIndex(t => new { t.FromProcessTemplateTaskId, t.IsDefault })
                .IsUnique()
                .HasFilter("[IsDefault] =1");

            modelBuilder.Entity<ProcessInstance>()
                .HasOne(i => i.ProcessTemplate)
                .WithMany()
                .HasForeignKey(i => i.ProcessTemplateId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProcessInstance>()
                .HasOne(i => i.InitiatedBy)
                .WithMany()
                .HasForeignKey(i => i.InitiatedById)
                .OnDelete(DeleteBehavior.NoAction);
                   
            modelBuilder.Entity<ProcessInstance>()
               .HasOne(i => i.VendorCandidate)
               .WithMany(v => v.ProcessInstances)
               .HasForeignKey(i => i.VendorCandidateId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProcessTask>()
                .HasOne(t => t.ProcessInstance)
                .WithMany(i => i.Tasks)
                .HasForeignKey(t => t.ProcessInstanceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProcessTask>()
                .HasOne(t => t.ProcessTemplateTask)
                .WithMany()
                .HasForeignKey(t => t.ProcessTemplateTaskId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProcessTask>()
                .HasOne(t => t.Creator)
                .WithMany()
                .HasForeignKey(t => t.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProcessTask>()
                .HasOne(t => t.Approver)
                .WithMany()
                .HasForeignKey(t => t.ApproverId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProcessTask>()
                .HasOne(t => t.Owner)
                .WithMany()
                .HasForeignKey(t => t.OwnerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProcessTemplate>()
                .HasOne(t => t.Creator)
                .WithMany()
                .HasForeignKey(t => t.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProcessTemplateTask>()
                .HasOne(t => t.ProcessTemplate)
                .WithMany(t => t.Tasks)
                .HasForeignKey(t => t.ProcessTemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProcessTemplateTask>()
                .HasMany(t => t.Transitions)
                .WithOne(t => t.FromProcessTemplateTask)
                .HasForeignKey(t => t.FromProcessTemplateTaskId)
                .OnDelete(DeleteBehavior.Cascade);

            foreach (var fk in modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())
                .Where(fk => !fk.IsOwnership &&
                               fk.DeleteBehavior == DeleteBehavior.Cascade))
            {
                var child = fk.DeclaringEntityType.ClrType.Name;
                var parent = fk.PrincipalEntityType.ClrType.Name;

                var allowedCascades = new[]
                {
                    ("ProcessTask", "ProcessInstance"), 
                    ("ProcessTemplateTask", "ProcessTemplate"), 
                    ("ProcessTemplateTransition", "ProcessTemplateTask")
                };

                var isAllowed = allowedCascades.Any(c =>
                    c.Item1 == child && c.Item2 == parent);
                if (!isAllowed)
                    fk.DeleteBehavior = DeleteBehavior.NoAction; 
            }
        }

    }
}
