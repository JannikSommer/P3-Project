using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Central_Controller;

namespace Unit_Tests.Controller_Tests
{
    [TestClass]
    public class Controller_CheckCountedItem_Tests
    {
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

        [TestMethod]
        public void CheckCountedItem_Test4_MultiLocationItemTest()
        {
            // Arrange
            Controller TestController = new Controller();

            Item TestItem = new Item("001", "T-Shirt", "White", "Large");
            TestItem.ServerQuantity = 10;
            TestItem.CountedQuantity = 8;

            Location TestLocation1 = new Location("001A01");
            Location TestLocation2 = new Location("002A01");

            TestItem.AddLocation(TestLocation1);
            TestItem.AddLocation(TestLocation2);

            Partition TestPartition = new Partition(true);
            TestPartition.AddLocation(TestLocation1);

            //Expected
            int Expected_VerifiedItems = 0;
            int Expected_PartitallyCountedItemsCount = 1;

            // Act
            TestController.CheckPartition(TestPartition);

            // Assert
            Assert.AreEqual(Expected_VerifiedItems, TestController.ItemsVerified);
            Assert.AreEqual(Expected_PartitallyCountedItemsCount, TestController.PartiallyCountedItems.Count);
        }
    }
}
