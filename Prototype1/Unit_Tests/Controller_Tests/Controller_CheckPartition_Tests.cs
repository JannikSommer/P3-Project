using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Central_Controller;

namespace Unit_Tests.Controller_Tests
{
    [TestClass]
    public class Controller_CheckPartition_Tests
    {
        [TestMethod]
        public void CheckCountedItem_Test1_SingleItemTest()
        {
            // Arrange
            Controller TestController = new Controller();

            Item TestItem = new Item("001", "T-Shirt", "White", "Large");
            TestItem.ServerQuantity = 10;
            TestItem.CountedQuantity = 10;

            Location TestLocation = new Location("001A01");
            TestLocation.AddItem(TestItem);

            Partition TestPartition = new Partition(false);
            TestPartition.AddLocation(TestLocation);

            //Expected
            int ExpectedVerifiedItems = 1;

            // Act
            TestController.CheckPartition(TestPartition);

            // Assert
            Assert.AreEqual(ExpectedVerifiedItems, TestController.ItemsVerified);
        }

        [TestMethod]
        public void CheckCountedItem_Test2_SingleItemTest()
        {
            // Arrange
            Controller TestController = new Controller();

            Item TestItem = new Item("001", "T-Shirt", "White", "Large");
            TestItem.ServerQuantity = 10;
            TestItem.CountedQuantity = 8;

            Location TestLocation = new Location("001A01");
            TestLocation.AddItem(TestItem);

            Partition TestPartition = new Partition(false);
            TestPartition.AddLocation(TestLocation);

            //Expected
            int ExpectedVerifiedItems = 0;

            // Act
            TestController.CheckPartition(TestPartition);

            // Assert
            Assert.AreEqual(ExpectedVerifiedItems, TestController.ItemsVerified);
        }

        [TestMethod]
        public void CheckCountedItem_Test3_SingleItemTest()
        {
            // Arrange
            Controller TestController = new Controller();

            Item TestItem = new Item("001", "T-Shirt", "White", "Large");
            TestItem.ServerQuantity = 10;
            TestItem.CountedQuantity = -1;

            Location TestLocation = new Location("001A01");
            TestLocation.AddItem(TestItem);

            Partition TestPartition = new Partition(false);
            TestPartition.AddLocation(TestLocation);

            //Expected
            int ExpectedVerifiedItems = 0;

            // Act
            TestController.CheckPartition(TestPartition);

            // Assert
            Assert.AreEqual(ExpectedVerifiedItems, TestController.ItemsVerified);
        }
        [TestMethod]
        public void CheckPartition_Test4_MultiLocationItemTest_partiallyCounted()
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

        [TestMethod]
        public void CheckPartition_Test5_MultiLocationItemTest_PartiallyCounted()
        {
            // Arrange
            Controller TestController = new Controller();

            Item TestItem = new Item("001", "T-Shirt", "White", "Large");
            TestItem.ServerQuantity = 10;
            TestItem.CountedQuantity = 10;

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

        [TestMethod]
        public void CheckPartition_Test6_MultiLocationItemTest_FullyCounted()
        {
            // Arrange
            Controller TestController = new Controller();

            Item TestItem = new Item("001", "T-Shirt", "White", "Large");
            TestItem.ServerQuantity = 10;
            TestItem.CountedQuantity = 10;

            Location TestLocation1 = new Location("001A01");
            Location TestLocation2 = new Location("002A01");

            TestItem.AddLocation(TestLocation1);
            TestItem.AddLocation(TestLocation2);

            Partition TestPartition = new Partition(true);
            TestPartition.AddLocation(TestLocation1);
            TestPartition.AddLocation(TestLocation2);

            //Expected
            int Expected_VerifiedItems = 1;
            int Expected_PartitallyCountedItemsCount = 0;

            // Act
            TestController.CheckPartition(TestPartition);

            // Assert
            Assert.AreEqual(Expected_VerifiedItems, TestController.ItemsVerified);
            Assert.AreEqual(Expected_PartitallyCountedItemsCount, TestController.PartiallyCountedItems.Count);
        }
    }
}
