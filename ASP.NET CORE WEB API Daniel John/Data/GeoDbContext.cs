using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ASP.NET_CORE_WEB_API_Daniel_John.Models;

namespace ASP.NET_CORE_WEB_API_Daniel_John.Data
{
    public class GeoDbContext : /* DbContext */ IdentityDbContext<MyUser> 
    {
        public GeoDbContext(DbContextOptions<GeoDbContext> options)
            : base(options)
        {
        }

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

                "Tester123\"#"); 



            await SaveChangesAsync();
        }




    }

    /*
    public class UserDbContext : IdentityDbContext<IdentityUser>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

    

        
        public static async Task Reset(IServiceProvider provider)
        {
            var context = provider.GetRequiredService<UserDbContext>();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            var userManager = provider.GetRequiredService<UserManager<IdentityUser>>();

            await userManager.CreateAsync(
                new IdentityUser
                {
                    UserName = "TestUser"
                },
                "QWEqwe123!\"#");
        } 
    } */
}
