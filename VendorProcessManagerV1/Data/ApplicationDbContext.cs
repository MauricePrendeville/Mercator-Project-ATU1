using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VendorProcessManagerV1.Models;


namespace VendorProcessManagerV1.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
   // public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<AppUser>(options)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options) 
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
        public DbSet<ProcessTaskTransition> ProcessTaskTransition { get; set; } = default!;
        public DbSet<ProcessTemplateTransition> ProcessTemplateTransition { get; set; } = default!;
        
       // public DbSet<AppUser> AppUsers { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
       
    }
}
