using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Models;

namespace TicketSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<Reply> Reply { get; set; }
        public DbSet<MailTicket> MailTicket { get; set; }
        public DbSet<Bug> Bug { get; set; }
        public DbSet<APIKey> APIKey { get; set; }
        public DbSet<Companies> Company { get; set; }
        public DbSet <Contacts> Contacts { get; set; }
        public DbSet<UserInCompany> UserInCompany { get; set; }
        public DbSet<KnowledgeMap> KnowledgeMap { get; set; }
        public DbSet<Article> Article { get; set; }
        public DbSet<TargetDirectory> TargetDirectory { get; set; }
        public DbSet<DMS> DMS { get; set; }
        public DbSet<Categories> Categories { get; set; }

        public DbSet<Notifications> Notifications { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
