using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECEG_Migration.Models
{
    public class TargetAudience
    {
        private int audienceCriteria;
        private string audienceName;

        public int AudienceCriteria { get => audienceCriteria; set => audienceCriteria = value; }
        public string AudienceName { get => audienceName; set => audienceName = value; }
    }
}