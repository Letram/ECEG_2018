using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECEG_Migration.Models
{
    public class City
    {
        private int city_id;
        private String city_name;
        private int county_id;

        public int City_id { get => city_id; set => city_id = value; }
        public string City_name { get => city_name; set => city_name = value; }
        public int County_id { get => county_id; set => county_id = value; }
    }
}