using System;

namespace Model.Log {
    public class TextLogMessage : LogMessage {

        public TextLogMessage(DateTime time, string text) {
            Time = time;
            Text = text;
            Type = LogMessageType.Message;
            Message = GetMessageString();
        }

        public override DateTime Time { get; set; }
        public override LogMessageType Type { get; }
        public string Text { get; set; }
        public override string Message { get; set; }


        public override string GetExportableString() {
            return Type.ToString() + _seperator +
                   Time.ToString("s") + _seperator +
                   Text;
        }

        public override string GetSaveString() {
            return GetExportableString();
        }

        public override string GetMessageString() {
            return Text;
        }
    }
}
