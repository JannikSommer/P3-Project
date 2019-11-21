﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    //public enum ItemSize { S = 0, Small = 0, M = 1, Medium = 1, L = 2, Large = 2, XL = 3, ExtraLarge = 3, XXL, XXXL, XXXXL, XXXXXL}

    public class Item
    {
        public string ID { get; private set; }
        public string Name { get; private set; }
        public string Color { get; private set; }
        public string Size { get; private set; }
        public bool HasMultiLocation { get; set; }
        public int ServerQuantity { get; set; }
        public int CountedQuantity { get; set; }
        public List<Location> Locations { get; private set; }

        public Item() { } // Used for JSON Deserialization.

        public Item(string id)
        {
            ID = id;
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

        public Item(string _ID, string _Name, string _Color, string _Size, List<Location> _Locations)
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
    }
}
