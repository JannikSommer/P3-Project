using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Factories;
using System.Collections.Generic;
using Model;
using System;
using SQL_DB_test_Frame;


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
            Sorting sorter = new Sorting();
            foreach (stock_available stock_Available in stock_Availables)
            {
                if (stock_Available.id_product == product.id)
                {
                    product.location = stock_Available.location;
                }
            }
                return new Item(product.id.ToString(), product.name[1].Value, Convert.ToInt32(product.quantity), product.original_color, 
                            null, sorter.locationStringToList(sorter.locationCleaner(sorter.locationChecker(product.location))), product.upc ?? product.ean13);
        }

        public List<Item> GetAllItems()
        {
            ProductFactory productFactory = new ProductFactory(URL, APIKey, Password);
            List<product> products = productFactory.GetAll();
            StockAvailableFactory stockAvailableFactory = new StockAvailableFactory(URL, APIKey, Password);
            List<stock_available> stock_Availables = stockAvailableFactory.GetAll();
            List<Item> items = new List<Item>();
            Sorting sorter = new Sorting();
            foreach (product product in products)
            {
                foreach (stock_available stock_Available in stock_Availables)
                {
                    if (stock_Available.id_product == product.id)
                    {
                        if (product.type != "virtual")
                        {
                            items.Add(new Item(product.id.ToString(), product.name[1].Value, Convert.ToInt32(product.quantity), product.original_color,
                                      null, sorter.locationStringToList(sorter.locationCleaner(sorter.locationChecker(stock_Available.location))), product.upc ?? product.ean13));
                        }
                    }
                }
            }
            return items;
        }

        public void UpdateItemThroughAPI(Item item)
        {
            ProductFactory productFactory = new ProductFactory(URL, APIKey, Password);
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
    }
}