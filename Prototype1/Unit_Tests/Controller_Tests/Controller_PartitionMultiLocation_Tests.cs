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
            TestController.Location_Comparer = new LocationComparer(19);
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

            TestController.InitialPartitioningOfLocations();

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
            TestController.Location_Comparer = new LocationComparer(19);
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

            TestController.InitialPartitioningOfLocations();

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
            TestController.Location_Comparer = new LocationComparer(19);
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

            TestController.InitialPartitioningOfLocations();

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
            TestController.Location_Comparer = new LocationComparer(19);
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

            TestController.InitialPartitioningOfLocations();

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

        [TestMethod]
        public void PartitionMultiLocation_Test5_DevideLargerPaths()
        {
            // Arrange
            Controller TestController = new Controller();
            TestController.Location_Comparer = new LocationComparer(19);
            TestController.MaxSizeForPartitions = 12;

            Item TestItem1 = new Item("001", "T-shirt", "White", "Large");
            List<string> TestItem1_Locations = new List<string> { "001A01", "002A01", "003A01", "004A01", "005A01", "006A01", "007A01", "008A01", "009A01", "010A01" };

            Item TestItem2 = new Item("002", "Hoddie", "Blue", "Medium");
            List<string> TestItem2_Locations = new List<string> { "009A01", "010A01", "011A01", "012A01", "013A01", "014A01" };

            Client TestClient1 = new Client("1");
            Client TestClient2 = new Client("2");

            //Expected
            int Expected_TestClient1PartitionCount = 10;
            int Expected_TestClient2PartitionCount = 4;

            string Expected_TestClient1Location1ID = "001A01";
            string Expected_TestClient1Location2ID = "002A01";
            string Expected_TestClient1Location3ID = "003A01";
            string Expected_TestClient1Location4ID = "004A01";
            string Expected_TestClient1Location5ID = "005A01";
            string Expected_TestClient1Location6ID = "006A01";
            string Expected_TestClient1Location7ID = "007A01";
            string Expected_TestClient1Location8ID = "008A01";
            string Expected_TestClient1Location9ID = "009A01";
            string Expected_TestClient1Location10ID = "010A01";

            string Expected_TestClient2Location1ID = "011A01";
            string Expected_TestClient2Location2ID = "012A01";
            string Expected_TestClient2Location3ID = "013A01";
            string Expected_TestClient2Location4ID = "014A01";

            // Act
            TestController.InitialAddItem(TestItem1, TestItem1_Locations);
            TestController.InitialAddItem(TestItem2, TestItem2_Locations);

            TestController.InitialPartitioningOfLocations();

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
            Assert.AreEqual(Expected_TestClient1Location8ID, Actual_TestClient1Partition.Locations[7].ID);
            Assert.AreEqual(Expected_TestClient1Location9ID, Actual_TestClient1Partition.Locations[8].ID);
            Assert.AreEqual(Expected_TestClient1Location10ID, Actual_TestClient1Partition.Locations[9].ID);

            Assert.AreEqual(Expected_TestClient2Location1ID, Actual_TestClient2Partition.Locations[0].ID);
            Assert.AreEqual(Expected_TestClient2Location2ID, Actual_TestClient2Partition.Locations[1].ID);
            Assert.AreEqual(Expected_TestClient2Location3ID, Actual_TestClient2Partition.Locations[2].ID);
            Assert.AreEqual(Expected_TestClient2Location4ID, Actual_TestClient2Partition.Locations[3].ID);
        }

        [TestMethod]
        public void PartitionMultiLocation_Test6_MixedPartitionTest()
        {
            // Arrange
            Controller TestController = new Controller();
            TestController.Location_Comparer = new LocationComparer(19);
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

            Item TestItem10 = new Item("010");
            List<string> TestItem10_Locations = new List<string> { "005A03" };

            Item TestItem11 = new Item("011");
            List<string> TestItem11_Locations = new List<string> { "008A11", "009A11", "010A11" };

            Item TestItem12 = new Item("012");
            List<string> TestItem12_Locations = new List<string> { "009A11", "010A11", "011A11", "012A11", };

            Item TestItem13 = new Item("013");
            List<string> TestItem13_Locations = new List<string> { "001A12", "002A12", "003A12", "004A12", "005A12", "006A12" };

            Item TestItem14 = new Item("014");
            List<string> TestItem14_Locations = new List<string> { "004A12", "005A12", "006A12", "007A12", "012A12" };

            Item TestItem15 = new Item("015");
            List<string> TestItem15_Locations = new List<string> { "006A12", "007A12", "008A12", "009A12", "010A12", "011A12" };

            Client TestClient1 = new Client("1");
            Client TestClient2 = new Client("2");
            Client TestClient3 = new Client("3");

            //Expected

                //Expected TestPartition1
                int Expected_TestPartition1Count = 5; 

                string Expected_Partition1_Location1ID = "001E05";
                string Expected_Partition1_Location2ID = "001A12";
                string Expected_Partition1_Location3ID = "002A12";
                string Expected_Partition1_Location4ID = "003A12";
                string Expected_Partition1_Location5ID = "004A12";

                //Expected TestPartition2
                int Expected_TestPartition2Count = 5;

                string Expected_Partition2_Location1ID = "005A03";
                string Expected_Partition2_Location2ID = "005A12";
                string Expected_Partition2_Location3ID = "006A12";
                string Expected_Partition2_Location4ID = "007A12";
                string Expected_Partition2_Location5ID = "008A12";

                //Expected TestPartition3
                int Expected_TestPartition3Count = 4;

                string Expected_Partition3_Location1ID = "009A12";
                string Expected_Partition3_Location2ID = "010A12";
                string Expected_Partition3_Location3ID = "011A12";
                string Expected_Partition3_Location4ID = "012A12";

                //Expected TestPartition4
                int Expected_TestPartition4Count = 5;

                string Expected_Partition4_Location1ID = "001A01";
                string Expected_Partition4_Location2ID = "001D04";
                string Expected_Partition4_Location3ID = "002A01";
                string Expected_Partition4_Location4ID = "003A01";
                string Expected_Partition4_Location5ID = "004A01";

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
            TestController.InitialAddItem(TestItem10, TestItem10_Locations);
            TestController.InitialAddItem(TestItem11, TestItem11_Locations);
            TestController.InitialAddItem(TestItem12, TestItem12_Locations);
            TestController.InitialAddItem(TestItem13, TestItem13_Locations);
            TestController.InitialAddItem(TestItem14, TestItem14_Locations);
            TestController.InitialAddItem(TestItem15, TestItem15_Locations);

            TestController.InitialPartitioningOfLocations();

            TestController.AddClient(TestClient1);
            TestController.AddClient(TestClient2);
            TestController.AddClient(TestClient3);

            Partition Actual_TestPartition1 = TestController.NextPartition(TestClient1);
            Partition Actual_TestPartition2 = TestController.NextPartition(TestClient2);
            Partition Actual_TestPartition3 = TestController.NextPartition(TestClient3);
            Partition Actual_TestPartition4 = TestController.NextPartition(TestClient1);

            // Assert

                //TestPartition1
                Assert.AreEqual(Expected_TestPartition1Count, Actual_TestPartition1.Locations.Count);

                Assert.AreEqual(Expected_Partition1_Location1ID, Actual_TestPartition1.Locations[0].ID);
                Assert.AreEqual(Expected_Partition1_Location2ID, Actual_TestPartition1.Locations[1].ID);
                Assert.AreEqual(Expected_Partition1_Location3ID, Actual_TestPartition1.Locations[2].ID);
                Assert.AreEqual(Expected_Partition1_Location4ID, Actual_TestPartition1.Locations[3].ID);
                Assert.AreEqual(Expected_Partition1_Location5ID, Actual_TestPartition1.Locations[4].ID);

                //TestPartition2
                Assert.AreEqual(Expected_TestPartition2Count, Actual_TestPartition2.Locations.Count);

                Assert.AreEqual(Expected_Partition2_Location1ID, Actual_TestPartition2.Locations[0].ID);
                Assert.AreEqual(Expected_Partition2_Location2ID, Actual_TestPartition2.Locations[1].ID);
                Assert.AreEqual(Expected_Partition2_Location3ID, Actual_TestPartition2.Locations[2].ID);
                Assert.AreEqual(Expected_Partition2_Location4ID, Actual_TestPartition2.Locations[3].ID);
                Assert.AreEqual(Expected_Partition2_Location5ID, Actual_TestPartition2.Locations[4].ID);

                //TestPartition3
                Assert.AreEqual(Expected_TestPartition3Count, Actual_TestPartition3.Locations.Count);

                Assert.AreEqual(Expected_Partition3_Location1ID, Actual_TestPartition3.Locations[0].ID);
                Assert.AreEqual(Expected_Partition3_Location2ID, Actual_TestPartition3.Locations[1].ID);
                Assert.AreEqual(Expected_Partition3_Location3ID, Actual_TestPartition3.Locations[2].ID);
                Assert.AreEqual(Expected_Partition3_Location4ID, Actual_TestPartition3.Locations[3].ID);

                //TestPartition4
                Assert.AreEqual(Expected_TestPartition4Count, Actual_TestPartition4.Locations.Count);

                Assert.AreEqual(Expected_Partition4_Location1ID, Actual_TestPartition4.Locations[0].ID);
                Assert.AreEqual(Expected_Partition4_Location2ID, Actual_TestPartition4.Locations[1].ID);
                Assert.AreEqual(Expected_Partition4_Location3ID, Actual_TestPartition4.Locations[2].ID);
                Assert.AreEqual(Expected_Partition4_Location4ID, Actual_TestPartition4.Locations[3].ID);
                Assert.AreEqual(Expected_Partition4_Location5ID, Actual_TestPartition4.Locations[4].ID);
        }

        [TestMethod]
        public void PartitionMultiLocation_Test7_MixedPartitionTest()
        {
            // Arrange
            Controller TestController = new Controller();
            TestController.Location_Comparer = new LocationComparer(19);
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

            Item TestItem10 = new Item("010");
            List<string> TestItem10_Locations = new List<string> { "005A03" };

            Item TestItem11 = new Item("011");
            List<string> TestItem11_Locations = new List<string> { "007A11", "008A11", "009A11", "010A11" };

            Item TestItem12 = new Item("012");
            List<string> TestItem12_Locations = new List<string> { "009A11", "010A11", "011A11", "012A11", };

            Item TestItem13 = new Item("013");
            List<string> TestItem13_Locations = new List<string> { "001A12", "002A12", "003A12", "004A12", "005A12", "006A12" };

            Item TestItem14 = new Item("014");
            List<string> TestItem14_Locations = new List<string> { "004A12", "005A12", "006A12", "007A12", "012A12" };

            Item TestItem15 = new Item("015");
            List<string> TestItem15_Locations = new List<string> { "006A12", "007A12", "008A12", "009A12", "010A12", "011A12" };

            Client TestClient1 = new Client("1");
            Client TestClient2 = new Client("2");
            Client TestClient3 = new Client("3");

            //Expected

                //Expected TestPartition1
                int Expected_TestPartition1Count = 4;

                string Expected_Partition1_Location1ID = "007A11";
                string Expected_Partition1_Location2ID = "008A11";
                string Expected_Partition1_Location3ID = "009A11";
                string Expected_Partition1_Location4ID = "010A11";

                //Expected TestPartition2
                int Expected_TestPartition2Count = 2;

                string Expected_Partition2_Location1ID = "011A11";
                string Expected_Partition2_Location2ID = "012A11";

                //Expected TestPartition3
                int Expected_TestPartition3Count = 5;

                string Expected_Partition3_Location1ID = "001A01";
                string Expected_Partition3_Location2ID = "001D04";
                string Expected_Partition3_Location3ID = "002A01";
                string Expected_Partition3_Location4ID = "003A01";
                string Expected_Partition3_Location5ID = "004A01";

                //Expected TestPartition4
                int Expected_TestPartition4Count = 5;

                string Expected_Partition4_Location1ID = "001E05";
                string Expected_Partition4_Location2ID = "001A12";
                string Expected_Partition4_Location3ID = "002A12";
                string Expected_Partition4_Location4ID = "003A12";
                string Expected_Partition4_Location5ID = "004A12";

                //Expected TestPartition5
                int Expected_TestPartition5Count = 5;

                string Expected_Partition5_Location1ID = "005A03";
                string Expected_Partition5_Location2ID = "005A12";
                string Expected_Partition5_Location3ID = "006A12";
                string Expected_Partition5_Location4ID = "007A12";
                string Expected_Partition5_Location5ID = "008A12";

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
            TestController.InitialAddItem(TestItem10, TestItem10_Locations);
            TestController.InitialAddItem(TestItem11, TestItem11_Locations);
            TestController.InitialAddItem(TestItem12, TestItem12_Locations);
            TestController.InitialAddItem(TestItem13, TestItem13_Locations);
            TestController.InitialAddItem(TestItem14, TestItem14_Locations);
            TestController.InitialAddItem(TestItem15, TestItem15_Locations);

            TestController.InitialPartitioningOfLocations();

            TestController.AddClient(TestClient1);
            TestController.AddClient(TestClient2);

            Partition Actual_TestPartition1 = TestController.NextPartition(TestClient1);
            Partition Actual_TestPartition2 = TestController.NextPartition(TestClient2);
            Partition Actual_TestPartition3 = TestController.NextPartition(TestClient1);

            TestController.AddClient(TestClient3);

            Partition Actual_TestPartition4 = TestController.NextPartition(TestClient3);
            Partition Actual_TestPartition5 = TestController.NextPartition(TestClient2);

            // Assert

            //TestPartition1
            Assert.AreEqual(Expected_TestPartition1Count, Actual_TestPartition1.Locations.Count);

            Assert.AreEqual(Expected_Partition1_Location1ID, Actual_TestPartition1.Locations[0].ID);
            Assert.AreEqual(Expected_Partition1_Location2ID, Actual_TestPartition1.Locations[1].ID);
            Assert.AreEqual(Expected_Partition1_Location3ID, Actual_TestPartition1.Locations[2].ID);
            Assert.AreEqual(Expected_Partition1_Location4ID, Actual_TestPartition1.Locations[3].ID);

            //TestPartition2
            Assert.AreEqual(Expected_TestPartition2Count, Actual_TestPartition2.Locations.Count);

            Assert.AreEqual(Expected_Partition2_Location1ID, Actual_TestPartition2.Locations[0].ID);
            Assert.AreEqual(Expected_Partition2_Location2ID, Actual_TestPartition2.Locations[1].ID);

            //TestPartition3
            Assert.AreEqual(Expected_TestPartition3Count, Actual_TestPartition3.Locations.Count);

            Assert.AreEqual(Expected_Partition3_Location1ID, Actual_TestPartition3.Locations[0].ID);
            Assert.AreEqual(Expected_Partition3_Location2ID, Actual_TestPartition3.Locations[1].ID);
            Assert.AreEqual(Expected_Partition3_Location3ID, Actual_TestPartition3.Locations[2].ID);
            Assert.AreEqual(Expected_Partition3_Location4ID, Actual_TestPartition3.Locations[3].ID);
            Assert.AreEqual(Expected_Partition3_Location5ID, Actual_TestPartition3.Locations[4].ID);

            //TestPartition4
            Assert.AreEqual(Expected_TestPartition4Count, Actual_TestPartition4.Locations.Count);

            Assert.AreEqual(Expected_Partition4_Location1ID, Actual_TestPartition4.Locations[0].ID);
            Assert.AreEqual(Expected_Partition4_Location2ID, Actual_TestPartition4.Locations[1].ID);
            Assert.AreEqual(Expected_Partition4_Location3ID, Actual_TestPartition4.Locations[2].ID);
            Assert.AreEqual(Expected_Partition4_Location4ID, Actual_TestPartition4.Locations[3].ID);
            Assert.AreEqual(Expected_Partition4_Location5ID, Actual_TestPartition4.Locations[4].ID);

            //TestPartition5
            Assert.AreEqual(Expected_TestPartition5Count, Actual_TestPartition5.Locations.Count);

            Assert.AreEqual(Expected_Partition5_Location1ID, Actual_TestPartition5.Locations[0].ID);
            Assert.AreEqual(Expected_Partition5_Location2ID, Actual_TestPartition5.Locations[1].ID);
            Assert.AreEqual(Expected_Partition5_Location3ID, Actual_TestPartition5.Locations[2].ID);
            Assert.AreEqual(Expected_Partition5_Location4ID, Actual_TestPartition5.Locations[3].ID);
            Assert.AreEqual(Expected_Partition5_Location5ID, Actual_TestPartition5.Locations[4].ID);
        }
    }
}
