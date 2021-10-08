using System.Collections.Generic;

namespace LMSG3.Core.Models.Entities
{
    public class ActivityType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // NAV
        public ICollection<Activity> Activities { get; set; }
    }
}