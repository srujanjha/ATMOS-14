using SQLite;
using System;

namespace ATMOS_14
{
    [Table("Schedule")]
    public class Events
    {
        public string Id { get; set; }
        public string eventName { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public string venue { get; set; }
    }
}
