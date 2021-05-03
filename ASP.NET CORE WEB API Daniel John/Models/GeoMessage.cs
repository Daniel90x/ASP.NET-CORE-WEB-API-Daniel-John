﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_WEB_API_Daniel_John.Models
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

// Hej test 