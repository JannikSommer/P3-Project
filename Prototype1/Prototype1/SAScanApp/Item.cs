using System;
using System.Collections.Generic;
using System.Text;

namespace SAScanApp
{
    //public enum ItemSize { S = 0, Small = 0, M = 1, Medium = 1, L = 2, Large = 2, XL = 3, ExtraLarge = 3, XXL, XXXL, XXXXL, XXXXXL}

    public class Item
    {
        public string ItemID { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Checksum { get; set; }
        public string ImageUrl { get; set; }
        public bool HasMultiLocation { get; set; }
        public List<Location> Locations { get; set; }

        public Item() { } // Used for JSON Deserialization.

        public Item(string _ID, string _Name, string _Color, string _Size, string _Checksum, string _ImageUrl)
        {
            ItemID = _ID; 
            Name = _Name;
            Color = _Color;
            Size = _Size;
            Checksum = _Checksum;
            ImageUrl = _ImageUrl;
            HasMultiLocation = false;
            Locations = new List<Location>();
        }

        public Item(string _ID, string _Name, string _Color, string _Size, List<Location> _Locations)
        {
            ItemID = _ID;
            Name = _Name;
            Color = _Color;
            Size = _Size;
            Locations = _Locations;

            if (_Locations.Count >= 2)
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
            if (Locations.Count >= 2)
            {
                HasMultiLocation = true;
            }
        }

    }
}
