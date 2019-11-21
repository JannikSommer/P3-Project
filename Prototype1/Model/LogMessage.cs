using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    class LogMessage
    {
        public enum logtype { Error1, Error2, Error3 };
        DateTime Datetime { get; set; }
        logtype LogType { get; set; }
        string LogText { get; set; }

        public LogMessage(DateTime _datetime, logtype _logtype, string _logtext)
        {
            this.Datetime = _datetime;
            this.LogType = _logtype;
            this.LogText = _logtext;
        }

    }
}
