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
using Newtonsoft.Json;



namespace ASP.NET_CORE_WEB_API_Daniel_John.Controllers
{


    namespace V1
    {
        [ApiController]
        [ApiVersion("1.0")]
        [Route("api/v{Version:ApiVersion}/[controller]-comments")]
        public class GeoController : ControllerBase
        {
            private readonly GeoDbContext _context;

            public GeoController(GeoDbContext context)
            {
                _context = context;
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<Models.V1.GeoMessageDTO>> GetGeo(int id) 
            {
                var test = await _context.GeoMessage.FindAsync(id);
                if (test == null)
                {
                    return NotFound();
                }
                var geoV1 = new Models.V1.GeoMessageDTO {
                    //Message = test.Message,
                    Longitude = test.Longitude,
                    Latitude = test.Latitude
                    
                };
                return geoV1;
            }


            [HttpGet]
            public async Task<ActionResult<IEnumerable<Models.V1.GeoMessageDTO>>> GetGeoAll()
            {

                
                return await _context.GeoMessage.Select(g => V2GeoMessageDTOToV1(g.ToDto())).ToListAsync();
            }

            public static Models.V1.GeoMessageDTO V2GeoMessageDTOToV1(Models.V2.GeoMessageDTO geoV2)
            {
                var geoV1 = new Models.V1.GeoMessage
                {
                    //Message = geoV2.Message,
                    Longitude = geoV2.Longitude,
                    Latitude = geoV2.Latitude

                };
                return geoV1;
            }
            public static Models.V2.GeoMessage V1GeoMessageDTOToV2(Models.V1.GeoMessageDTO geoV1)
            {
                var geoV2 = new Models.V2.GeoMessage
                {
                    //Message = geoV1.Message,
                    Longitude = geoV1.Longitude,
                    Latitude = geoV1.Latitude

                };
                return geoV2;
            }

            [Authorize]
            [HttpPost]
            public async Task<ActionResult<Models.V1.GeoMessageDTO>> PostGeoMessage(Models.V1.GeoMessageDTO geoPost)
            {

                var geo = geoPost.GeoMessageModel();
                var geoV2 = V1GeoMessageDTOToV2(geoPost);


                _context.GeoMessage.Add(geoV2); // WiP 
                await _context.SaveChangesAsync();
                return Ok();

            }

        }
    }


    namespace v2
    {

        [ApiController]
        [ApiVersion("2.0")]
        [Route("api/v{Version:ApiVersion}/[controller]-comments")]
        public class GeoController : ControllerBase
        {
            private readonly GeoDbContext _context;

            public GeoController(GeoDbContext context)
            {
                _context = context;
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<Models.V2.GeoMessage>> GetGeo(int id)
            {
                var test = await _context.GeoMessage.FindAsync(id);
                if (test == null)
                {
                    return NotFound();
                }
                /*List<string> testar = new List<string>();
                testar.Add(test.Body);
                testar.Add(test.Author);
                testar.Add(test.Title);
                test.Message = testar;*/


                var testar = new Models.V2.GeoMessage();
       //         testar.Message = test.Title + test.Body + test.Author;
                testar.Latitude = test.Latitude;
                testar.Longitude = test.Longitude;
                string json = JsonConvert.SerializeObject(testar);

                return testar;
            }


            [HttpGet]
            public async Task<ActionResult<IEnumerable<Models.V2.GeoMessageDTO>>> GetGeoAll()
            {


                return await _context.GeoMessage.Select(g => g.ToDto()).ToListAsync();
            }

            [Authorize]
            [HttpPost]
            public async Task<ActionResult<Models.V2.GeoMessage>> PostGeoMessage(Models.V2.GeoMessageDTO geoPost)
            {

                var geo = geoPost.GeoMessageModel();
                _context.GeoMessage.Add(geo);
                await _context.SaveChangesAsync();

                return Ok();

            }

        }







    }
}
