using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Location
    {
        public string ID { get; private set; }
        public bool IsEmpty { get; private set; }
        public int Shelf { get; private set; }
        public char Row { get; private set; }
        public int Position { get; private set; }
        public bool HasMultilocationItem { get; set; }
        public List<Item> Items { get; private set; }

        public Location() { } // Used for JSON Deserialization.

        public Location(string _ID)
        {
            ID = _ID;
            IsEmpty = true;
            Items = new List<Item>();

            if (_ID.Length == 6)
            {
                ID = _ID;
                ConvertID(ID);
            }
            else
            {
                throw new Exception("ID has to be 6 charaters long, 3 digits, then 1 letter and 2 digits again");
            }
        }

        public Location(string _ID, List<Item> items)
        {
            if (_ID.Length == 6)
            {
                ID = _ID;
                ConvertID(ID);
            }
            else
            {
                throw new Exception("ID sting to long");
            }

            Items = new List<Item>();

            if(items.Count > 0)
            {
                foreach(Item item in items)
                {
                    AddItem(item);
                }
            }
            else
            {
                IsEmpty = true;
            }
        }

        private void ConvertID(string ID)
        {
            Shelf = Convert.ToInt32(ID.Substring(0, 3));
            Row = ID[3];
            Position = Convert.ToInt32(ID.Substring(4, 2));
        }

        public bool HasItem(Item item)
        {
            return Items.Exists(x => x.ID == item.ID);
        }

        public void AddItem(Item _Item)
        {
            if (!HasItem(_Item))
            {
                Items.Add(_Item);
                IsEmpty = false;

                if (!_Item.HasLocation(this))
                {
                    _Item.AddLocation(this);
                }

                if (_Item.HasMultiLocation)
                {
                    HasMultilocationItem = true;
                }
            }
        }

        public int CompareDistance(Location OtherLocation, LocationComparer locationComparer)
        {
            if(OtherLocation.Shelf == Shelf)
            {
                return Math.Abs(OtherLocation.Position - Position);
            }
            else
            {
                //multiplies 1000 to Switching shelfs, in order to make sure its always considered a longer distance then staying at the same shelf
                return Math.Abs(locationComparer.ShelfHierakyi[OtherLocation.Shelf] - locationComparer.ShelfHierakyi[Shelf]) * 1000;
            }
        }

        /*public int CompareTo(object location)
        {
            Location OtherLocation = (Location)location;
            return ID.CompareTo(OtherLocation.ID);
        }*/
    }
}
