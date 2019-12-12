using System;
using System.Collections.Generic;
using System.IO;


namespace Model.Log {
    public class LogReader {

        public LogReader() {}

        private readonly char _seperator = '|';


        public LogFile GetLogFromFile(string path) {
            StreamReader streamReader = new StreamReader(path);
            LogFile result = new LogFile(ParseDate(streamReader.ReadLine()), GetMessages(streamReader)); ;
            streamReader.Dispose();
            return result;
        }

        private string GetName(string path) {
            int start = -1;

            for(int i = path.Length - 1; i > 0; i--) {                
                 if(path[i] == '\\' && start == -1) {
                    start = i;
                    break;
                }
            }

            return path.Substring(start, path.Length - 1 - start);
        }

        private DateTime ParseDate(string text) {
            return DateTime.Parse(text);
        }

        private List<LogMessage> GetMessages(StreamReader streamReader) {
            List<LogMessage> result = new List<LogMessage>();

            while(streamReader.EndOfStream == false) {
                string[] messageData = streamReader.ReadLine().Split(_seperator);
                
                switch(Enum.Parse(typeof(LogMessageType), messageData[0])) {
                    case LogMessageType.LocationUpdate:
                        result.Add(ParseLocationUpdate(messageData));
                        break;

                    case LogMessageType.Verification:
                        result.Add(ParseVerificationUpdate(messageData));
                        break;
                    
                    case LogMessageType.Message:
                        result.Add(ParseTextUpdate(messageData));
                        break;
                    
                    default:
                        throw new Exception("Message type not recognized.");
                }
            }

            return result;
        }

        private TextLogMessage ParseTextUpdate(string[] messageData) {
            return new TextLogMessage(DateTime.Parse(messageData[1]), messageData[2]);
        }

        private VerificationLogMessage ParseVerificationUpdate(string[] messageData) {
            return new VerificationLogMessage(DateTime.Parse(messageData[1]), messageData[2], messageData[3], Boolean.Parse(messageData[4]));
        }

        private LocationLogMessage ParseLocationUpdate(string[] messageData) {
            string[] itemIds = messageData[4].Split(',');
            string[] itemQuantities = messageData[5].Split(',');
            List<(string itemId, string countedQuantity)> items = new List<(string itemId, string countedQuantity)>();

            for(int i = 0; i < itemIds.Length; i++) {
                items.Add((itemIds[i], itemQuantities[i]));
            }

            return new LocationLogMessage(DateTime.Parse(messageData[1]), messageData[2], messageData[3], items);
        }
    }
}
