using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Factories;
using System.Collections.Generic;
using Model;
using System;


namespace PrestaSharpAPI
{
    public class ProductAPI
    {
        private readonly string APIKey = "RS9K8F6XLQRKCL51Y3U9TZ7NY39CPMJM";
        private readonly string URL = "http://streetammo.dk/api/";
        private readonly string Password = ""; // Password is not used

        public Item GetItem(long id)
        {
            ProductFactory ProductFactory = new ProductFactory(URL, APIKey, Password);
            StockAvailableFactory stockAvailableFactory = new StockAvailableFactory(URL, APIKey, Password);
            List<stock_available> stock_Availables = stockAvailableFactory.GetAll();
            product product = ProductFactory.Get(id);
            foreach (stock_available stock_Available in stock_Availables)
            {
                if (stock_Available.id_product == product.id)
                {
                    product.location = stock_Available.location;
                }
            }
                return new Item(product.id.ToString(), product.name[1].Value, Convert.ToInt32(product.quantity), product.original_color, 
                            null, locationStringToList(locationCleaner(locationChecker(product.location))), product.upc ?? product.ean13);
        }

        public List<Item> GetAllItems()
        {
            ProductFactory productFactory = new ProductFactory(URL, APIKey, Password);
            List<product> products = productFactory.GetAll();
            StockAvailableFactory stockAvailableFactory = new StockAvailableFactory(URL, APIKey, Password);
            List<stock_available> stock_Availables = stockAvailableFactory.GetAll();
            List<Item> items = new List<Item>();
            foreach (product product in products)
            {
                foreach (stock_available stock_Available in stock_Availables)
                {
                    if (stock_Available.id_product == product.id)
                    {
                        if (product.type != "virtual")
                        {
                            items.Add(new Item(product.id.ToString(), product.name[1].Value, Convert.ToInt32(product.quantity), product.original_color,
                                      null, locationStringToList(locationCleaner(locationChecker(stock_Available.location))), product.upc ?? product.ean13));
                        }
                    }
                }
            }
            return items;
        }

        public void UpdateItemThroughAPI(Item item)
        {
            ProductFactory productFactory = new ProductFactory(URL, APIKey, Password);
            StockAvailableFactory stockAvailableFactory = new StockAvailableFactory(URL, APIKey, Password);
            List<stock_available> stock_Availables = stockAvailableFactory.GetAll();
            foreach (stock_available stock_Available in stock_Availables)
            {
                if (stock_Available.id_product == Convert.ToInt64(item.ID))
                {
                    stock_Available.quantity = item.CountedQuantity;
                    stock_Available.location = item.ToString(); //Returns all the locations for an item in a string format.
                    stockAvailableFactory.Update(stock_Available);
                }
            }
            product product = productFactory.Get(Convert.ToInt64(item.ID));
            product.quantity = item.CountedQuantity;
            productFactory.Update(product);
        }

        public void UpdateItemsThroughAPI(List<Item> items)
        {
            foreach (Item item in items)
            {
                UpdateItemThroughAPI(item);
            }
        }

        private List<string>[] createCombinedList(List<string>[] list)
        {
            //Create a list to store the result
            List<string>[] combinedList = new List<string>[3];
            combinedList[0] = new List<string>();
            combinedList[1] = new List<string>();
            combinedList[2] = new List<string>();

            int longestIndex = findLargestList(list);

            bool found = false;

            for (int i = 0; i < list[0].Count; i++)
            {
                found = false;
                int j_count = combinedList[0].Count;
                for (int j = 0; j < j_count; j++)
                {
                    if (list[0][i] == combinedList[0][j])
                    {
                        combinedList[1][j] = (Int32.Parse(combinedList[1][j]) + Int32.Parse(list[1][i])).ToString();
                        combinedList[2][j] = locationCombiner(combinedList[2][j], list[2][i]);

                        found = true;
                    }
                }
                if (!found)
                {
                    combinedList[0].Add(list[0][i]);
                    combinedList[1].Add(list[1][i]);
                    combinedList[2].Add(locationChecker(list[2][i]));
                }
            }
            for (int i = 0; i < combinedList[2].Count; i++)
            {
                string LocationString = combinedList[2][i];
                combinedList[2][i] = locationCleaner(LocationString);
            }
            return combinedList;
        }
        private int findLargestList(List<string>[] list)
        {
            int longestIndex = 0;
            for (int i = 0; i < list.Length; i++)
            {
                if (longestIndex < list[i].Count)
                {
                    longestIndex = i;
                }
            }
            return longestIndex;
        }
        private List<Location> locationStringToList(string locationString)
        {
            List<Location> locationList = new List<Location>();
            foreach (var location in locationString.Split(';'))
            {
                locationList.Add(new Location(location));
            }
            return locationList;
        }
        private string locationCombiner(string org, string add)
        {
            org += ";" + locationChecker(add);
            return org;
        }
        private string locationChecker(string location)
        {
            if (location == " ")
            {
                return "999Z99";
            }
            if (location == "")
            {
                return "999Z99";
            }
            if (location == null)
            {
                return "999Z99";
            }
            if (location.Length == 6)
            {
                return location;
            }
            if (location.Length < 6)
            {
                return "999Z99";
            }
            if (location.Length > 6)
            {
                bool semiFound = false;
                for (int i = 0; i < location.Length; i++)
                {
                    if (location[i] == ';')
                    {
                        semiFound = true;
                    }
                }
                if (semiFound)
                {
                    List<string> locationList = new List<string>();
                    string Final = string.Empty;
                    foreach (var difLocations in location.Split(';'))
                    {
                        locationList.Add(difLocations);
                    }
                    foreach (var difLocation in locationList)
                    {
                        Final = locationCombiner(Final, locationChecker(difLocation));
                    }
                }
                return "999Z99";
            }
            throw new Exception("Error! Unable to handle location:" + location);
        }
        private string locationCleaner(string locationString)
        {
            List<string> locationList = new List<string>();
            List<string> returnLocationList = new List<string>();
            foreach (var Location in locationString.Split(';'))
            {
                locationList.Add(Location);
            }
            bool dubFound = false;
            for (int i = 0; i < locationList.Count; i++)
            {
                for (int j = 0; j < returnLocationList.Count; j++)
                {
                    if (locationList[i] == returnLocationList[j])
                    {
                        dubFound = true;
                    }
                }
                if (!dubFound)
                {
                    returnLocationList.Add(locationList[i]);
                }
            }
            string returnString = returnLocationList[0];
            for (int i = 1; i < returnLocationList.Count; i++)
            {
                returnString = locationCombiner(returnString, returnLocationList[i]);
            }
            return returnString;
        }
    }
}