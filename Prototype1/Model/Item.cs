using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Model {
    public class Item : INotifyPropertyChanged {
        public Item() { } // Used for XML/JSON Deserialization.
        public Item(string id) {
            ID = id;
            Locations = new List<Location>();
        }
        public Item(string id, string name, string color, string size) {
            ID = id;
            Name = name;
            Color = color;
            Size = size;
            HasMultiLocation = false;
            Locations = new List<Location>();
        }
        public Item(string id, string name, string color, string size, List<Location> locations): this(id, name, color, size) {
            Locations = new List<Location>();
            foreach(Location location in locations) {
                AddLocation(location);
            }
        }
        public Item(string id, string name, int quantity, string color, string size, List<Location> locations, string barcode) : this(id,name, color, size, locations) {
            ServerQuantity = quantity;
            Barcode = barcode;
        }


        public int ServerQuantity { get; set; }
        public int CountedQuantity {
            get { return _countedQuantity; }
            set { _countedQuantity = value;
                OnPropertyChanged("ItemQuantity");
            }
        }
        public int QuantityVariance {
            get { return Math.Abs(ServerQuantity - CountedQuantity); }
        }
        public bool AllLocationsVisited {
            get { return !(Locations.Exists(x => x.Visited == false)); }
        }
        public bool HasMultiLocation { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string ImageUrl { get; set; }
        public string CheckSum { get; set; }
        public string Barcode { get; set; }
        public List<Location> Locations { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private int _countedQuantity = 0;


        public void AddLocation(Location location) {
            if(!HasLocation(location)) {
                Locations.Add(location);
                if (Locations.Count >= 2) {
                    HasMultiLocation = true;
                    foreach (Location loc in Locations) {
                        loc.HasMultilocationItem = true;
                    }
                }
            }

            if (!location.HasItem(this)) {
                location.AddItem(this);
            }
        }

        public bool HasLocation(Location location) {
            return Locations.Exists(x => x.ID == location.ID);
        }

        public int CompareDistance(Item otherItem, LocationComparer locationComparer) {
            int TotalDistance = 0;
            int ShortestDistance;
            int x;

            if(Locations.Count == 0 || otherItem.Locations.Count == 0) {
                throw new Exception("Can't compare Distance of items where one or both doesn't have any assigned locations"); //TODO: is this nessesary?
            }

            foreach(Location ThisItemsLocation in Locations) {
                ShortestDistance = int.MaxValue;

                foreach (Location OtherItemLocation in otherItem.Locations) {
                    x = ThisItemsLocation.CompareDistance(OtherItemLocation, locationComparer);

                    if (x < ShortestDistance) {
                        ShortestDistance = x;
                    }
                }
                TotalDistance += ShortestDistance;
            }
            return TotalDistance;
        }

        protected void OnPropertyChanged(string name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
