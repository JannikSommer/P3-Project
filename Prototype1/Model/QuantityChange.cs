using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    class QuantityChange
    {
        List<Location> _locations { get; set; }
        int _quantity { get; set; }

        public QuantityChange(List<Location> locations, int quantity)
        {
            this._locations = locations;
            this._quantity = quantity;
        }
        //.
    }
}
