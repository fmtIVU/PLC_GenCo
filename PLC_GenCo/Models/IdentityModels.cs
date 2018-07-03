using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PLC_GenCo.Models;
using PLC_GenCo.Models.Setups;

namespace PLC_GenCo.ViewModels
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Standard> Standards { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<ComponentLocation> ComponentLocations { get; set; }
        public DbSet<IO> IOs { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<PLC> PLC { get; set; }
        public DbSet<DIAlarmSetup> DIAlarms { get; set; }
        public DbSet<DIPulseSetup> DIpulses { get; set; }
        public DbSet<AIAlarmSetup> AIAlarms { get; set; }
        public DbSet<MDirSetup> MDirs { get; set; }



        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}