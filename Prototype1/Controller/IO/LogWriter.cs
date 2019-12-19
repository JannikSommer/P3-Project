using System;
using System.IO;

namespace Model.Log {
    public class LogWriter {
        public LogWriter(string filePath, LogFile log) {
            Path = filePath;
            Log = log;
        }

        public string Path {
            get {
                return _path;
            }
            set {
                if(value != string.Empty) {
                    _path = value;
                } else {
                    throw new ArgumentException();
                }
            }
        }
        public LogFile Log {
            get {
                return _log;
            }
            set {
                if(value != null) {
                    _log = value;
                } else {
                    throw new ArgumentNullException();
                }
            }
        }

        private string _path;
        private LogFile _log;


        public void CreateNewFile() {
            CreateNewFile(_path);
        }
        
        public void CreateNewFile(string otherPath) {
            StreamWriter streamWriter = new StreamWriter(otherPath);
            streamWriter.WriteLine(_log.StartDate.ToString("s"));
            streamWriter.Dispose();

            SaveNewMessages();
        }

        public void SaveNewMessages() {
            StreamWriter streamWriter = new StreamWriter(_path, true);

            while(_log.UnsavedMessages.Count != 0) {
                streamWriter.WriteLine(_log.UnsavedMessages.Dequeue().GetSaveString());
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
