using System;
using Model;
using System.Collections.Generic;
using System.IO;
using PrestaSharpAPI;
using Newtonsoft.Json;

namespace StatusController
{
    public class Status
    {
        public List<Location> Locations { get; private set; }
        public bool IsInitialized { get; set; } = true;

        private readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public Status()
        {
            try
            {
                LoadProgressFromFile();
            }
            catch (System.IO.IOException e)
            {
                // catch error
            }
            finally
            {
                if (Locations == null)
                {
                    IsInitialized = false;
                }
                else
                {
                    IsInitialized = true;
                }
            }
        }

        public void UpdateCountedLocations(List<Location> locations)
        {
            foreach (Location location in locations)
            {
                Locations.Add(location);
            }
        }

        public void SaveProgressToFile()
        {
            string json = JsonConvert.SerializeObject(Locations, Settings);
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//StatusData.txt";
            System.IO.File.WriteAllText(path, json);
        }

        public void LoadProgressFromFile()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//StatusData.txt";
            string json = File.ReadAllText(path);
            Locations = JsonConvert.DeserializeObject<List<Location>>(json, Settings);
        }
    }
}