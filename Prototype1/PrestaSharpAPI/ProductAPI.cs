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
        private readonly string Password = ""; // Passwords are not used

        public Item GetItem(long id)
        {
            ProductFactory productFactory = new ProductFactory(URL, APIKey, Password);
            product product = productFactory.Get(id);
            return new Item(product.id.ToString(), product.name[1].Value, Convert.ToInt32(product.quantity), product.original_color, 
                            null, new List<Location>(), product.upc ?? product.ean13, product.upc != null ? true : false);
        }

        public List<Item> GetAllItems()
        {
            ProductFactory productFactory = new ProductFactory(URL, APIKey, Password);
            List<product> products = productFactory.GetAll();
            List<Item> items = new List<Item>();
            foreach (product product in products)
            {
                if (product.type != "virtual")
                {
                    items.Add(new Item(product.id.ToString(), product.name[1].Value, Convert.ToInt32(product.quantity), product.original_color, 
                              null, new List<Location>(), product.upc ?? product.ean13, product.upc != null ? true : false));
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