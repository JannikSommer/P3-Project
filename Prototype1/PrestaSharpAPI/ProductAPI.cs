using System;
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

        public product GetProduct(long ID)
        {
            ProductFactory ProductFactory = new ProductFactory(URL, APIKey, Password);
            return ProductFactory.Get(ID);
        }

        public List<Item> GetAllProducts()
        {
            ProductFactory ProductFactory = new ProductFactory(URL, APIKey, Password);
            List<product> products = ProductFactory.GetAll();
            List<Item> items = new List<Item>();
            foreach (product product in products)
            {
                items.Add(new Item(product.id.ToString(), product.name[1].Value, "color", "size"));
            }

            return items;
        }
    }
}