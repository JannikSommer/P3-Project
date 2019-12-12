using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Central_Controller;
using System.Collections.Generic;

namespace Unit_Tests.Controller_Tests
{
    [TestClass]
    public class Controller_CheckForAFKclients_Tests
    {
        [TestMethod]
        public void CheckForAFKclients_Test1_SingleLocationPartition()
        {
            // Arrange
            Client TestClient1 = new Client("01");
            Client TestClient2 = new Client("02");

            Controller TestController = new Controller();
            TestController.Location_Comparer = new LocationComparer(19);
            TestController.TimeBeforeAFK = new TimeSpan(0, 0, 0);

            Item TestItem1 = new Item("001");
            List<string> TestLocation1 = new List<string> { "001A01" };

            Item TestItem2 = new Item("002");
            List<string> TestLocation2 = new List<string> { "001B01" };

            Item TestItem3 = new Item("003");
            List<string> TestLocation3 = new List<string> { "001C01" };

            Item TestItem4 = new Item("004");
            List<string> TestLocation4 = new List<string> { "001D01" };

            Item TestItem5 = new Item("005");
            List<string> TestLocation5 = new List<string> { "001E01" };

            //Expected
            int Expected_PartitionSize = 5;
            int Expected_ActiveClientCount = 1;

            string Expected_1stItemID = "001";
            string Expected_2ndItemID = "002";
            string Expected_5thItemID = "005";


            // Act
            TestController.InitialAddItem(TestItem1, TestLocation1);
            TestController.InitialAddItem(TestItem2, TestLocation2);
            TestController.InitialAddItem(TestItem3, TestLocation3);
            TestController.InitialAddItem(TestItem4, TestLocation4);
            TestController.InitialAddItem(TestItem5, TestLocation5);

            TestController.InitialPartitioningOfLocations();

            TestController.AddClient(TestClient1);
            TestController.NextPartition(TestClient1);

            TestController.CheckForAFKclients();

            TestController.AddClient(TestClient2);
            Partition Actual_Partition = TestController.NextPartition(TestClient2);

            // Assert
            Assert.AreEqual(Expected_PartitionSize, Actual_Partition.Locations.Count);
            Assert.AreEqual(Expected_ActiveClientCount, TestController.Active_Clients.Count);

            Assert.AreEqual(Expected_1stItemID, Actual_Partition.Locations[0].Items[0].ID);
            Assert.AreEqual(Expected_2ndItemID, Actual_Partition.Locations[1].Items[0].ID);
            Assert.AreEqual(Expected_5thItemID, Actual_Partition.Locations[4].Items[0].ID);
        }

        [TestMethod]
        public void CheckForAFKclients_Test2_MultiLocationPartition()
        {
            // Arrange
            Client TestClient1 = new Client("01");
            Client TestClient2 = new Client("02");

            Controller TestController = new Controller();
            TestController.Location_Comparer = new LocationComparer(19);
            TestController.TimeBeforeAFK = new TimeSpan(0, 0, 0);

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

            TestController.CheckForAFKclients();

            TestController.AddClient(TestClient2);
            Partition Actual_Partition = TestController.NextPartition(TestClient2);

            // Assert
            Assert.AreEqual(Expected_Partition2Size, Actual_Partition.Locations.Count);
            Assert.AreEqual(Expected_ActiveClientCount, TestController.Active_Clients.Count);

            Assert.AreEqual(Expected_Partition_Location1, Actual_Partition.Locations[0].ID);
            Assert.AreEqual(Expected_Partition_Location3, Actual_Partition.Locations[2].ID);
        }
    }
}
