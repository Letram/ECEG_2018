using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECEG_Migration.Models
{
    public class Imprint
    {
        private int grammar_id;
        private string city_name;
        private int city_id;
        private string county_name;
        private int county_id;
        private string country_name;
        private int country_id;
        private string printers;
        private string booksellers;
        private string price;
        private string description;

        public string City_name { get => city_name; set => city_name = value; }
        public int City_id { get => city_id; set => city_id = value; }
        public string County_name { get => county_name; set => county_name = value; }
        public int County_id { get => county_id; set => county_id = value; }
        public string Country_name { get => country_name; set => country_name = value; }
        public string Printers { get => printers; set => printers = value; }
        public string Booksellers { get => booksellers; set => booksellers = value; }
        public string Price { get => price; set => price = value; }
        public string Description { get => description; set => description = value; }
        public int Grammar_id { get => grammar_id; set => grammar_id = value; }
        public int Country_id { get => country_id; set => country_id = value; }
    }
}