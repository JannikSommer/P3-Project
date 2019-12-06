using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Factories;
using System.Collections.Generic;
using Model;

namespace PrestaSharpAPI
{
    public class ProductAPI
    {
        private readonly string APIKey = "RS9K8F6XLQRKCL51Y3U9TZ7NY39CPMJM";
        private readonly string URL = "http://streetammo.dk/api/";
        private readonly string Password = ""; // Passwords are not used

        public Item GetItem(long ID)
        {
            ProductFactory ProductFactory = new ProductFactory(URL, APIKey, Password);
            product product = ProductFactory.Get(ID);
            return new Item(product.id.ToString(), product.name[1].Value, product.original_color, null, new List<Location>(),
                            product.upc ?? product.ean13);
        }

        public List<Item> GetAllItems()
        {
            ProductFactory ProductFactory = new ProductFactory(URL, APIKey, Password);
            List<product> products = ProductFactory.GetAll();
            List<Item> items = new List<Item>();
            foreach (product product in products)
            {
                items.Add(new Item(product.id.ToString(), product.name[1].Value, product.original_color, null, new List<Location>(),
                          product.upc ?? product.ean13));
            }
            return items;
        }
    }
}