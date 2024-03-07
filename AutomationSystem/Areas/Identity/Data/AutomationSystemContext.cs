using AutomationSystem.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AutomationSystem.Data;

public class AutomationSystemContext : IdentityDbContext<AutomationSystemUser>
{
    public AutomationSystemContext(DbContextOptions<AutomationSystemContext> options)
        : base(options)
    {
    }
    public DbSet<AutomationSystemUser> AutomationSystemUsers { get; set; }
    public DbSet<FixCategory> FixCategories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Technik> Techniks { get; set; }
    public DbSet<MasterData> MasterDatas { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
