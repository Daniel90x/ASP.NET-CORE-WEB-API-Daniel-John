using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ASP.NET_CORE_WEB_API_Daniel_John.Models
{
    namespace V2
    {

        public class GeoMessage : GeoMessageDTO
        {
            public int Id { get; set; }
             
        }




        public class GeoMessageDTO
        {
            public double Longitude { get; set; }
            public double Latitude { get; set; }  
            public string Title { get; set; }
            public string Author { get; set; }
            public string Body { get; set; }   




       /*   public class MessageDTOv2
            {
                string Title;
                string Body;
                string Author;
            }

            public class GeoMessageDTO2
            {

                MessageDTOv2 Message;
                double Longitude;
                double Latitude;
            }     */

            public GeoMessage GeoMessageModel()
            {





                /*var test = new GeoMessage
                {
                    Title = this.Title,
                    Author = this.Author,
                    Body = this.Body
                };*/
                return new GeoMessage
                {








                 
                    Longitude = this.Longitude,
                    Latitude = this.Latitude,
                    Title = this.Title,
                    Author = this.Author,
                    Body = this.Body
                };
            }
            public GeoMessageDTO ToDto()
            {
                return this;
            }

         /*   public GeoMessage GeoMessageModel2()
            {
                return new GeoMessage
                {
                    Longitude = this.Longitude,
                    Latitude = this.Latitude, 
                };
            }
            public GeoMessageDTO ToDto2()
            {
                return this;
            } */
        }

    }
    
    namespace V1
    {

        public class GeoMessage : GeoMessageDTO
        {
            public int Id { get; set; }
        }

        public class GeoMessageDTO
        {

            public string Message { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }


            public GeoMessage GeoMessageModel()
            {
                return new GeoMessage
                {
                    Message = this.Message,
                    Longitude = this.Longitude,
                    Latitude = this.Latitude,
                };
            }
            public GeoMessageDTO ToDto()
            {
                return this;
            }
        }

    }


}
    


