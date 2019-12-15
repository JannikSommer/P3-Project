using System;
using System.Collections.Generic;
using Model;


namespace StatusController 
{
    public class Status 
    {
        public List<Item> Items { get; set; } = new List<Item>();
        public List<Location> Locations { get; set; } = new List<Location>();


        public void UpdateCountedLocations(List<Location> locations) 
        {
            foreach (Location location in locations) 
            {
                Locations.Add(location);
            }
        }   

        public void SaveProgressToFile() 
        {

        }

        public void LoadProgressFromFile() 
        {
            
        }
    }
}