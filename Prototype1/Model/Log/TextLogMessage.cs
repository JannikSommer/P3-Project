using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Log {
    public class TextLogMessage : LogMessage {

        public TextLogMessage(DateTime time, string text) {
            Time = time;
            Text = text;
            Type = LogMessageType.Message;
            Message = GetMessageString();
        }


        // TODO: Validation
        public override DateTime Time {
            get { return _time; }
            set { _time = value; }
        }
        public override LogMessageType Type { get; }
        public string Text { get; set; }
        public override string Message { get; set; }

        private DateTime _time;
        private string _text;

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
