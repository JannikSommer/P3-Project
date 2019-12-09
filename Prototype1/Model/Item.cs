using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Model
{
    //public enum ItemSize { S = 0, Small = 0, M = 1, Medium = 1, L = 2, Large = 2, XL = 3, ExtraLarge = 3, XXL, XXXL, XXXXL, XXXXXL}
    [Serializable]
    public class Item : INotifyPropertyChanged
    {
        public int ServerQuantity { get; set; }
        private int _countedQuantity = -1;
        public int CountedQuantity {
            get { return _countedQuantity; }
            set { _countedQuantity = value;
                OnPropertyChanged("ItemQuantity");
            }
        }
        public int QuantityVariance {
            get { return Math.Abs(ServerQuantity - CountedQuantity); }
        }
        public bool HasMultiLocation { get; set; }
        public bool IsUPCBarcode { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string ImageUrl { get; set; }
        public string CheckSum { get; set; }
        public List<Location> Locations { get; set; }
        public string Barcode { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        public Item() { } // Used for XML/JSON Deserialization.

        public Item(string id)
        {
            ID = id;
            Locations = new List<Location>();
            _countedQuantity = 0;
        }

        public Item(string id, string name, string color, string size)
        {
            ID = id;
            Name = name;
            Color = color;
            Size = size;
            HasMultiLocation = false;
            Locations = new List<Location>();
        }

        public Item(string id, string name, string color, string size, List<Model.Location> locations)
        {
            ID = id;
            Name = name;
            Color = color;
            Size = size;
            Locations = new List<Location>();

            foreach (Location location in locations)
            {
                AddLocation(location);
            }
        }
        public Item(string id, string name, int quantity, string color, string size, List<Model.Location> locations, string barcode, bool isUPCBarcode)
        {
            ID = id;
            Name = name;
            ServerQuantity = quantity;
            Color = color;
            Size = size;
            Barcode = barcode;
            IsUPCBarcode = isUPCBarcode;
            Locations = new List<Location>();

            foreach (Location location in locations)
            {
                AddLocation(location);
            }
        }

        public void AddLocation(Location location)
        {
            if(!HasLocation(location))
            {
                Locations.Add(location);
                if (Locations.Count >= 2)
                {
                    HasMultiLocation = true;
                    foreach (Location loc in Locations)
                    {
                        loc.HasMultilocationItem = true;
                    }
                }
            }

            if (!location.HasItem(this))
            {
                location.AddItem(this);
            }
        }

        public bool HasLocation(Location location)
        {
            return Locations.Exists(x => x.ID == location.ID);
        }

        public int CompareDistance(Item otherItem, LocationComparer locationComparer)
        {
            int TotalDistance = 0;
            int ShortestDistance;
            int x;

            if(Locations.Count == 0 || otherItem.Locations.Count == 0)
            {
                throw new Exception("Can't compare Distance of items where one or both doesn't have any assigned locations");
            }

            foreach(Location ThisItemsLocation in Locations)
            {
                ShortestDistance = int.MaxValue;

                foreach (Location OtherItemLocation in otherItem.Locations)
                {
                    x = ThisItemsLocation.CompareDistance(OtherItemLocation, locationComparer);

                    if (x < ShortestDistance)
                    {
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
