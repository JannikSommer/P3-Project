using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace Model.Log {
    public class LogWriter {

        public LogWriter(string filePath, LogFile log) {
            _path = filePath;
            _log = log;
        }

        private readonly string _path;
        private LogFile _log;


        public void CreateNewFile() {
            CreateNewFile(_path);
        }
        
        public void CreateNewFile(string otherPath) {
            StreamWriter streamWriter = new StreamWriter(otherPath);

            streamWriter.WriteLine(_log.StartDate);
            foreach(var message in _log.Messages) {
                streamWriter.WriteLine(message.GetSaveString());
            }

            streamWriter.Dispose();
            _log.UnaddedMessages.Clear();
        }

        public void SaveNewMessages() {
            StreamWriter streamWriter = new StreamWriter(_path, true);

            while(_log.UnaddedMessages.Count != 0) {
                streamWriter.WriteLine(_log.UnaddedMessages.Dequeue().GetSaveString());
            }

            streamWriter.Dispose();
        }

        public void ExportLogFile(string path) {
            StreamWriter streamWriter = new StreamWriter(path);

            streamWriter.WriteLine(_log.StartDate);
            foreach(var message in _log.Messages) {
                streamWriter.WriteLine(message.GetExportableString());
            }

            streamWriter.Dispose();
        }

    }
}
