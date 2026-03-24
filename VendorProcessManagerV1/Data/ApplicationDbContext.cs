using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VendorProcessManagerV1.Models;
//using Task = VendorProcessManagerV1.Models.Task; //to prevent ambiguiity

namespace VendorProcessManagerV1.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<ProcessTask> ProcessTasks { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ProcessInstance> ProcessInstances { get; set; }
        public DbSet<ProcessTemplate> ProcessTemplates { get; set; }
        public DbSet<ProcessTemplateTask> ProcessTemplatesTasks { get; set; }
        public DbSet<TemplateDependency> TemplateDependencies { get; set; }
        public DbSet<User> Users {  get; set; }
        public DbSet<VendorCandidate> VendorCandidates { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
       
    }
}
