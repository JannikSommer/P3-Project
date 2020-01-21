using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Central_Controller.Central_Controller;

namespace Unit_Tests.Controller_Tests
{
    [TestClass]
    public class Controller_NextPartition_Tests
    {
        [TestMethod]
        public void NextPartition_Test1()
        {
            // Arrange
            Controller TestController = new Controller();
            TestController.Location_Comparer = new LocationComparer(19);

            Item TestItem1 = new Item("001", "T-shirt", "White", "Large");
            List<string> TestItem1_Locations = new List<string> { "001A01" };

            Item TestItem2 = new Item("002", "Hoddie", "Blue", "Medium");
            List<string> TestItem2_Locations = new List<string> { "001B02" };

            Item TestItem3 = new Item("003", "Pants", "Green", "Extra-Large");
            List<string> TestItem3_Locations = new List<string> { "001C03" };

            Item TestItem4 = new Item("004", "Shorts", "Red", "Small");
            List<string> TestItem4_Locations = new List<string> { "001D04" };

            Item TestItem5 = new Item("005", "Shoe", "Black", "43");
            List<string> TestItem5_Locations = new List<string> { "001E05" };

            User TestUser1 = new User("1");
            User TestUser2 = new User("2");

            //Expected
            string ExpectedItem1ID = "004";
            string ExpectedItem1Name = "Shorts";
            string ExpectedItem1Color = "Red";
            string ExpectedItem1Size = "Small";

            string ExpectedLocation1ID = "001D04";

            int ExpectedPartitionSize = 1;

            // Act
            TestController.InitialAddItem(TestItem1, TestItem1_Locations);
            TestController.InitialAddItem(TestItem2, TestItem2_Locations);
            TestController.InitialAddItem(TestItem3, TestItem3_Locations);
            TestController.InitialAddItem(TestItem4, TestItem4_Locations);
            TestController.InitialAddItem(TestItem5, TestItem5_Locations);

            TestController.InitialPartitioningOfLocations();

            TestController.AddUser(TestUser1);
            TestController.AddUser(TestUser2);

            Partition DumpPartition = TestController.NextPartition(TestUser1);
            DumpPartition = TestController.NextPartition(TestUser2);

            Partition ActualPartition = TestController.NextPartition(TestUser2);

            // Assert
            Assert.AreEqual(ExpectedPartitionSize, ActualPartition.Locations.Count);

            Assert.AreEqual(ExpectedLocation1ID, ActualPartition.Locations[0].ID);

            Assert.AreEqual(ExpectedItem1ID, ActualPartition.Locations[0].Items[0].ID);
            Assert.AreEqual(ExpectedItem1Name, ActualPartition.Locations[0].Items[0].Name);
            Assert.AreEqual(ExpectedItem1Color, ActualPartition.Locations[0].Items[0].Color);
            Assert.AreEqual(ExpectedItem1Size, ActualPartition.Locations[0].Items[0].Size);
        }

        [TestMethod]
        public void NextPartition_Test2_nullTest()
        {
            // Arrange
            Controller TestController = new Controller();
            TestController.Location_Comparer = new LocationComparer(19);

            Item TestItem1 = new Item("001", "T-shirt", "White", "Large");
            List<string> TestItem1_Locations = new List<string> { "001A01" };

            Item TestItem2 = new Item("002", "Hoddie", "Blue", "Medium");
            List<string> TestItem2_Locations = new List<string> { "001B02" };

            Item TestItem3 = new Item("003", "Pants", "Green", "Extra-Large");
            List<string> TestItem3_Locations = new List<string> { "001C03" };

            Item TestItem4 = new Item("004", "Shorts", "Red", "Small");
            List<string> TestItem4_Locations = new List<string> { "001D04" };

            Item TestItem5 = new Item("005", "Shoe", "Black", "43");
            List<string> TestItem5_Locations = new List<string> { "001E05" };

            Item TestItem6 = new Item("006", "Jacket", "Brown", "Extra-Small");
            List<string> TestItem6_Locations = new List<string> { "001F06" };

            Item TestItem7 = new Item("007", "Skjorte", "Grey", "XXL");
            List<string> TestItem7_Locations = new List<string> { "001G07" };

            User TestUser1 = new User("1");
            User TestUser2 = new User("2");

            //Expected
            Partition ExpectedPartition = null;

            // Act
            TestController.InitialAddItem(TestItem1, TestItem1_Locations);
            TestController.InitialAddItem(TestItem2, TestItem2_Locations);
            TestController.InitialAddItem(TestItem3, TestItem3_Locations);
            TestController.InitialAddItem(TestItem4, TestItem4_Locations);
            TestController.InitialAddItem(TestItem5, TestItem5_Locations);
            TestController.InitialAddItem(TestItem6, TestItem6_Locations);
            TestController.InitialAddItem(TestItem7, TestItem7_Locations);

            TestController.InitialPartitioningOfLocations();

            TestController.AddUser(TestUser1);
            TestController.AddUser(TestUser2);

            Partition DumpPartition = TestController.NextPartition(TestUser1);
            DumpPartition = TestController.NextPartition(TestUser2);
            DumpPartition = TestController.NextPartition(TestUser2);
            DumpPartition = TestController.NextPartition(TestUser2);
            DumpPartition = TestController.NextPartition(TestUser2);
            DumpPartition = TestController.NextPartition(TestUser2);
            DumpPartition = TestController.NextPartition(TestUser2);

            Partition ActualPartition = TestController.NextPartition(TestUser2);

            // Assert
            Assert.AreEqual(ExpectedPartition, ActualPartition);
        }

        [TestMethod]
        public void NextPartition_Test3_nullTest()
        {
            // Arrange
            Controller TestController = new Controller();
            TestController.Location_Comparer = new LocationComparer(19);

            Item TestItem1 = new Item("001", "T-shirt", "White", "Large");
            List<string> TestItem1_Locations = new List<string> { "001A01" };

            Item TestItem2 = new Item("002", "Hoddie", "Blue", "Medium");
            List<string> TestItem2_Locations = new List<string> { "001B02" };

            Item TestItem3 = new Item("003", "Pants", "Green", "Extra-Large");
            List<string> TestItem3_Locations = new List<string> { "001C03" };

            Item TestItem4 = new Item("004", "Shorts", "Red", "Small");
            List<string> TestItem4_Locations = new List<string> { "001D04" };

            Item TestItem5 = new Item("005", "Shoe", "Black", "43");
            List<string> TestItem5_Locations = new List<string> { "001E05" };

            Item TestItem6 = new Item("006", "Jacket", "Brown", "Extra-Small");
            List<string> TestItem6_Locations = new List<string> { "001F06" };

            Item TestItem7 = new Item("007", "Skjorte", "Grey", "XXL");
            List<string> TestItem7_Locations = new List<string> { "001G07" };

            User TestUser1 = new User("1");
            User TestUser2 = new User("2");
            User TestUser3 = new User("3");

            //Expected
            Partition ExpectedPartition = null;

            // Act
            TestController.InitialAddItem(TestItem1, TestItem1_Locations);
            TestController.InitialAddItem(TestItem2, TestItem2_Locations);
            TestController.InitialAddItem(TestItem3, TestItem3_Locations);
            TestController.InitialAddItem(TestItem4, TestItem4_Locations);
            TestController.InitialAddItem(TestItem5, TestItem5_Locations);
            TestController.InitialAddItem(TestItem6, TestItem6_Locations);
            TestController.InitialAddItem(TestItem7, TestItem7_Locations);

            TestController.InitialPartitioningOfLocations();

            TestController.AddUser(TestUser1);
            TestController.AddUser(TestUser2);
            TestController.AddUser(TestUser3);

            Partition DumpPartition = TestController.NextPartition(TestUser1);
            DumpPartition = TestController.NextPartition(TestUser2);
            DumpPartition = TestController.NextPartition(TestUser2);
            DumpPartition = TestController.NextPartition(TestUser2);
            DumpPartition = TestController.NextPartition(TestUser2);
            DumpPartition = TestController.NextPartition(TestUser2);
            DumpPartition = TestController.NextPartition(TestUser2);

            Partition ActualPartition = TestController.NextPartition(TestUser3);

            // Assert
            Assert.AreEqual(ExpectedPartition, ActualPartition);
        }
    }
}
