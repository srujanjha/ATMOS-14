using SQLite;

namespace ATMOS_14.DataModel
{
    [Table("Event")]
    public class Results
    {
        public string Id { get; set; }
        public string eventName { get; set; }
        public string first { get; set; }
        public string second { get; set; }
        public string third { get; set; }
        public string remark { get; set; }

    }
}
