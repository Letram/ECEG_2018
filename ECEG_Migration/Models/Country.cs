using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECEG_Migration.Models
{
    public class Country
    {
        private int country_id;
        private String country_name;

        public int Country_id { get => country_id; set => country_id = value; }
        public string Country_name { get => country_name; set => country_name = value; }
    }
}