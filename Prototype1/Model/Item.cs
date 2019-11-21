using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    //public enum ItemSize { S = 0, Small = 0, M = 1, Medium = 1, L = 2, Large = 2, XL = 3, ExtraLarge = 3, XXL, XXXL, XXXXL, XXXXXL}

    public class Item
    {
        public int ServerQuantity { get; set; }
        public int CountedQuantity { get; set; }
        public bool HasMultiLocation { get; private set; }
        public string ID { get; private set; }
        public string Name { get; private set; }
        public string Color { get; private set; }
        public string Size { get; private set; }
        public string ImageUrl { get; private set; }
        public string CheckSum { get; set; }
        public List<Model.Location> Locations { get; private set; }


        public Item() { } // Used for JSON Deserialization.

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
            Locations = _Locations;

            if(_Locations.Count >= 2)
            {
                HasMultiLocation = true;
            }
            else
            {
                HasMultiLocation = false;
            }
        }

        public void AddLocation(Location _Location)
        {
            Locations.Add(_Location);
            if(Locations.Count >= 2)
            {
                HasMultiLocation = true;
            }
        }
    }
}