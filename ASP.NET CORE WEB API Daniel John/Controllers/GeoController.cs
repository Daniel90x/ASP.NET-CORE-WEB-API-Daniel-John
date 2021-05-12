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

                if (test.Message == null)
                {

                    var geoV1 = new Models.V1.GeoMessageDTO
                    {
                        Message = test.Body,         
                        Longitude = test.Longitude,
                        Latitude = test.Latitude

                    };

                    return geoV1;
                }
                else
                 {

                    var geoV1 = new Models.V1.GeoMessageDTO
                    {
                        Message = test.Message,         
                        Longitude = test.Longitude,
                        Latitude = test.Latitude

                    };
                    return geoV1;
                }
            }


            [HttpGet]
            public async Task<ActionResult<IEnumerable<Models.V1.GeoMessageDTO>>> GetGeoAll()
            {
              //  if(test.message == null)   
                var AllGeo = await _context.GeoMessage.Select(g => g.ToDto()).ToListAsync();
                var GeoList = new List<Models.V1.GeoMessageDTO>();

                foreach (var item in AllGeo)
                {
                    //var Result = ToMessage(item);
                    var AllGeoV1 = V2GeoMessageDTOToV1(item);
                    if (item.Message == null)
                    {
                        GeoList.Add(new Models.V1.GeoMessageDTO
                        {
                            Latitude = AllGeoV1.Latitude,
                            Longitude = AllGeoV1.Longitude,
                            Message = item.Body

                        });
                    }

                    else
                    {
                        GeoList.Add(new Models.V1.GeoMessageDTO
                        {
                            Latitude = AllGeoV1.Latitude,
                            Longitude = AllGeoV1.Longitude,
                            Message = AllGeoV1.Message

                        });
                    }
                }
                return GeoList;
            }

           
            public static Models.V1.GeoMessageDTO V2GeoMessageDTOToV1(Models.V2.GeoMessageDTO geoV2)
            {
                var geoV1 = new Models.V1.GeoMessage
                {
                    Message = geoV2.Message,          
                    Longitude = geoV2.Longitude,
                    Latitude = geoV2.Latitude

                };
                return geoV1;
            }
            public static Models.V2.GeoMessage V1GeoMessageDTOToV2(Models.V1.GeoMessageDTO geoV1)
            {
                var geoV2 = new Models.V2.GeoMessage
                {
                    Message = geoV1.Message,         
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


                _context.GeoMessage.Add(geoV2); 
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

                if (test.Body == null)
                {
                    var message = new Message
                    {
                        Title = test.Title,
                        Body = test.Message,
                        Author = test.Author
                    };

                    return new Return
                    {
                        Message = message,
                        Latitude = test.Latitude,
                        Longitude = test.Longitude,
                    };
                }
                else
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

                        if (Result.Message.Body == null)
                        {
                            Result.Message.Body = item.Message;
                            GeoList.Add(new Return
                            {
                                Latitude = Result.Latitude,
                                Longitude = Result.Longitude,
                                Message = Result.Message

                            });
                        }

                        else
                        {
                            GeoList.Add(new Return
                            {
                                Latitude = Result.Latitude,
                                Longitude = Result.Longitude,
                                Message = Result.Message

                            });
                        }

                    }
                    return GeoList;
                }
                var AllGeo2 = await _context.GeoMessage.Where(g => g.Longitude >= MinLongitude && g.Longitude <= MaxLongitude
                && g.Latitude >= MinLatitude && g.Latitude <= MaxLatitude).ToListAsync();

                var GeoList2 = new List<Return>();

                foreach (var item in AllGeo2)
                {
                    var Result = ToMessage(item);

                    if (Result.Message.Body == null)
                    {
                        Result.Message.Body = item.Message;
                        GeoList2.Add(new Return
                        {
                            Latitude = Result.Latitude,
                            Longitude = Result.Longitude,
                            Message = Result.Message

                        });
                    }

                    else
                    {
                        GeoList2.Add(new Return
                        {
                            Latitude = Result.Latitude,
                            Longitude = Result.Longitude,
                            Message = Result.Message

                        });
                    }
                }
                return GeoList2;
            }

            [Authorize]
            [HttpPost]
            public async Task<ActionResult<ReturnNoAuthor>> PostGeoMessage(ReturnNoAuthor geoPost/*, UserManager<MyUser> userManager*/)
            {


                var userName = User.Identity.Name;

                _context.GeoMessage.Add(new Models.V2.GeoMessage {
                    Author = userName,
                    Title = geoPost.Message.Title,          // fyll body med message och tvärtom
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




            public class ReturnNoAuthor
            {
                public MessageNoAuthor Message { get; set; }
                public double Longitude { get; set; }
                public double Latitude { get; set; }
            }

            public class MessageNoAuthor
            {
                public string Title { get; set; }
                public string Body { get; set; }
               
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
