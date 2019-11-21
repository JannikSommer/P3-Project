using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    class Log
    {
        public List<LogMessage> Logmessage { get; set; }

        public Log()
        {
            this.Logmessage = new List<LogMessage>();
        }

        void UpdateLog(string LogMessage, LogMessage.logtype ErrorType)
        {
            DateTime localDate = DateTime.Now;

            LogMessage logmessage = new LogMessage(localDate, ErrorType, LogMessage);
            //.
        }
    }

}
