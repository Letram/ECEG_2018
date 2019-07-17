using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECEG_Migration.Models
{
    public class TypeOfWork
    {
        private string code;
        private string type_description;

        public string Code { get => code; set => code = value; }
        public string Type_description { get => type_description; set => type_description = value; }
    }
}