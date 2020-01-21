namespace Model {
    class QuantityChange {
        public QuantityChange(int quantity, string user, Location location) {
            Quantity = quantity;
            User = user;
            Location = location;
        }

        int Quantity { get; set; }
        string User { get; set; }
        Location Location { get; set; }
    }
}
