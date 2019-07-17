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

        public string Code { get => code; set => code = value; }
        public string Library_name { get => library_name; set => library_name = value; }
    }
}