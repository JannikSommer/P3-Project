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
        public string Row { get; private set; }
        public string Posistion { get; private set; }
        public List<Item> Items { get; private set; }

        public Location() { } // Used for JSON Deserialization.
        public Location(string _ID)
        {
            IsEmpty = true;
            Items = new List<Item>();

            if (_ID.Length == 6)
            {
                ID = _ID;
                ConvertID(ID);
            }
            else
            {
                throw new Exception("ID sting to long");
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

            Items = items;

            if(items.Count > 0)
            {
                IsEmpty = false;
            }
            else
            {
                IsEmpty = true;
            }
        }

        private void ConvertID(string ID)
        {
            Shelf = Convert.ToInt32(ID.Substring(0, 3));
            Row = ID.Substring(3, 1);
            Posistion = ID.Substring(4, 2);
        }

        public void AddItem(Item _Item)
        {
            Items.Add(_Item);
            IsEmpty = false;
        }
    }
}
