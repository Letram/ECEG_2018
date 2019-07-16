using System;

namespace ECEG_Migration
{
    class Occupation
    {
        private String topic_name;
        private int topic_id;
        private String details;

        public string Topic_name { get => topic_name; set => topic_name = value; }
        public int Topic_id { get => topic_id; set => topic_id = value; }
        public string Details { get => details; set => details = value; }
    }
}