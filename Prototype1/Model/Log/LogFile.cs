using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Log {
    public class LogFile {

        public LogFile(string name, DateTime startDate) : this(name, startDate, new List<LogMessage>()) {}

        public LogFile(string name, DateTime startDate, List<LogMessage> messages) {
            Name = name;
            StartDate = startDate;
            Messages = messages;
            UnsavedMessages = new Queue<LogMessage>();
        }

        public string Name { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate {
            get {
                return _endDate;
            }
            private set {
                if(value > StartDate) {
                    _endDate = value;
                } else {
                    throw new ArgumentException();
                }
            }
        }
        public List<LogMessage> Messages { get; }
        public Queue<LogMessage> UnsavedMessages { get; }

        private DateTime _endDate;

        public void AddMessage(LogMessage message) {
            UnsavedMessages.Enqueue(message);
            Messages.Add(message);
        }

        public void AddMultipleMessages(IEnumerable<LogMessage> messages) {
            foreach(var message in messages) {
                AddMessage(message);
            }
        }

        // TODO: Are enumerations for types more effecient?
        public List<LogMessage> Search(LogSearchFilter filter) {
            List<LogMessage> results = new List<LogMessage>();

            foreach(var message in Messages) {
                if((filter.ItemId != string.Empty || filter.UserId != string.Empty || filter.LocationId != string.Empty)) {
                    // Remove all text messages
                    if(message.Type == LogMessageType.Message)
                        continue;

                    // Looking for location id
                    if(filter.LocationId != string.Empty && message.Type != LogMessageType.LocationUpdate) {
                        if(message.Type == LogMessageType.LocationUpdate) {
                            if(((LocationLogMessage)message).LocationId != filter.LocationId)
                                continue;
                        } else 
                            continue;
                    }

                    // Look for user id
                    if(filter.UserId != string.Empty) {
                        if(message.Type == LogMessageType.LocationUpdate && ((LocationLogMessage)message).UserId != filter.UserId)
                            continue;
                        if(message.Type == LogMessageType.Verification && ((VerificationLogMessage)message).UserId != filter.UserId)
                            continue;
                    }

                    // Look for item id
                    if(filter.ItemId != string.Empty) {
                        if(message.Type == LogMessageType.Verification && ((VerificationLogMessage)message).ItemId != filter.ItemId)
                            continue;
                        if(message.Type == LogMessageType.LocationUpdate && ((LocationLogMessage)message).Items.Where(x => x.itemId == filter.ItemId).Count() == 0) 
                            continue;
                    }
                }

                // Select all in specified time interval
                if(message.Time < filter.After || message.Time > filter.Before)
                    continue;

                results.Add(message);
            }

            return results;
        }
    }
}
