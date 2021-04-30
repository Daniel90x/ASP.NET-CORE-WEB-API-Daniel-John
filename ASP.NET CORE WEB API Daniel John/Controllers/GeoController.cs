using ASP.NET_CORE_WEB_API_Daniel_John.Data;
using ASP.NET_CORE_WEB_API_Daniel_John.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_WEB_API_Daniel_John.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeoController : ControllerBase
    {
        private readonly GeoDbContext _context;

        public GeoController(GeoDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GeoMessage>> GetGeo(int id)
        {
            var test = await _context.GeoMessage.FindAsync(id);
            if(test == null)
            {
                return NotFound();
            }
            return test;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeoMessage>>> GetGeoAll()
        {
            var test = await _context.GeoMessage.Where(g => g.Id > 0).ToListAsync();
            if (test == null)
            {
                return NotFound();
            }
            return test;
        }


        /*[HttpPost] */

    }
}
