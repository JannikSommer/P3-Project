using System;
using System.Collections.Generic;
using System.Text;

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

        public string LocationId {
            get {
                return _locationId;
            }
            set {
                if(value != string.Empty) {
                    _locationId = value;
                } else {
                    throw new ArgumentException();
                }
            }
        }
        public string UserId {
            get {
                return _userId;
            }
            set {
                if(value != string.Empty) {
                    _userId = value;
                } else {
                    throw new ArgumentException();
                }
            }
        }
        public override string Message { get; set; }
        public override DateTime Time { get; set; }
        public override LogMessageType Type { get; }
        public List<(string itemId, string countedQuantity)> Items { get; set; }

        private string _userId;
        private string _locationId;



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
                + LocationId
                + _seperator);

            /// Add each counted item
            string itemIds = "",
                   itemQuantities = "";

            // Compile item-ids and -quantities to seperate strings
            foreach(var (itemId, countedQuantity) in Items) {
                itemIds += itemId + ",";
                itemQuantities += countedQuantity + ",";
            }

            // Remove last comma and add item-ids and -quantities
            stringBuilder.Append(
                  itemIds.Substring(0, itemIds.Length - 1)
                + _seperator
                + itemQuantities.Substring(0, itemQuantities.Length - 1));

            return stringBuilder.ToString();
        }

        public override string GetMessageString() {
            StringBuilder stringBuilder = new StringBuilder();

           stringBuilder.AppendLine(UserId + " started counting location " + LocationId);

            // Add each counted item
            foreach(var (itemId, countedQuantity) in Items) {
                stringBuilder.AppendLine(
                      "   - The quanity of item "
                    + itemId
                    + " was changed to "
                    + countedQuantity);
            }

            stringBuilder.AppendLine(
                  UserId
                + " finished counting location "
                + LocationId);

            return stringBuilder.ToString();
        }
    }
}
