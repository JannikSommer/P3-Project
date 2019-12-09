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
        public void CheckPartition_Test1_SingleLocationTest()
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
        public void CheckPartition_Test2_SingleLocationTest()
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
        public void CheckPartition_Test3_SingleLocationTest()
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

        [TestMethod]
        public void CheckPartition_Test7__MultiLocationItemTest_NotCounted()
        {
            // Arrange
            Controller TestController = new Controller();

            Item TestItem = new Item("001", "T-Shirt", "White", "Large");
            TestItem.ServerQuantity = 10;
            TestItem.CountedQuantity = -1;

            Location TestLocation1 = new Location("001A01");
            Location TestLocation2 = new Location("002A01");

            TestItem.AddLocation(TestLocation1);
            TestItem.AddLocation(TestLocation2);

            Partition TestPartition = new Partition(true);
            TestPartition.AddLocation(TestLocation1);
            TestPartition.AddLocation(TestLocation2);

            //Expected
            int Expected_VerifiedItems = 0;
            int Expected_PartitallyCountedItemsCount = 0;
            int Expected_MultiLocationPartitions = 1;

            // Act
            TestController.CheckPartition(TestPartition);

            // Assert
            Assert.AreEqual(Expected_VerifiedItems, TestController.ItemsVerified);
            Assert.AreEqual(Expected_PartitallyCountedItemsCount, TestController.PartiallyCountedItems.Count);
            Assert.AreEqual(Expected_MultiLocationPartitions, TestController.MultiLocationPartitions[0].Count);
        }

        [TestMethod]
        public void CheckPartition_Test8__MultiLocationItemTest_NotCounted()
        {
            // Arrange
            Controller TestController = new Controller();

            Item TestItem = new Item("001", "T-Shirt", "White", "Large");
            TestItem.ServerQuantity = 10;
            TestItem.CountedQuantity = -1;

            Location TestLocation1 = new Location("001A01");
            Location TestLocation2 = new Location("002A01");

            TestItem.AddLocation(TestLocation1);
            TestItem.AddLocation(TestLocation2);

            Partition TestPartition = new Partition(true);
            TestPartition.AddLocation(TestLocation1);

            //Expected
            int Expected_VerifiedItems = 0;
            int Expected_PartitallyCountedItemsCount = 0;
            int Expected_PriorityPartionsCount = 1;

            // Act
            TestController.CheckPartition(TestPartition);

            // Assert
            Assert.AreEqual(Expected_VerifiedItems, TestController.ItemsVerified);
            Assert.AreEqual(Expected_PartitallyCountedItemsCount, TestController.PartiallyCountedItems.Count);
            Assert.AreEqual(Expected_PriorityPartionsCount, TestController.PriorityPartitions.Count);
        }

        [TestMethod]
        public void CheckPartition_Test9__MultiLocationItemTest_NotCounted()
        {
            // Arrange
            Controller TestController = new Controller();

            Item TestItem1 = new Item("001", "T-Shirt", "White", "Large");
            TestItem1.ServerQuantity = 10;
            TestItem1.CountedQuantity = -1;

            Item TestItem2 = new Item("002", "Pants", "black", "Small");
            TestItem2.ServerQuantity = 10;
            TestItem2.CountedQuantity = -1;

            Location TestLocation1 = new Location("001A01");
            Location TestLocation2 = new Location("002A01");
            Location TestLocation3 = new Location("001B01");
            Location TestLocation4 = new Location("002B01");
            Location TestLocation5 = new Location("003B01");

            TestItem1.AddLocation(TestLocation1);
            TestItem1.AddLocation(TestLocation2);

            TestItem2.AddLocation(TestLocation3);
            TestItem2.AddLocation(TestLocation4);
            TestItem2.AddLocation(TestLocation5);

            Partition TestPartition1 = new Partition(true);
            TestPartition1.AddLocation(TestLocation1);

            Partition TestPartition2 = new Partition(true);
            TestPartition2.AddLocation(TestLocation3);
            TestPartition2.AddLocation(TestLocation4);

            //Expected
            int Expected_VerifiedItems = 0;
            int Expected_PartitallyCountedItemsCount = 0;
            int Expected_PriorityPartionsCount = 1;

            // Act
            TestController.CheckPartition(TestPartition1);

            // Assert
            Assert.AreEqual(Expected_VerifiedItems, TestController.ItemsVerified);
            Assert.AreEqual(Expected_PartitallyCountedItemsCount, TestController.PartiallyCountedItems.Count);
            Assert.AreEqual(Expected_PriorityPartionsCount, TestController.PriorityPartitions.Count);
        }
    }
}
