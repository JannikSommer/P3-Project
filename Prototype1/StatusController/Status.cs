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
        public List<Item> NotCountedItems { get; set; } = new List<Item>();
        public List<Item> ServerItems { get; set; } = new List<Item>();
        public List<LocationBarcode> CountedLocations { get; set; }
        public bool IsInitialized { get; set; }

        private ProductAPI ProductAPI = new ProductAPI();

        private readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };


        public Status()
        {
            var path = Environment.CurrentDirectory + @"\SaveData\StatusData.txt";
            if (File.Exists(path))
            {
                IsInitialized = true;
                LoadApiItemsFromFile();
                foreach (Item item in ServerItems)
                {
                    NotCountedItems.Add(item);
                }
                LoadProgressFromFile();
                GetItemsFromCountedLocations();
            }
            else
            {
                IsInitialized = false;
            }
        }

        public void StartStatus()
        {
            CountedLocations = new List<LocationBarcode>();
            ServerItems = ProductAPI.GetAllItems();
            foreach (Item item in ServerItems)
            {
                NotCountedItems.Add(item);
            }
            SaveApiItemsToFile();
            IsInitialized = true;
        }

        public void FinishStatus()
        {
            // GetItemsFromCountedLocations();
            IsInitialized = false;
            var path = Environment.CurrentDirectory + @"\SaveData\CompletedStatus---" + DateTime.Now.ToString("M-d-yyyy") + ".txt";
            string json = JsonConvert.SerializeObject(CountedItems, Settings);
            System.IO.File.WriteAllText(path, json);
            DeleteStatusProgress();
            // ProductAPI.UpdateItemsThroughAPI(Items); Currently not eligble for updating through Streetammo.dk/api
        }

        public bool CheckForUncountedItems()
        {
            foreach (Item countedItem in CountedItems)
            {
                if (!(ServerItems.Exists(x => x.ID == countedItem.ID)))
                {
                    // If the server items has an item which is not in CountedItems there are remaining items which are not counted
                    return false;
                }
            }
            return true;
        }

        public void UpdateCountedLocations(List<LocationBarcode> locationBarcodes)
        {
            foreach (LocationBarcode locationBarcode in locationBarcodes)
            {
                CountedLocations.Add(locationBarcode);
            }
            GetItemsFromCountedLocations(); // Updates the implemented model
            SaveProgressToFile(); // Saves data each time the data gets updated
        }

        public void GetItemsFromCountedLocations()
        {
            foreach (LocationBarcode locationBarcode in CountedLocations)
            {
                foreach (ItemBarcode itemBarcode in locationBarcode.ItemBarcodes)
                {
                    foreach (Item item in CountedItems) // All currently counted items 
                    {
                        if ((item.UpcBarcode == itemBarcode.Barcode) || (item.EanBarcode == itemBarcode.Barcode))
                        {
                            // If the item already exists in the counted items, the quantity gets summed and the new location is added
                            item.CountedQuantity += 1;
                            // Add the new location to the counted item.
                            //item.Locations.Add(location);
                        }
                        else
                        {
                            foreach (Item serverItem in ServerItems)
                            {
                                if ((item.UpcBarcode == itemBarcode.Barcode) || (item.EanBarcode == itemBarcode.Barcode))
                                {
                                    // If the item is the first of the a barcode, the Item form the API gets added a location. To make sure that CountedItems has all item information. 
                                    // Then that item gets added to the list of counted items and removed from uncounted items.
                                    // serverItem.AddLocation(location);
                                    serverItem.CountedQuantity = 1; // Count is 1 after the barcode has been found once. 
                                    CountedItems.Add(serverItem);
                                    NotCountedItems.Remove(serverItem); // might not work. 
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            //// Transforms the Location structure into Item to enable easy updating of the products through API.
            //foreach (Location location in CountedLocations) // Every currently counted locations
            //{
            //    foreach (Item locationItem in location.Items) // All the items currently counted in a counted location
            //    {
            //        foreach (Item item in CountedItems) // All currently counted items 
            //        {
            //            if ((item.UpcBarcode == locationItem.UpcBarcode) || (item.EanBarcode == locationItem.EanBarcode)) 
            //            {
            //                // If the item already exists in the counted items, the quantity gets summed and the new location is added
            //                item.CountedQuantity += locationItem.CountedQuantity; 
            //                item.Locations.Add(location); 
            //            }
            //            else
            //            {
            //                foreach (Item serverItem in ServerItems)
            //                {
            //                    if ((item.UpcBarcode == locationItem.UpcBarcode) || (item.EanBarcode == locationItem.EanBarcode))
            //                    {
            //                        // If the item is the first of the a barcode, the Item form the API gets added a location. To make sure that CountedItems has all item information. 
            //                        // Then that item gets added to the list of counted items and removed from uncounted items.
            //                        serverItem.AddLocation(location); 
            //                        CountedItems.Add(serverItem);
            //                        NotCountedItems.Remove(serverItem); // might not work. 
            //                        break;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
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
            CountedLocations = JsonConvert.DeserializeObject<List<LocationBarcode>>(json, Settings);
        }

        private void SaveApiItemsToFile()
        {
            string json = JsonConvert.SerializeObject(ServerItems, Settings);
            var path = Environment.CurrentDirectory + @"\SaveData\APIData.txt";
            // var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//StatusData.txt";
            System.IO.File.WriteAllText(path, json);
        }

        private void LoadApiItemsFromFile()
        {
            var path = Environment.CurrentDirectory + @"\SaveData\APIData.txt";
            string json = File.ReadAllText(path);
            ServerItems = JsonConvert.DeserializeObject<List<Item>>(json, Settings);
        }
    }
}