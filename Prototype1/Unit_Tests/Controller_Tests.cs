using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using WPF_PC.Central_Controller;

namespace Unit_Tests
{
    [TestClass]
    public class Controller_Tests
    {
        [TestMethod]
        public void skabelon()
        {
            // Arrange

            //Expected

            // Act

            // Assert
        }

        [TestMethod]
        public void InitialAddItem_Test()
        {
            // Arrange
            Item TestItem1 = new Item("001", "TestItem1", "Red", "Large");
            List<String> TestItem1_Location_IDs = new List<string> { "001A01" };

            Item TestItem2 = new Item("002", "TestItem2", "blue", "Medium");
            List<String> TestItem2_Location_IDs = new List<string> { "001A02" };

            Item TestItem3 = new Item("003", "TestItem3", "green", "Small");
            List<String> TestItem3_Location_IDs = new List<string> { "002A01", "003A01" };

            //Expected
            SortedList<string, Location> Expected = new SortedList<string, Location>();

            Location TestLocation1 = new Location("001A01");
            TestLocation1.AddItem(TestItem1);
            Expected.Add(TestLocation1.ID, TestLocation1);

            Location TestLocation2 = new Location("001A02");
            TestLocation2.AddItem(TestItem2);
            Expected.Add(TestLocation2.ID, TestLocation2);

            Location TestLocation3 = new Location("002A01");
            TestLocation3.AddItem(TestItem3);
            Expected.Add(TestLocation3.ID, TestLocation3);

            Location TestLocation4 = new Location("003A01");
            TestLocation4.AddItem(TestItem3);
            Expected.Add(TestLocation4.ID, TestLocation4);

            // Act
            Controller Actual = new Controller();
            Actual.InitialAddItem(TestItem1, TestItem1_Location_IDs);
            Actual.InitialAddItem(TestItem2, TestItem2_Location_IDs);
            Actual.InitialAddItem(TestItem3, TestItem3_Location_IDs);

            // Assert
            Assert.AreEqual(Expected.Values[0].ID, Actual.UnPartitionedLocations.Values[0].ID);
            Assert.AreEqual(Expected.Values[1].ID, Actual.UnPartitionedLocations.Values[1].ID);
            Assert.AreEqual(Expected.Values[2].ID, Actual.UnPartitionedLocations.Values[2].ID);
            Assert.AreEqual(Expected.Values[3].ID, Actual.UnPartitionedLocations.Values[3].ID);

            Assert.AreEqual(Expected.Values[2].HasMultilocationItem, Actual.UnPartitionedLocations.Values[2].HasMultilocationItem);
            Assert.AreEqual(Expected.Values[3].HasMultilocationItem, Actual.UnPartitionedLocations.Values[3].HasMultilocationItem);
        }

        [TestMethod]
        public void CheckCountedItem_Test1()
        {
            // Arrange
            Controller TestController = new Controller();

            Item TestItem = new Item("001", "T-Shirt", "White", "Large");
            TestItem.ServerQuantity = 10;
            TestItem.CountedQuantity = 10;

            //Expected
            int ExpectedVerifiedItems = 1;

            // Act
            TestController.CheckCountedItem(TestItem);

            // Assert
            Assert.AreEqual(ExpectedVerifiedItems, TestController.ItemsVerified);
        }

        [TestMethod]
        public void CheckCountedItem_Test2()
        {
            // Arrange
            Controller TestController = new Controller();

            Item TestItem = new Item("001", "T-Shirt", "White", "Large");
            TestItem.ServerQuantity = 10;
            TestItem.CountedQuantity = 8;

            //Expected
            int ExpectedVerifiedItems = 0;

            // Act
            TestController.CheckCountedItem(TestItem);

            // Assert
            Assert.AreEqual(ExpectedVerifiedItems, TestController.ItemsVerified);
        }

        [TestMethod]
        public void CheckCountedItem_Test3()
        {
            // Arrange
            Controller TestController = new Controller();

            Item TestItem = new Item("001", "T-Shirt", "White", "Large");
            TestItem.ServerQuantity = 10;
            TestItem.CountedQuantity = -1;

            //Expected
            int ExpectedVerifiedItems = 0;

            // Act
            TestController.CheckCountedItem(TestItem);

            // Assert
            Assert.AreEqual(ExpectedVerifiedItems, TestController.ItemsVerified);
        }
    }
}
