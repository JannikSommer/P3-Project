using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Log {
    public class LogSearchFilter {

        public LogSearchFilter(DateTime after, DateTime before, string userId, string locationId, string itemId) {
            After = after;
            Before = before;
            UserId = userId;
            LocationId = locationId;
            ItemId = itemId;
        }

        public DateTime After { get; }
        public DateTime Before { get; }
        public string UserId { get; }
        public string LocationId { get; }
        public string ItemId { get; }

    }
}
