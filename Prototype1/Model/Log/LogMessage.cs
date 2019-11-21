using System;

namespace Model.Log {
    public abstract class LogMessage {

        public abstract DateTime Time { get; set; }
        public abstract LogMessageType Type { get; }
        public abstract string Message { get; set; }

        protected readonly char _seperator = '|';

        public abstract string GetSaveString();
        public abstract string GetExportableString();
        public abstract string GetMessageString();
    }
}
