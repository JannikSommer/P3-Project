using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Model.Log {
    public class LocationLogMessage : LogMessage {

        public LocationLogMessage(DateTime time, string userId, Location location) : this(time, userId, location.ID) {
            Items = new List<(string itemId, string countedQuantity)>();
            foreach(var item in location.Items) {
                Items.Add((item.ID, item.CountedQuantity.ToString()));
            }
            Message = GetMessageString();

        }

        public LocationLogMessage(DateTime time, string userId, string locationId, List<(string itemId, string countedQuantity)> items) : this(time, userId, locationId) {
            Items = items;
            Message = GetMessageString();
        }

        private LocationLogMessage(DateTime time, string userId, string locationId) {
            Time = time;
            UserId = userId;
            LocationId = locationId;
            Type = LogMessageType.LocationUpdate;
        }

        public override DateTime Time { get; set; }
        public string LocationId { get; set; }
        public string UserId { get; set; }
        public List<(string itemId, string countedQuantity)> Items { get; set; }
        public override LogMessageType Type { get; }
        public override string Message { get; set; }

        // TODO: Add language options
        public override string GetExportableString() {
            StringBuilder stringBuilder = new StringBuilder();

            // Type of message, when, who, what?
            stringBuilder.AppendLine(
                   Type.ToString()
                + _seperator
                + Time.ToString("s")
                + _seperator
                + UserId
                + GetMessageString());

            return stringBuilder.ToString();       
        }

        public override string GetSaveString() {
            StringBuilder stringBuilder = new StringBuilder();

            // Type of message, when, who, what?
            stringBuilder.Append(
                   Type.ToString()
                + _seperator
                + Time.ToString("s")
                + _seperator
                + UserId
                + _seperator
                + LocationId);

            /// Add each counted item
            string itemIds = "",
                   itemQuantities = "";

            // Compile item-ids and -quantities to seperate strings
            foreach(var item in Items) {
                itemIds += item.itemId + ",";
                itemQuantities += item.countedQuantity + ",";
            }

            // Remove last comma and add item-ids and -quantities
            stringBuilder.AppendLine(
                  itemIds.Substring(0, itemIds.Length - 1)
                + _seperator
                + itemQuantities.Substring(0, itemQuantities.Length - 1));

            return stringBuilder.ToString();
        }

        public override string GetMessageString() {
            StringBuilder stringBuilder = new StringBuilder();

           stringBuilder.AppendLine(UserId + " started counting location " + LocationId);

            // Add each counted item
            foreach(var item in Items) {
                stringBuilder.AppendLine(
                      "   - The quanity of item "
                    + item.itemId
                    + " was changed to "
                    + item.countedQuantity);
            }

            stringBuilder.AppendLine(
                  UserId
                + " finished counting location "
                + LocationId);

            return stringBuilder.ToString();
        }
    }
}
