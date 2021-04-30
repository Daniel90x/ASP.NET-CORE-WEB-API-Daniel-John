using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
                new GeoMessage(){message = "Hejsan", longitude = 25, latitude = 33},
                new GeoMessage(){message = "KAn du ha det bra?", longitude = 77, latitude = 12},

            });


            await SaveChangesAsync();
        }
        public DbSet<GeoMessage> GeoMessage { get; set; }

        


    }
}
