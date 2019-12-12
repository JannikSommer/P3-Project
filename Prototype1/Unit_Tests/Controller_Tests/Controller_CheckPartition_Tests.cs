using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Central_Controller;
using System.Collections.Generic;

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
            TestLocation.Visited = true;
            TestLocation.AddItem(TestItem);

            Partition TestPartition = new Partition(false);
            TestPartition.AddLocation(TestLocation);

            //Expected
            int ExpectedVerifiedItems = 1;

            // Act
            TestController.CheckPartition(TestPartition);

            // Assert
            Assert.AreEqual(ExpectedVerifiedItems, TestController.NumOfItemsVerified);
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
            Assert.AreEqual(ExpectedVerifiedItems, TestController.NumOfItemsVerified);
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
            Assert.AreEqual(ExpectedVerifiedItems, TestController.NumOfItemsVerified);
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
            TestLocation1.Visited = true;
            Location TestLocation2 = new Location("002A01");
            TestLocation2.Visited = true;

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
            Assert.AreEqual(Expected_VerifiedItems, TestController.NumOfItemsVerified);
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
            TestLocation1.Visited = true;
            Location TestLocation2 = new Location("002A01");
            TestLocation2.Visited = true;

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
            Assert.AreEqual(Expected_VerifiedItems, TestController.NumOfItemsVerified);
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
            TestLocation1.Visited = true;
            TestLocation2.Visited = true;

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
            Assert.AreEqual(Expected_VerifiedItems, TestController.NumOfItemsVerified);
            Assert.AreEqual(Expected_PartitallyCountedItemsCount, TestController.PartiallyCountedItems.Count);
        }

        [TestMethod]
        public void CheckPartition_Test7__MultiLocationItemTest_NotCounted()
        {
            // Arrange
            Controller TestController = new Controller();

            Item TestItem = new Item("001", "T-Shirt", "White", "Large");
            TestItem.ServerQuantity = 10;

            Location TestLocation1 = new Location("001A01");
            Location TestLocation2 = new Location("002A01");

            TestItem.AddLocation(TestLocation1);
            TestItem.AddLocation(TestLocation2);
            TestLocation1.Visited = false;
            TestLocation2.Visited = false;

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
            Assert.AreEqual(Expected_VerifiedItems, TestController.NumOfItemsVerified);
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
            TestLocation1.Visited = false;
            TestLocation2.Visited = false;

            Partition TestPartition = new Partition(true);
            TestPartition.AddLocation(TestLocation1);

            //Expected
            int Expected_VerifiedItems = 0;
            int Expected_PartitallyCountedItemsCount = 0;
            int Expected_PriorityPartionsCount = 1;

            // Act
            TestController.CheckPartition(TestPartition);

            // Assert
            Assert.AreEqual(Expected_VerifiedItems, TestController.NumOfItemsVerified);
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

            Item TestItem2 = new Item("002", "Pants", "black", "Small");
            TestItem2.ServerQuantity = 10;

            Location TestLocation1 = new Location("001A01");
            Location TestLocation2 = new Location("002A01");
            Location TestLocation3 = new Location("001B01");
            Location TestLocation4 = new Location("002B01");
            Location TestLocation5 = new Location("003B01");
            TestLocation1.Visited = false;
            TestLocation2.Visited = false;
            TestLocation3.Visited = false;
            TestLocation4.Visited = false;
            TestLocation5.Visited = false;

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
            Assert.AreEqual(Expected_VerifiedItems, TestController.NumOfItemsVerified);
            Assert.AreEqual(Expected_PartitallyCountedItemsCount, TestController.PartiallyCountedItems.Count);
            Assert.AreEqual(Expected_PriorityPartionsCount, TestController.PriorityPartitions.Count);
        }

        [TestMethod]
        public void CheckPartition_Test10__MultiLocationItemTest_NotCounted()
        {
            // Arrange
            Client TestClient1 = new Client("01");

            Controller TestController = new Controller();

            Item TestItem1 = new Item("001");
            List<string> TestLocation1 = new List<string> { "001A01", "002A01" };

            Item TestItem2 = new Item("002");
            List<string> TestLocation2 = new List<string> { "001B01", "002A01" };

            Item TestItem3 = new Item("003");
            List<string> TestLocation3 = new List<string> { "001C01" };

            Item TestItem4 = new Item("004");
            List<string> TestLocation4 = new List<string> { "001D01" };

            Item TestItem5 = new Item("005");
            List<string> TestLocation5 = new List<string> { "001E01" };

            //Expected
            int Expected_Partition2Size = 6;
            int Expected_ActiveClientCount = 1;

            string Expected_Partition_Location1 = "001A01";
            string Expected_Partition_Location3 = "001C01";

            // Act
            TestController.InitialAddItem(TestItem1, TestLocation1);
            TestController.InitialAddItem(TestItem2, TestLocation2);
            TestController.InitialAddItem(TestItem3, TestLocation3);
            TestController.InitialAddItem(TestItem4, TestLocation4);
            TestController.InitialAddItem(TestItem5, TestLocation5);

            TestController.InitialPartitioningOfLocations();

            TestController.AddClient(TestClient1);
            Partition DumbPartition = TestController.NextPartition(TestClient1);

            TestController.CheckPartition(DumbPartition);
            Partition Actual_Partition = TestController.NextPartition(TestClient1);

            // Assert
            Assert.AreEqual(Expected_Partition2Size, Actual_Partition.Locations.Count);
            Assert.AreEqual(Expected_ActiveClientCount, TestController.Active_Clients.Count);

            Assert.AreEqual(Expected_Partition_Location1, Actual_Partition.Locations[0].ID);
            Assert.AreEqual(Expected_Partition_Location3, Actual_Partition.Locations[2].ID);
        }

        [TestMethod]
        public void CheckPartition_Test11_MultiLocationItemTest_NotCounted()
        {
            // Arrange
            Client TestClient1 = new Client("01");

            Controller TestController = new Controller();

            Item TestItem1 = new Item("001");
            TestItem1.ServerQuantity = 10;

            Item TestItem2 = new Item("001");
            TestItem2.ServerQuantity = 10;

            Location TestLocation1 = new Location("001A01");
            Location TestLocation2 = new Location("002A01");

            TestItem1.AddLocation(TestLocation1);
            TestItem1.AddLocation(TestLocation2);

            //Expected
            int Expected_VerifiedItemsCount = 1;

            // Act
            Partition DumpPartition = new Partition(true);
            DumpPartition.AddLocation(TestLocation1);
            TestLocation1.Visited = true;
            TestItem1.CountedQuantity = 6;

            TestController.CheckPartition(DumpPartition);

            TestLocation1.Items.Clear();
            TestLocation2.Items.Clear();

            TestItem2.AddLocation(TestLocation1);
            TestItem2.AddLocation(TestLocation2);

            DumpPartition = new Partition(true);
            DumpPartition.AddLocation(TestLocation2);
            TestLocation2.Visited = true;
            TestItem2.CountedQuantity = 4;

            TestController.CheckPartition(DumpPartition);

            // Assert
            Assert.AreEqual(Expected_VerifiedItemsCount, TestController.NumOfItemsVerified);
        }
    }
}
