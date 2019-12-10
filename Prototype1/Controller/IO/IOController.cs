using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Model;
using Model.Log;
using System.IO;
using System.Xml.Serialization;

namespace Central_Controller.IO {
    public class IOController {
        public IOController() {
            if(Directory.GetDirectories(_savesPath).Length == 0) {
                _chosenCycleId = DateTime.Now.ToString("yyyy-MM-dd");
            } else {
                _chosenCycleId = LoadPreviousCycleId();
            }
            Setup();
        }

        public IOController(string cycleId) {
            _chosenCycleId = cycleId;
            Setup();
        }

        private readonly string _savesPath = Environment.CurrentDirectory + @"\Cycles\";
        private string _chosenCycleId;
        private string _chosenCyclePath;
        private string _logPath;
        private string _countedItemsPath;
        private string _shelfPath;


        private string LoadPreviousCycleId() {
            string[] directories = Directory.GetDirectories(_savesPath);
            string result = string.Empty;

            foreach(var path in directories) {
                if(result == string.Empty) {
                    result = path;
                } else if(Directory.GetCreationTime(result) > Directory.GetCreationTime(path)) {
                    result = path;
                }
            }
            return result.Substring(_savesPath.Length, result.Length - _savesPath.Length);
        }

        private void Setup() {
            _chosenCyclePath = _savesPath + _chosenCycleId;
            _logPath = _chosenCyclePath + @"\Log";
            _countedItemsPath = _chosenCyclePath + @"\CountedItems";
            _shelfPath = _chosenCyclePath + @"\SortPriority";
            if(!Directory.Exists(_chosenCyclePath)) { Directory.CreateDirectory(_chosenCyclePath); }
        }


        public int[] LoadShelves() {
            if(File.Exists(_shelfPath)) {
                return Array.ConvertAll(File.ReadAllLines(_shelfPath), int.Parse); 
            }
            return new int[0];
        }

        public Cycle LoadCycle(List<Item> allItems) {
            Cycle result = new Cycle();
            result.Id = _chosenCycleId;

            if(File.Exists(_logPath)) { result.Log = new LogReader().GetLogFromFile(_logPath); } 
            else { result.Log = new LogFile(_logPath, DateTime.Now); }
                
            if(File.Exists(_countedItemsPath)) { result.VerifiedItems = GetVerifiedItems(allItems, File.ReadAllLines(_countedItemsPath)); } 
            else { result.VerifiedItems = new List<Item>(); }

            return result;
        }

        private List<Item> GetVerifiedItems(List<Item> allItems, string[] verifiedItems) {
            List<Item> result = new List<Item>();
            foreach(var item in allItems) {
                foreach(var id in verifiedItems) {
                    if(item.ID == id) { break; }
                }
                result.Add(item);
            }
            return result;
        }


        public void Save(Cycle cycle, int[] shelves) {
            LogWriter logWriter = new LogWriter(_logPath, cycle.Log);
            if(!File.Exists(_logPath)) { logWriter.CreateNewFile();  } 
            else { logWriter.SaveNewMessages(); }
            File.WriteAllLines(_countedItemsPath, from item in cycle.VerifiedItems select item.ID);
            File.WriteAllLines(_shelfPath, Array.ConvertAll(shelves, Convert.ToString));
        }


        
       

    }
}
