using BCI.Api.Models;
using BCI.Api.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BCI.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CompanyProducts>()
                .HasKey(al => new { al.CompanyId, al.ProductId });

            
        }

        public DbSet<ActivationRequest> ActivationRequest { get; set; }
        public DbSet<Trace> Trace { get; set; }
        public DbSet<Poll> Poll { get; set; }
        public DbSet<CompanyProducts>  CompanyProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SalesAmount> SalesAmount { get; set; }
        public DbSet<ProcessLog> ProcessLog { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ProcessSave();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ProcessSave()
        {
            foreach (var auditableEntity in ChangeTracker.Entries<IAuditable>())
            {
                if (auditableEntity.State == EntityState.Added ||
                    auditableEntity.State == EntityState.Modified)
                {
                    auditableEntity.Entity.Modified = DateTime.Now;

                    if (auditableEntity.State == EntityState.Added)
                    {
                        auditableEntity.Entity.Created = DateTime.Now;
                    }
                }
            }
        }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@Directory.GetCurrentDirectory() + "/../BCI.Apis/appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("defaultConnectionString");
            builder.UseSqlServer(connectionString);
            return new ApplicationDbContext(builder.Options);
        }
    }    
}