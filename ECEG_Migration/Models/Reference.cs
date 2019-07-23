using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECEG_Migration.Models
{
    public class Reference
    {
        private string description;
        private int reference_id;
        private int group;
        private int grammar_id;

        public string Description { get => description; set => description = value; }
        public int Reference_id { get => reference_id; set => reference_id = value; }
        public int Group { get => group; set => group = value; }
        public int Grammar_id { get => grammar_id; set => grammar_id = value; }
    }
}