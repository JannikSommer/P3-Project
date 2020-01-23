using System;
using Model;
using System.Collections.Generic;
using System.IO;
using PrestaSharpAPI;
using Newtonsoft.Json;
using System.Collections;

namespace StatusController
{
    public class Status
    {
        public List<Item> CountedItems { get; set; } = new List<Item>();
        public List<Item> ServerItems { get; set; } = new List<Item>();
        public List<LocationBarcode> CountedLocations { get; set; } = new List<LocationBarcode>();
        public bool IsInitialized { get; set; }

        public Hashtable Hashtable { get; set; } = new Hashtable();
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
                    foreach (Location locataion in item.Locations)
                    {
                        if (locataion.ID != "999Z99" && !(Hashtable.ContainsKey(item.Barcode)))
                        {
                            Hashtable.Add(item.Barcode, 0);
                        }
                    }
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
            ServerItems = ProductAPI.GetAllItems();
            foreach (Item item in ServerItems)
            {
                foreach (Location locataion in item.Locations)
                {
                    if (locataion.ID != "999Z99" && !(Hashtable.ContainsKey(item.Barcode)))
                    {
                        Hashtable.Add(item.Barcode, 0);
                    }
                }
            }
            SaveApiItemsToFile();
            IsInitialized = true;
        }

        public void FinishStatus()
        {
            // GetItemsFromCountedLocations();
            IsInitialized = false;
            foreach (DictionaryEntry de in Hashtable)
            {
                foreach (Item item in ServerItems)
                {
                    if (Hashtable.ContainsKey(item.Barcode))
                    {
                        item.CountedQuantity = (int) de.Value; // Updates the quantity in the list serverItems
                        CountedItems.Add(item);
                    }
                }
            }
            var path = Environment.CurrentDirectory + @"\SaveData\CompletedStatus---" + DateTime.Now.ToString("M-d-yyyy") + ".txt";
            string json = JsonConvert.SerializeObject(CountedItems, Settings);
            System.IO.File.WriteAllText(path, json);
            DeleteStatusProgress();
            DeleteApiItemsFile();
            // ProductAPI.UpdateItemsThroughAPI(); Currently not eligble for updating through Streetammo.dk/api
        }

        private void DeleteApiItemsFile()
        {
            var path = Environment.CurrentDirectory + @"\SaveData\APIData.txt";
            File.Delete(path);
        }

        public bool CheckForUncountedItems()
        {
            foreach (Item serverItem in ServerItems)
            {
                if (!ContainsItem(CountedItems, serverItem))
                {
                    return false;
                }
            }
            return true;
        }
        private bool ContainsItem(List<Item> itemList, Item target)
        {
            foreach (Item item in itemList)
            {
                if (item.ID == target.ID)
                {
                    return true;
                }
            }
            return false;
        }

        public void UpdateCountedLocations(List<LocationBarcode> locationBarcodes)
        {
            foreach (LocationBarcode locationBarcode in locationBarcodes)
            {
                if (CountedLocations.Count == 0)
                {
                    CountedLocations.Add(locationBarcode);
                }
                else
                {
                    foreach (LocationBarcode countedLocationBarcode in CountedLocations)
                    {
                        if (!(locationBarcode.Barcode == countedLocationBarcode.Barcode))
                        {
                            // The locations are the same and should not be added more than once.
                            CountedLocations.Add(locationBarcode);
                        }
                    }
                }
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
                    if (Hashtable.ContainsKey(itemBarcode.Barcode))
                    {
                        // Don't know if this works :/
                        int count = (int) Hashtable[itemBarcode.Barcode];
                        count += itemBarcode.Quantity;
                        Hashtable[itemBarcode.Barcode] = count;
                        foreach (Item serverItem in ServerItems)
                        {
                            if (serverItem.Barcode == itemBarcode.Barcode)
                            {
                                foreach (Location location in serverItem.Locations)
                                {
                                    if (location.ID == locationBarcode.Barcode)
                                    {
                                        serverItem.CountedQuantity = (int)Hashtable[itemBarcode.Barcode];
                                        CountedItems.Add(serverItem);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DeleteStatusProgress() // Used to erase the status progress file when the status is completed. 
        {
            var path = Environment.CurrentDirectory + @"\SaveData\StatusData.txt";
            File.Delete(path);
        }

        private void SaveProgressToFile()
        {
            string json = JsonConvert.SerializeObject(CountedLocations, Settings);
            var path = Environment.CurrentDirectory + @"\SaveData\StatusData.txt";
            // var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//StatusData.txt";
            System.IO.File.WriteAllText(path, json);
        }

        private void LoadProgressFromFile()
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