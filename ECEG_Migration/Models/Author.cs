using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECEG_Migration.Models
{
    public class Author
    {
        private int author_id;
        private String name;
        private String gender;
        private int gender_id;
        private String city_name;
        private int city_id;
        private String county_name;
        private int county_id;
        private String country_name;
        private int country_id;
        private Occupation[] occupations;
        private String biographical_details;

        public int Author_id { get => author_id; set => author_id = value; }
        public string Name { get => name; set => name = value; }
        public string Gender { get => gender; set => gender = value; }
        public int Gender_id { get => gender_id; set => gender_id = value; }
        public string City_name { get => city_name; set => city_name = value; }
        public int City_id { get => city_id; set => city_id = value; }
        public string County_name { get => county_name; set => county_name = value; }
        public int County_id { get => county_id; set => county_id = value; }
        public string Country_name { get => country_name; set => country_name = value; }
        public int Country_id { get => country_id; set => country_id = value; }
        public string Biographical_details { get => biographical_details; set => biographical_details = value; }
        internal Occupation[] Occupations { get => occupations; set => occupations = value; }
    }
}