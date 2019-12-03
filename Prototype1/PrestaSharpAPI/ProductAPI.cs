using System;
using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Factories;
using System.Collections.Generic;


namespace PrestaSharpAPI
{
    public class ProductAPI
    {
        private string APIKey = "VFIS4BMMSYNJLLVRG1571X3U15ZLTSIW";
        private string URL = "http://157.230.120.255/api/";
        private string Password = ""; // Passwords are not used.

        public product GetProduct(long ID)
        {
            ProductFactory ProductFactory = new ProductFactory(URL, APIKey, Password);
            return ProductFactory.Get(ID);
        }

        public List<product> GetAllProducts()
        {
            ProductFactory ProductFactory = new ProductFactory(URL, APIKey, Password);
            return ProductFactory.GetAll();
        }
    }
}