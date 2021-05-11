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
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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
                    Message = test.Message,         // maybe
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
                    Message = geoV2.Message,          // maybe
                    Longitude = geoV2.Longitude,
                    Latitude = geoV2.Latitude

                };
                return geoV1;
            }
            public static Models.V2.GeoMessage V1GeoMessageDTOToV2(Models.V1.GeoMessageDTO geoV1)
            {
                var geoV2 = new Models.V2.GeoMessage
                {
                    Message = geoV1.Message,         // Maybe?
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
            private readonly UserManager<MyUser> _userManager;


            public MyUser MyUser { get; set; }

    

            public GeoController(GeoDbContext context, UserManager<MyUser> userManager)
            {
                _context = context;
                _userManager = userManager;
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<Return>> GetGeo(int id)
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


                /*var testar = new Models.V2.GeoMessage();
                testar.Message = test.Title + test.Body + test.Author;
                testar.Latitude = test.Latitude;
                testar.Longitude = test.Longitude;
                string json = JsonConvert.SerializeObject(testar);

                return testar; */

                var message = new Message
                {
                    Title = test.Title,
                    Body = test.Body,
                    Author = test.Author
                };

                return new Return
                {
                    Message = message,
                    Latitude = test.Latitude,
                    Longitude = test.Longitude,
                };

            }
            
            

            [HttpGet]
            public async Task<ActionResult<List<Return>>> GetGeoAll
                (double? MaxLongitude, double? MinLongitude, double? MaxLatitude, double? MinLatitude)
            {
                if (MaxLatitude == null || MinLatitude == null || MaxLongitude == null || MinLongitude == null)
                {
                    var AllGeo = await _context.GeoMessage.Select(g => g.ToDto()).ToListAsync();

                    var GeoList = new List<Return>();

                    foreach (var item in AllGeo)
                    {
                        var Result = ToMessage(item);
                        GeoList.Add(new Return
                        {
                            Latitude = Result.Latitude,
                            Longitude = Result.Longitude,
                            Message = Result.Message

                        });

                    }
                    return GeoList;
                }
                var AllGeo2 = await _context.GeoMessage.Where(g => g.Longitude >= MinLongitude && g.Longitude <= MaxLongitude
                && g.Latitude >= MinLatitude && g.Latitude <= MaxLatitude).ToListAsync();

                var GeoList2 = new List<Return>();

                foreach (var item in AllGeo2)
                {
                    var Result = ToMessage(item);
                    GeoList2.Add(new Return
                    {
                        Latitude = Result.Latitude,
                        Longitude = Result.Longitude,
                        Message = Result.Message

                    });
                }
                return GeoList2;
            }

            [Authorize]
            [HttpPost]
            public async Task<ActionResult<Return>> PostGeoMessage(Return geoPost/*, UserManager<MyUser> userManager*/)
            {

                //var user = await _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefaultAsync();
                // MyUser = user;

                //var userId = User.Claims.FirstOrDefault(c => c.Type  == ClaimTypes.Name).Value;
                //var user = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();


                /*       string id;
                       id = User.Identity.GetUserId();
                       id = RequestContext.Principal.Identity.GetUserId(); */
                var test = User.Identity.Name;
                var claim = User.FindFirst(ClaimTypes.NameIdentifier);




                _context.GeoMessage.Add(new Models.V2.GeoMessage {
                    Author = test,
                    Title = geoPost.Message.Title,
                    Body = geoPost.Message.Body,
                    Latitude = geoPost.Latitude,
                    Longitude = geoPost.Longitude

                });
                await _context.SaveChangesAsync();

                return Ok();

            }


            public class Return
            {
                public Message Message { get; set; }
                public double Longitude { get; set; }
                public double Latitude { get; set; }
            }

            public class Message
            {
                public string Title { get; set; }
                public string Body { get; set; }
                public string Author { get; set; }
            }



            public static Return ToMessage(Models.V2.GeoMessageDTO test)
            {
                var message = new Message
                {
                    Title = test.Title,
                    Body = test.Body,
                    Author = test.Author
                };

                return new Return
                {
                    Message = message,
                    Latitude = test.Latitude,
                    Longitude = test.Longitude,
                };
            }




        }







    }
}
