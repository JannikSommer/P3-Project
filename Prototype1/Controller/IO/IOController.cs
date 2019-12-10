using System;
using System.Collections.Generic;
using System.Text;
using Model;
using Model.Log;
using System.IO;
using System.Xml.Serialization;

namespace Central_Controller.IO {
    public class IOController {

        public LogFile Log { get; set; }
        public List<Item> Items { get; set; }
        public List<string> CountedItems { get; set; }
        private readonly string _savesPath = Environment.CurrentDirectory + @"\Cycles\";
        private string _chosenCycleId;
        private string _chosenCyclePath;
        private string _logPath;
        private string _countedItemsPath;

        public IOController(string cycleId) {
            CountedItems = new List<string>();
            _chosenCycleId = cycleId;
            Setup();
        }

        private void Setup() {
            _chosenCyclePath = _savesPath + _chosenCycleId;
            _logPath = _chosenCyclePath + @"\Log";
            _countedItemsPath = _chosenCyclePath + @"\CountedItems";

            if(!Directory.Exists(_chosenCyclePath)) { Directory.CreateDirectory(_chosenCyclePath); } 

            if(File.Exists(_logPath)) { Log = new LogReader().GetLogFromFile(_chosenCyclePath + @"\Log"); } 
            else { Log = new LogFile(_chosenCycleId + "Log", DateTime.Now); }
                
            if(File.Exists(_countedItemsPath)) { CountedItems.AddRange(File.ReadAllLines(_countedItemsPath)); } 
            else { CountedItems = new List<string>(); }
        }


        public void Save() {
            LogWriter logWriter = new LogWriter(_chosenCyclePath + @"\Log", Log);
            if(!File.Exists(_chosenCyclePath + @"\Log")) { logWriter.CreateNewFile();  } 
            else { logWriter.SaveNewMessages(); }
            File.WriteAllLines(_countedItemsPath, CountedItems);
        }

        




        // OBSOLETE. XmlSerialization is not needed. Would be ineffecient.
        private void SaveCountedItems() {
            StreamWriter writer = new StreamWriter(_countedItemsPath);
            XmlSerializer serializer = new XmlSerializer(typeof(List<Item>));

            serializer.Serialize(writer, CountedItems);
            writer.Dispose();
        }

        // OBSOLETE. XmlSerialization is not needed. Would be ineffecient.
        private List<Item> LoadCountedItems() {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Item>));
            FileStream fileStream = new FileStream(_countedItemsPath, FileMode.Open);

            List<Item> items = (List<Item>)serializer.Deserialize(fileStream);
            fileStream.Close();
            return items;
        }

    }
}
