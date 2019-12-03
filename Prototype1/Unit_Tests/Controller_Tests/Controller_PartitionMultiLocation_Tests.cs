using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Central_Controller;
using Model;
using System.Collections.Generic;

namespace Unit_Tests.Controller_Tests
{
    [TestClass]
    public class Controller_PartitionMultiLocation_Tests
    {
        [TestMethod]
        public void PartitionMultiLocation_Test1_CombineShorterPaths()
        {
            // Arrange
            Controller TestController = new Controller();
            TestController.MaxSizeForPartitions = 5;

            Item TestItem1 = new Item("001", "T-shirt", "White", "Large");
            List<string> TestItem1_Locations = new List<string> { "001A01", "002A01" };

            Item TestItem2 = new Item("002", "Hoddie", "Blue", "Medium");
            List<string> TestItem2_Locations = new List<string> { "001B02", "002B01" };

            Item TestItem3 = new Item("003", "Pants", "Green", "Extra-Large");
            List<string> TestItem3_Locations = new List<string> { "001C03", "002C03" };

            Item TestItem4 = new Item("004", "Shorts", "Red", "Small");
            List<string> TestItem4_Locations = new List<string> { "001D04" };

            Item TestItem5 = new Item("005", "Shoe", "Black", "43");
            List<string> TestItem5_Locations = new List<string> { "001E05" };

            Item TestItem6 = new Item("006", "Jacket", "Brown", "Extra-Small");
            List<string> TestItem6_Locations = new List<string> { "001F06" };

            Item TestItem7 = new Item("007", "Skjorte", "Grey", "XXL");
            List<string> TestItem7_Locations = new List<string> { "001G07" };

            Client TestClient1 = new Client("1");
            Client TestClient2 = new Client("2");

            //Expected
            int Expected_TestClient1PartitionCount = 5;
            int Expected_TestClient2PartitionCount = 5;

            string Expected_TestClient1Location1ID = "001A01";
            string Expected_TestClient1Location2ID = "001B02";
            string Expected_TestClient1Location3ID = "001D04";
            string Expected_TestClient1Location4ID = "002A01";
            string Expected_TestClient1Location5ID = "002B01";

            string Expected_TestClient2Location1ID = "001C03";
            string Expected_TestClient2Location2ID = "001E05";
            string Expected_TestClient2Location3ID = "001F06";
            string Expected_TestClient2Location4ID = "001G07";
            string Expected_TestClient2Location5ID = "002C03";

            // Act
            TestController.InitialAddItem(TestItem1, TestItem1_Locations);
            TestController.InitialAddItem(TestItem2, TestItem2_Locations);
            TestController.InitialAddItem(TestItem3, TestItem3_Locations);
            TestController.InitialAddItem(TestItem4, TestItem4_Locations);
            TestController.InitialAddItem(TestItem5, TestItem5_Locations);
            TestController.InitialAddItem(TestItem6, TestItem6_Locations);
            TestController.InitialAddItem(TestItem7, TestItem7_Locations);

            TestController.InitialPartitionUnpartitionedLocations();

            TestController.AddClient(TestClient1);
            TestController.AddClient(TestClient2);

            Partition Actual_TestClient1Partition = TestController.NextPartition(TestClient1);
            Partition Actual_TestClient2Partition = TestController.NextPartition(TestClient2);

            // Assert
            Assert.AreEqual(Expected_TestClient1PartitionCount, Actual_TestClient1Partition.Locations.Count);
            Assert.AreEqual(Expected_TestClient2PartitionCount, Actual_TestClient2Partition.Locations.Count);

            Assert.AreEqual(Expected_TestClient1Location1ID, Actual_TestClient1Partition.Locations[0].ID);
            Assert.AreEqual(Expected_TestClient1Location2ID, Actual_TestClient1Partition.Locations[1].ID);
            Assert.AreEqual(Expected_TestClient1Location3ID, Actual_TestClient1Partition.Locations[2].ID);
            Assert.AreEqual(Expected_TestClient1Location4ID, Actual_TestClient1Partition.Locations[3].ID);
            Assert.AreEqual(Expected_TestClient1Location5ID, Actual_TestClient1Partition.Locations[4].ID);

            Assert.AreEqual(Expected_TestClient2Location1ID, Actual_TestClient2Partition.Locations[0].ID);
            Assert.AreEqual(Expected_TestClient2Location2ID, Actual_TestClient2Partition.Locations[1].ID);
            Assert.AreEqual(Expected_TestClient2Location3ID, Actual_TestClient2Partition.Locations[2].ID);
            Assert.AreEqual(Expected_TestClient2Location4ID, Actual_TestClient2Partition.Locations[3].ID);
            Assert.AreEqual(Expected_TestClient2Location5ID, Actual_TestClient2Partition.Locations[4].ID);
        }

        [TestMethod]
        public void PartitionMultiLocation_Test2_CombineShorterPaths()
        {
            // Arrange
            Controller TestController = new Controller();
            TestController.MaxSizeForPartitions = 5;

            Item TestItem1 = new Item("001", "T-shirt", "White", "Large");
            List<string> TestItem1_Locations = new List<string> { "001A01", "002A01", "003A01", "004A01" };

            Item TestItem2 = new Item("002", "Hoddie", "Blue", "Medium");
            List<string> TestItem2_Locations = new List<string> { "001B02", "002B01" };

            Item TestItem3 = new Item("003", "Pants", "Green", "Extra-Large");
            List<string> TestItem3_Locations = new List<string> { "001C03", "001C04" };

            Item TestItem4 = new Item("004", "Shorts", "Red", "Small");
            List<string> TestItem4_Locations = new List<string> { "001D04" };

            //Item TestItem5 = new Item("005", "Shoe", "Black", "43");
            //List<string> TestItem5_Locations = new List<string> { "001E05" };

            //Item TestItem6 = new Item("006", "Jacket", "Brown", "Extra-Small");
            //List<string> TestItem6_Locations = new List<string> { "001F06" };

            //Item TestItem7 = new Item("007", "Skjorte", "Grey", "XXL");
            //List<string> TestItem7_Locations = new List<string> { "001G07" };

            Client TestClient1 = new Client("1");
            Client TestClient2 = new Client("2");

            //Expected
            int Expected_TestClient1PartitionCount = 5;
            int Expected_TestClient2PartitionCount = 4;

            string Expected_TestClient1Location1ID = "001A01";
            string Expected_TestClient1Location2ID = "001D04";
            string Expected_TestClient1Location3ID = "002A01";
            string Expected_TestClient1Location4ID = "003A01";
            string Expected_TestClient1Location5ID = "004A01";

            string Expected_TestClient2Location1ID = "001B02";
            string Expected_TestClient2Location2ID = "001C03";
            string Expected_TestClient2Location3ID = "001C04";
            string Expected_TestClient2Location4ID = "002B01";

            // Act
            TestController.InitialAddItem(TestItem1, TestItem1_Locations);
            TestController.InitialAddItem(TestItem2, TestItem2_Locations);
            TestController.InitialAddItem(TestItem3, TestItem3_Locations);
            TestController.InitialAddItem(TestItem4, TestItem4_Locations);
            //TestController.InitialAddItem(TestItem5, TestItem5_Locations);
            //TestController.InitialAddItem(TestItem6, TestItem6_Locations);
            //TestController.InitialAddItem(TestItem7, TestItem7_Locations);

            TestController.InitialPartitionUnpartitionedLocations();

            TestController.AddClient(TestClient1);
            TestController.AddClient(TestClient2);

            Partition Actual_TestClient1Partition = TestController.NextPartition(TestClient1);
            Partition Actual_TestClient2Partition = TestController.NextPartition(TestClient2);

            // Assert
            Assert.AreEqual(Expected_TestClient1PartitionCount, Actual_TestClient1Partition.Locations.Count);
            Assert.AreEqual(Expected_TestClient2PartitionCount, Actual_TestClient2Partition.Locations.Count);

            Assert.AreEqual(Expected_TestClient1Location1ID, Actual_TestClient1Partition.Locations[0].ID);
            Assert.AreEqual(Expected_TestClient1Location2ID, Actual_TestClient1Partition.Locations[1].ID);
            Assert.AreEqual(Expected_TestClient1Location3ID, Actual_TestClient1Partition.Locations[2].ID);
            Assert.AreEqual(Expected_TestClient1Location4ID, Actual_TestClient1Partition.Locations[3].ID);
            Assert.AreEqual(Expected_TestClient1Location5ID, Actual_TestClient1Partition.Locations[4].ID);

            Assert.AreEqual(Expected_TestClient2Location1ID, Actual_TestClient2Partition.Locations[0].ID);
            Assert.AreEqual(Expected_TestClient2Location2ID, Actual_TestClient2Partition.Locations[1].ID);
            Assert.AreEqual(Expected_TestClient2Location3ID, Actual_TestClient2Partition.Locations[2].ID);
            Assert.AreEqual(Expected_TestClient2Location4ID, Actual_TestClient2Partition.Locations[3].ID);
            //Assert.AreEqual(Expected_TestClient2Location5ID, Actual_TestClient2Partition.Locations[4].ID);
        }

        [TestMethod]
        public void PartitionMultiLocation_Test3_MultiLocationPatitions_And_SingleLocationPartitions()
        {
            // Arrange
            Controller TestController = new Controller();
            TestController.MaxSizeForPartitions = 5;

            Item TestItem1 = new Item("001", "T-shirt", "White", "Large");
            List<string> TestItem1_Locations = new List<string> { "001A01", "002A01", "003A01", "004A01" };

            Item TestItem2 = new Item("002", "Hoddie", "Blue", "Medium");
            List<string> TestItem2_Locations = new List<string> { "001B02", "002B01" };

            Item TestItem3 = new Item("003", "Pants", "Green", "Extra-Large");
            List<string> TestItem3_Locations = new List<string> { "001C03", "001C04" };

            Item TestItem4 = new Item("004", "Shorts", "Red", "Small");
            List<string> TestItem4_Locations = new List<string> { "001D04" };

            Item TestItem5 = new Item("005", "Shoe", "Black", "43");
            List<string> TestItem5_Locations = new List<string> { "001E05" };

            Item TestItem6 = new Item("006", "Jacket", "Brown", "Extra-Small");
            List<string> TestItem6_Locations = new List<string> { "005F01" };

            Item TestItem7 = new Item("007", "Skjorte", "Grey", "XXL");
            List<string> TestItem7_Locations = new List<string> { "005G01" };

            Item TestItem8 = new Item("008");
            List<string> TestItem8_Locations = new List<string> { "005A01" };

            Item TestItem9 = new Item("009");
            List<string> TestItem9_Locations = new List<string> { "005B01" };

            //Item TestItem10 = new Item("010");
            //List<string> TestItem10_Locations = new List<string> { "005A03" };

            Client TestClient1 = new Client("1");
            Client TestClient2 = new Client("2");
            Client TestClient3 = new Client("3");

            //Expected
            int Expected_TestClient1PartitionCount = 5;
            int Expected_TestClient2PartitionCount = 5;
            int Expected_TestClient3PartitionCount = 4;

            string Expected_TestClient1Location1ID = "001A01";
            string Expected_TestClient1Location2ID = "001D04";
            string Expected_TestClient1Location3ID = "002A01";
            string Expected_TestClient1Location4ID = "003A01";
            string Expected_TestClient1Location5ID = "004A01";

            string Expected_TestClient2Location1ID = "001B02";
            string Expected_TestClient2Location2ID = "001C03";
            string Expected_TestClient2Location3ID = "001C04";
            string Expected_TestClient2Location4ID = "001E05";
            string Expected_TestClient2Location5ID = "002B01";

            string Expected_TestClient3Location1ID = "005A01";
            string Expected_TestClient3Location2ID = "005B01";
            string Expected_TestClient3Location3ID = "005F01";
            string Expected_TestClient3Location4ID = "005G01";

            // Act
            TestController.InitialAddItem(TestItem1, TestItem1_Locations);
            TestController.InitialAddItem(TestItem2, TestItem2_Locations);
            TestController.InitialAddItem(TestItem3, TestItem3_Locations);
            TestController.InitialAddItem(TestItem4, TestItem4_Locations);
            TestController.InitialAddItem(TestItem5, TestItem5_Locations);
            TestController.InitialAddItem(TestItem6, TestItem6_Locations);
            TestController.InitialAddItem(TestItem7, TestItem7_Locations);
            TestController.InitialAddItem(TestItem8, TestItem8_Locations);
            TestController.InitialAddItem(TestItem9, TestItem9_Locations);

            TestController.InitialPartitionUnpartitionedLocations();

            TestController.AddClient(TestClient1);
            TestController.AddClient(TestClient2);
            TestController.AddClient(TestClient3);

            Partition Actual_TestClient1Partition = TestController.NextPartition(TestClient1);
            Partition Actual_TestClient2Partition = TestController.NextPartition(TestClient2);
            Partition Actual_TestClient3Partition = TestController.NextPartition(TestClient3);

            // Assert
            Assert.AreEqual(Expected_TestClient1PartitionCount, Actual_TestClient1Partition.Locations.Count);
            Assert.AreEqual(Expected_TestClient2PartitionCount, Actual_TestClient2Partition.Locations.Count);
            Assert.AreEqual(Expected_TestClient3PartitionCount, Actual_TestClient3Partition.Locations.Count);

            Assert.AreEqual(Expected_TestClient1Location1ID, Actual_TestClient1Partition.Locations[0].ID);
            Assert.AreEqual(Expected_TestClient1Location2ID, Actual_TestClient1Partition.Locations[1].ID);
            Assert.AreEqual(Expected_TestClient1Location3ID, Actual_TestClient1Partition.Locations[2].ID);
            Assert.AreEqual(Expected_TestClient1Location4ID, Actual_TestClient1Partition.Locations[3].ID);
            Assert.AreEqual(Expected_TestClient1Location5ID, Actual_TestClient1Partition.Locations[4].ID);

            Assert.AreEqual(Expected_TestClient2Location1ID, Actual_TestClient2Partition.Locations[0].ID);
            Assert.AreEqual(Expected_TestClient2Location2ID, Actual_TestClient2Partition.Locations[1].ID);
            Assert.AreEqual(Expected_TestClient2Location3ID, Actual_TestClient2Partition.Locations[2].ID);
            Assert.AreEqual(Expected_TestClient2Location4ID, Actual_TestClient2Partition.Locations[3].ID);
            Assert.AreEqual(Expected_TestClient2Location5ID, Actual_TestClient2Partition.Locations[4].ID);

            Assert.AreEqual(Expected_TestClient3Location1ID, Actual_TestClient3Partition.Locations[0].ID);
            Assert.AreEqual(Expected_TestClient3Location2ID, Actual_TestClient3Partition.Locations[1].ID);
            Assert.AreEqual(Expected_TestClient3Location3ID, Actual_TestClient3Partition.Locations[2].ID);
            Assert.AreEqual(Expected_TestClient3Location4ID, Actual_TestClient3Partition.Locations[3].ID);
            //Assert.AreEqual(Expected_TestClient2Location5ID, Actual_TestClient2Partition.Locations[4].ID);
        }

        [TestMethod]
        public void PartitionMultiLocation_Test4_DevideLargerPaths()
        {
            // Arrange
            Controller TestController = new Controller();
            TestController.MaxSizeForPartitions = 8;

            Item TestItem1 = new Item("001", "T-shirt", "White", "Large");
            List<string> TestItem1_Locations = new List<string> { "001A01", "002A01", "003A01", "004A01", "005A01", "006A01" };

            Item TestItem2 = new Item("002", "Hoddie", "Blue", "Medium");
            List<string> TestItem2_Locations = new List<string> { "004A01", "005A01", "006A01", "007A01" };

            Item TestItem3 = new Item("003", "Pants", "Green", "Extra-Large");
            List<string> TestItem3_Locations = new List<string> { "006A01", "007A01", "008A01", "009A01", "010A01" };

            Client TestClient1 = new Client("1");
            Client TestClient2 = new Client("2");

            //Expected
            int Expected_TestClient1PartitionCount = 7;
            int Expected_TestClient2PartitionCount = 3;

            string Expected_TestClient1Location1ID = "001A01";
            string Expected_TestClient1Location2ID = "002A01";
            string Expected_TestClient1Location3ID = "003A01";
            string Expected_TestClient1Location4ID = "004A01";
            string Expected_TestClient1Location5ID = "005A01";
            string Expected_TestClient1Location6ID = "006A01";
            string Expected_TestClient1Location7ID = "007A01";

            string Expected_TestClient2Location1ID = "008A01";
            string Expected_TestClient2Location2ID = "009A01";
            string Expected_TestClient2Location3ID = "010A01";

            // Act
            TestController.InitialAddItem(TestItem1, TestItem1_Locations);
            TestController.InitialAddItem(TestItem2, TestItem2_Locations);
            TestController.InitialAddItem(TestItem3, TestItem3_Locations);

            TestController.InitialPartitionUnpartitionedLocations();

            TestController.AddClient(TestClient1);
            TestController.AddClient(TestClient2);

            Partition Actual_TestClient1Partition = TestController.NextPartition(TestClient1);
            Partition Actual_TestClient2Partition = TestController.NextPartition(TestClient2);

            // Assert
            Assert.AreEqual(Expected_TestClient1PartitionCount, Actual_TestClient1Partition.Locations.Count);
            Assert.AreEqual(Expected_TestClient2PartitionCount, Actual_TestClient2Partition.Locations.Count);

            Assert.AreEqual(Expected_TestClient1Location1ID, Actual_TestClient1Partition.Locations[0].ID);
            Assert.AreEqual(Expected_TestClient1Location2ID, Actual_TestClient1Partition.Locations[1].ID);
            Assert.AreEqual(Expected_TestClient1Location3ID, Actual_TestClient1Partition.Locations[2].ID);
            Assert.AreEqual(Expected_TestClient1Location4ID, Actual_TestClient1Partition.Locations[3].ID);
            Assert.AreEqual(Expected_TestClient1Location5ID, Actual_TestClient1Partition.Locations[4].ID);
            Assert.AreEqual(Expected_TestClient1Location6ID, Actual_TestClient1Partition.Locations[5].ID);
            Assert.AreEqual(Expected_TestClient1Location7ID, Actual_TestClient1Partition.Locations[6].ID);

            Assert.AreEqual(Expected_TestClient2Location1ID, Actual_TestClient2Partition.Locations[0].ID);
            Assert.AreEqual(Expected_TestClient2Location2ID, Actual_TestClient2Partition.Locations[1].ID);
            Assert.AreEqual(Expected_TestClient2Location3ID, Actual_TestClient2Partition.Locations[2].ID);
        }
    }
}
