using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECEG_Migration
{
    public static class StaticData
    {
        private static List<int> last_search_ids = new List<int>();

        public static List<int> Last_search_ids { get => last_search_ids; set => last_search_ids = value; }
    }
}