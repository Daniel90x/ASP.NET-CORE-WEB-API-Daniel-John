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
    public class GeoDbContext : DbContext
    {
        public GeoDbContext (DbContextOptions<GeoDbContext> options)
            : base(options)
        {
        }

        public async Task Seed ()
        {
            // var context = GeoDbContext();
            await this.Database.EnsureDeletedAsync();
            await this.Database.EnsureCreatedAsync();


            GeoMessage.AddRange(new List<GeoMessage>()
            {
                new GeoMessage(){Message = "Hejsan", Longitude = 25, Latitude = 33},
                new GeoMessage(){Message = "Kan du ha det bra?", Longitude = 77, Latitude = 12},

            });


            await SaveChangesAsync();
        }
        public DbSet<GeoMessage> GeoMessage { get; set; }

        


    }
}
