using ASP.NET_CORE_WEB_API_Daniel_John.Data;
using ASP.NET_CORE_WEB_API_Daniel_John.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace ASP.NET_CORE_WEB_API_Daniel_John.Controllers
{


    namespace V1
    {
        [ApiController]
        [ApiVersion("1.0")]
        [Route("api/v{Version:ApiVersion}/[controller]-comments")] //[Version:ApiVersion]
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
                if (test == null)
                {
                    return NotFound();
                }
                return test;
            }


            [HttpGet]
            public async Task<ActionResult<IEnumerable<GeoMessageDTO>>> GetGeoAll()
            {


                return await _context.GeoMessage.Select(g => g.ToDto()).ToListAsync();
            }

            [Authorize]
            [HttpPost]
            public async Task<ActionResult<GeoMessageDTO>> PostGeoMessage(GeoMessageDTO geoPost)
            {

                var geo = geoPost.GeoMessageModel();
                _context.GeoMessage.Add(geo);
                await _context.SaveChangesAsync();

                return Ok();

            }

        }
    }
}
