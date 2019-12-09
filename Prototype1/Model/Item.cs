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
        public string ID { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string ImageUrl { get; set; }
        public string CheckSum { get; set; }
        public List<Location> Locations { get; set; }
        public string Barcode { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;


        public int _countedQuantity { get; set; }

        public Item() { } // Used for XML/JSON Deserialization.

        public Item(string id)
        {
            ID = id;
            Locations = new List<Location>();
            _countedQuantity = 0;
        }

        public Item(string _ID, string _Name, string _Color, string _Size)
        {
            ID = _ID;   
            Name = _Name;
            Color = _Color;
            Size = _Size;
            HasMultiLocation = false;
            Locations = new List<Location>();
        }

        public Item(string _ID, string _Name, string _Color, string _Size, List<Model.Location> _Locations)
        {
            ID = _ID;
            Name = _Name;
            Color = _Color;
            Size = _Size;
            Locations = new List<Location>();

            foreach (Location location in _Locations)
            {
                AddLocation(location);
            }
        }
        public Item(string _ID, string _Name, string _Color, string _Size, List<Model.Location> _Locations, string barcode)
        {
            ID = _ID;
            Name = _Name;
            Color = _Color;
            Size = _Size;
            Barcode = barcode;
            Locations = new List<Location>();

            foreach (Location location in _Locations)
            {
                AddLocation(location);
            }
        }

        public void AddLocation(Location _Location)
        {
            if(!HasLocation(_Location))
            {
                Locations.Add(_Location);
                if (Locations.Count >= 2)
                {
                    HasMultiLocation = true;
                    foreach (Location location in Locations)
                    {
                        location.HasMultilocationItem = true;
                    }
                }
            }

            if (!_Location.HasItem(this))
            {
                _Location.AddItem(this);
            }
        }

        public bool HasLocation(Location location)
        {
            return Locations.Exists(x => x.ID == location.ID);
        }

        public int CompareDistance(Item OtherItem, LocationComparer locationComparer)
        {
            int TotalDistance = 0;
            int ShortestDistance;
            int x;

            if(Locations.Count == 0 || OtherItem.Locations.Count == 0)
            {
                throw new Exception("Can't compare Distance of items where one or both doesn't have any assigned locations");
            }

            foreach(Location ThisItemsLocation in Locations)
            {
                ShortestDistance = int.MaxValue;

                foreach (Location OtherItemLocation in OtherItem.Locations)
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
