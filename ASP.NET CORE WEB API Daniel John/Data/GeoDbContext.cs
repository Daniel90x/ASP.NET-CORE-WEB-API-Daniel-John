using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ASP.NET_CORE_WEB_API_Daniel_John.Models;
using ASP.NET_CORE_WEB_API_Daniel_John.Models.V2;


namespace ASP.NET_CORE_WEB_API_Daniel_John.Data
{
    public class GeoDbContext : /* DbContext */ IdentityDbContext<MyUser> 
    {
        public GeoDbContext(DbContextOptions<GeoDbContext> options)
            : base(options)
        {
        }

       /*  protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GeoMessage>().Ignore(g => g.Message);
        } */

        public DbSet<MyUser> User { get; set; }
        public DbSet<GeoMessage> GeoMessage { get; set; }

        public async Task Seed(IServiceProvider provider)
        {
            var context = provider.GetRequiredService<GeoDbContext>();
            await this.Database.EnsureDeletedAsync();
            await this.Database.EnsureCreatedAsync();

            var userManager = provider.GetRequiredService<UserManager<MyUser>>();

            GeoMessage.AddRange(new List<GeoMessage>()
            {
                new GeoMessage(){Message = "Hejsan", Longitude = 25, Latitude = 33},
                new GeoMessage(){Message = "Kan du ha det bra?", Longitude = 77, Latitude = 12},

            });

             await userManager.CreateAsync(
                new MyUser
                {
                    UserName = "Tester",
                    FirstName = "Hans",
                    LastName = "Svensson"
                },

                "Tester_123"); 



            await SaveChangesAsync();
        }




    }

}
