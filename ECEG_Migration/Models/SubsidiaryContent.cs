using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECEG_Migration.Models
{
    public class SubsidiaryContent
    {
        private int grammar_id;
        private int sub_content_id;
        private string sub_content_name;

        public int Sub_content_id { get => sub_content_id; set => sub_content_id = value; }
        public string Sub_content_name { get => sub_content_name; set => sub_content_name = value; }
        public int Grammar_id { get => grammar_id; set => grammar_id = value; }
    }
}