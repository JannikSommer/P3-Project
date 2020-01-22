using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using StatusController;
using Networking;


namespace Unit_Tests
{
    [TestClass]
    public class StatusTest
    {
        public List<LocationBarcode> CreateBarcodeData()
        {
            List<LocationBarcode> locationBarcodes = new List<LocationBarcode>();
            locationBarcodes.Add(new LocationBarcode("001A02"));
            locationBarcodes.Add(new LocationBarcode("001B02"));
            locationBarcodes.Add(new LocationBarcode("001C02"));
            locationBarcodes.Add(new LocationBarcode("001D02"));
            locationBarcodes.Add(new LocationBarcode("001E02"));
            foreach (LocationBarcode locationBarcode in locationBarcodes)
            {
                locationBarcode.AddItemBarcode("12345");
                locationBarcode.AddItemBarcode("23456");
                locationBarcode.AddItemBarcode("34567");
                locationBarcode.AddItemBarcode("45678");
                locationBarcode.AddItemBarcode("56789");
            }

            return locationBarcodes;
        }

        [TestMethod]
        public void Test1()
        {
            // Arrange
            List<LocationBarcode> locationBarcodes = CreateBarcodeData();
            Status status = new Status();

            // Act
            status.UpdateCountedLocations(locationBarcodes);

            // Assert
            var expected = 5;
            var actual = status.CountedLocations.Count;
            Assert.AreEqual(expected, actual);
        }
    }
}
