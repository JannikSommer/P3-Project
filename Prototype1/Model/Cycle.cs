using System;
using System.Collections.Generic;
using Model.Log;
using System.ComponentModel;

namespace Model
{
    public class Cycle : INotifyPropertyChanged {
        public Cycle() {
            Log = new LogFile(DateTime.Now);
        }
        public Cycle(string id) : this() {
            Id = id;
        }


        public int NumberOfCountedItems {
            get { 
                return _numberOfCountedItems; 
            }
            set {
                _numberOfCountedItems = value;
                OnPropertyChanged("NumberOfCountedItems");
            }
        }
        public int NumberOfItems {
            get {
                return _numberOfItems;
            }
            set {
                _numberOfItems = value;
                OnPropertyChanged("NumberOfItems");
            }
        }

        public string Id { get; set; }
        public LogFile Log { get; set; }
        public List<Item> CountedItems { get; set; } = new List<Item>();
        public List<Item> VerifiedItems { get; set; } = new List<Item>();
        public List<Item> AllItems { get; set; } = new List<Item>();
        public List<Partition> UncountedPartitions { get; set; }
        public List<Partition> CountedPartitions { get; set; }
        public List<Partition> VerifiedPartitions { get; set; }

        // TODO: Create new list-class for easier eventhandling
        public event PropertyChangedEventHandler PropertyChanged;
        private int _numberOfCountedItems = 0;
        private int _numberOfItems = 0;

        protected void OnPropertyChanged(string name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
