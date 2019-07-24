using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECEG_Migration.Models
{
    public class Edition
    {
        private int grammar_id;
        private int edition_number;
        private string edition_year;
        private string printing_place;
        private string description;

        public int Grammar_id { get => grammar_id; set => grammar_id = value; }
        public int Edition_number { get => edition_number; set => edition_number = value; }
        public string Edition_year { get => edition_year; set => edition_year = value; }
        public string Printing_place { get => printing_place; set => printing_place = value; }
        public string Description { get => description; set => description = value; }
    }
}