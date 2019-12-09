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
        private readonly string Password = ""; // Passwords are not used

        public Item GetItem(long id)
        {
            ProductFactory ProductFactory = new ProductFactory(URL, APIKey, Password);
            product product = ProductFactory.Get(id);
            Sorting sorter = new Sorting();
            return new Item(product.id.ToString(), product.name[1].Value, Convert.ToInt32(product.quantity), product.original_color, 
                            null, sorter.locationStringToList(sorter.locationCleaner(sorter.locationChecker(product.location))), product.upc ?? product.ean13);
        }

        public List<Item> GetAllItems()
        {
            ProductFactory ProductFactory = new ProductFactory(URL, APIKey, Password);
            List<product> products = ProductFactory.GetAll();
            List<Item> items = new List<Item>();
            Sorting sorter = new Sorting();
            foreach (product product in products)
            {
                if (product.type != "virtual")
                {
                    items.Add(new Item(product.id.ToString(), product.name[1].Value, Convert.ToInt32(product.quantity), product.original_color, 
                              null, sorter.locationStringToList(sorter.locationCleaner(sorter.locationChecker(product.location))), product.upc ?? product.ean13));
                }
            }
            return items;
        }
    }
}