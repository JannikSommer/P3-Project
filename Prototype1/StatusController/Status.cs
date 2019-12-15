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
        public List<Item> CountedItems { get; set; } = new List<Item>();
        public List<Location> CountedLocations { get; set; }
        public bool IsInitialized { get; set; }
        ProductAPI ProductAPI = new ProductAPI();

        private readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };


        public Status()
        {
            //Locations = new List<Location>();
            //Locations.Add(new Location());
            //Locations[0].Items = ProductAPI.GetAllItems();

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
                if (CountedLocations == null)
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
                CountedLocations.Add(location);
            }
            SaveProgressToFile(); // Saves data each time the data gets updated
        }

        public void StartStatus()
        {
            CountedLocations = new List<Location>();
            IsInitialized = true;
        }

        public void GetItemsFromCountedLocations()
        {
            foreach (Location location in CountedLocations)
            {
                foreach (Item locationItem in location.Items)
                {
                    foreach (Item item in CountedItems)
                    {
                        if (item.ID == locationItem.ID)
                        {
                            item.CountedQuantity += locationItem.CountedQuantity; // adds the quantity
                            item.Locations.Add(location); // adds the location
                        }
                        else
                        {
                            CountedItems.Add(locationItem);
                            break;
                        }
                    }
                }
            }
        }

        public void FinishStatus()
        {
            GetItemsFromCountedLocations();
            IsInitialized = false;
            var path = Environment.CurrentDirectory + @"\SaveData\CompletedStatus---" + DateTime.Now.ToString("M-d-yyyy") + ".txt";
            string json = JsonConvert.SerializeObject(CountedLocations, Settings);
            System.IO.File.WriteAllText(path, json);
            DeleteStatusProgress();
            // ProductAPI.UpdateItemsThroughAPI(Items); Currently not eligble for updating through Streetammo.dk/api
        }

        private void DeleteStatusProgress() // Used to erase the status progress file when the status is completed. 
        {
            var path = Environment.CurrentDirectory + @"\SaveData\StatusData.txt";
            File.Delete(path);
        }

        public void SaveProgressToFile()
        {
            string json = JsonConvert.SerializeObject(CountedLocations, Settings);
            var path = Environment.CurrentDirectory + @"\SaveData\StatusData.txt";
            // var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//StatusData.txt";
            System.IO.File.WriteAllText(path, json);
        }

        public void LoadProgressFromFile()
        {
            var path = Environment.CurrentDirectory + @"\SaveData\StatusData.txt";
            // var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//StatusData.txt";
            string json = File.ReadAllText(path);
            CountedLocations = JsonConvert.DeserializeObject<List<Location>>(json, Settings);
        }
    }
}