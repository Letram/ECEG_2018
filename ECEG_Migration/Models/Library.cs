using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECEG_Migration.Models
{
    public class Library
    {
        private string code;
        private string library_name;
        private int grammar_id;
        public string Code { get => code; set => code = value; }
        public string Library_name { get => library_name; set => library_name = value; }
        public int Grammar_id { get => grammar_id; set => grammar_id = value; }
    }
}