using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECEG_Migration.Models
{
    public class County
    {
        private int county_id;
        private String county_name;
        private int country_id;

        public int County_id { get => county_id; set => county_id = value; }
        public string County_name { get => county_name; set => county_name = value; }
        public int Country_id { get => country_id; set => country_id = value; }
    }
}