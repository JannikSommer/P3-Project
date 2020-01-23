using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Model
{
    [Serializable]
    public class Location {
        public Location() { } // Used for JSON Deserialization.
        public Location(string _ID) {
            ID = _ID;
            Items = new List<Item>();

            if(_ID.Length == 6) {
                ID = _ID;
                ConvertID(ID);
            } else {
                throw new Exception("ID has to be 6 charaters long, 3 digits, then 1 letter and 2 digits again");
            }
        }
        public Location(string _ID, List<Item> items) {
            if(_ID.Length == 6) {
                ID = _ID;
                ConvertID(ID);
            } else {
                throw new Exception("ID sting to long");
            }

            Items = new List<Item>();

            if(items.Count > 0) {
                foreach(Item item in items) {
                    AddItem(item);
                }
            }
        }


        public int Shelf { get; set; }
        public int Position { get; set; }
        public char Row { get; set; }
        public bool Visited { get; set; } = false;
        public bool HasMultilocationItem { get; set; }
        public string ID { get; set; }
        public List<Item> Items { get; set; }


        private void ConvertID(string ID) {
            try
            {
                Shelf = Convert.ToInt32(ID.Substring(0, 3));
                Row = ID[3];
                Position = Convert.ToInt32(ID.Substring(4, 2));
            }
            catch (Exception e)
            {
                Shelf = 0;
                Row = 'y';
                Position = 2;

            }
        }

        public bool HasItem(Item item) {
            return Items.Exists(x => x.ID == item.ID);
        }

        public void AddItem(Item _Item) {
            if (!HasItem(_Item)) {
                Items.Add(_Item);

                if (!_Item.HasLocation(this)) {
                    _Item.AddLocation(this);
                }
                if (_Item.HasMultiLocation) {
                    HasMultilocationItem = true;
                }
            }
        }

        public int CompareDistance(Location OtherLocation, LocationComparer locationComparer) {
            if(OtherLocation.Shelf == Shelf) {
                return Math.Abs(OtherLocation.Position - Position);
            } else {
                // Multiplies 1000 to Switching shelfs, in order to make sure its always considered a longer distance then staying at the same shelf
                return Math.Abs(locationComparer.ShelfHierarchy[OtherLocation.Shelf] - locationComparer.ShelfHierarchy[Shelf]) * 1000;
            }
        }
    }
}
