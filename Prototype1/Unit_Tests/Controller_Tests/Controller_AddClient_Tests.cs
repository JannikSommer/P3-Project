using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Central_Controller.Central_Controller;

namespace Unit_Tests.Controller_Tests
{
    [TestClass]
    public class Controller_AddUser_Tests
    {
        [TestMethod]
        public void AddUser_Test1()
        {
            // Arrange
            Controller TestController = new Controller();
            TestController.Location_Comparer = new LocationComparer(19);

            Item TestItem1 = new Item("001", "T-shirt", "White", "Large");
            List<string> TestItem1_Locations = new List<string> { "001A01" };

            Item TestItem2 = new Item("002", "Hoddie", "Blue", "Medium");
            List<string> TestItem2_Locations = new List<string> { "001B01" };

            Item TestItem3 = new Item("003", "Pants", "Green", "Extra-Large");
            List<string> TestItem3_Locations = new List<string> { "001C01" };

            Item TestItem4 = new Item("004", "Shorts", "Red", "Small");
            List<string> TestItem4_Locations = new List<string> { "001D01" };

            Item TestItem5 = new Item("005", "Shoe", "Black", "43");
            List<string> TestItem5_Locations = new List<string> { "001E01" };

            User TestUser1 = new User("1");

            //Expected
            string ExpectedItem1ID = "001";
            string ExpectedItem1Name = "T-shirt";
            string ExpectedItem1Color = "White";
            string ExpectedItem1Size = "Large";

            string ExpectedLocation1ID = "001A01";

            int ExpectedPartitionSize = 5;

            // Act
            TestController.InitialAddItem(TestItem1, TestItem1_Locations);
            TestController.InitialAddItem(TestItem2, TestItem2_Locations);
            TestController.InitialAddItem(TestItem3, TestItem3_Locations);
            TestController.InitialAddItem(TestItem4, TestItem4_Locations);
            TestController.InitialAddItem(TestItem5, TestItem5_Locations);

            TestController.InitialPartitioningOfLocations();

            TestController.AddUser(TestUser1);

            Partition ActualPartition = TestController.NextPartition(TestUser1);

            // Assert
            Assert.AreEqual(ExpectedPartitionSize, ActualPartition.Locations.Count);

            Assert.AreEqual(ExpectedLocation1ID, ActualPartition.Locations[0].ID);

            Assert.AreEqual(ExpectedItem1ID, ActualPartition.Locations[0].Items[0].ID);
            Assert.AreEqual(ExpectedItem1Name, ActualPartition.Locations[0].Items[0].Name);
            Assert.AreEqual(ExpectedItem1Color, ActualPartition.Locations[0].Items[0].Color);
            Assert.AreEqual(ExpectedItem1Size, ActualPartition.Locations[0].Items[0].Size);
        }

        [TestMethod]
        public void AddUser_Test2()
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
            string ExpectedItem1ID = "005";
            string ExpectedItem1Name = "Shoe";
            string ExpectedItem1Color = "Black";
            string ExpectedItem1Size = "43";

            string ExpectedLocation1ID = "001E05";

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
        public void AddUser_Test3()
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
            string ExpectedItem1ID = "003";
            string ExpectedItem1Name = "Pants";
            string ExpectedItem1Color = "Green";
            string ExpectedItem1Size = "Extra-Large";

            string ExpectedLocation1ID = "001C03";

            int ExpectedPartitionSize = 1;

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

            Partition ActualPartition = TestController.NextPartition(TestUser3);

            // Assert
            Assert.AreEqual(ExpectedPartitionSize, ActualPartition.Locations.Count);

            Assert.AreEqual(ExpectedLocation1ID, ActualPartition.Locations[0].ID);

            Assert.AreEqual(ExpectedItem1ID, ActualPartition.Locations[0].Items[0].ID);
            Assert.AreEqual(ExpectedItem1Name, ActualPartition.Locations[0].Items[0].Name);
            Assert.AreEqual(ExpectedItem1Color, ActualPartition.Locations[0].Items[0].Color);
            Assert.AreEqual(ExpectedItem1Size, ActualPartition.Locations[0].Items[0].Size);
        }

        [TestMethod]
        public void AddUser_Test4()
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
            string ExpectedItem1ID = "001";
            string ExpectedItem1Name = "T-shirt";
            string ExpectedItem1Color = "White";
            string ExpectedItem1Size = "Large";

            string ExpectedLocation1ID = "001A01";

            int ExpectedPartitionSize = 1;

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

            TestController.RemoveUser(0);

            Partition ActualPartition = TestController.NextPartition(TestUser3); //expected test item 1

            // Assert
            Assert.AreEqual(ExpectedPartitionSize, ActualPartition.Locations.Count);

            Assert.AreEqual(ExpectedLocation1ID, ActualPartition.Locations[0].ID);

            Assert.AreEqual(ExpectedItem1ID, ActualPartition.Locations[0].Items[0].ID);
            Assert.AreEqual(ExpectedItem1Name, ActualPartition.Locations[0].Items[0].Name);
            Assert.AreEqual(ExpectedItem1Color, ActualPartition.Locations[0].Items[0].Color);
            Assert.AreEqual(ExpectedItem1Size, ActualPartition.Locations[0].Items[0].Size);
        }

        [TestMethod]
        public void AddUser_Test5()
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
            string ExpectedItem1ID = "007";
            string ExpectedItem1Name = "Skjorte";
            string ExpectedItem1Color = "Grey";
            string ExpectedItem1Size = "XXL";

            string ExpectedLocation1ID = "001G07";

            int ExpectedPartitionSize = 1;

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

            TestController.RemoveUser(1);

            Partition ActualPartition = TestController.NextPartition(TestUser3); //expected test item 7

            // Assert
            Assert.AreEqual(ExpectedPartitionSize, ActualPartition.Locations.Count);

            Assert.AreEqual(ExpectedLocation1ID, ActualPartition.Locations[0].ID);

            Assert.AreEqual(ExpectedItem1ID, ActualPartition.Locations[0].Items[0].ID);
            Assert.AreEqual(ExpectedItem1Name, ActualPartition.Locations[0].Items[0].Name);
            Assert.AreEqual(ExpectedItem1Color, ActualPartition.Locations[0].Items[0].Color);
            Assert.AreEqual(ExpectedItem1Size, ActualPartition.Locations[0].Items[0].Size);
        }
        [TestMethod]
        public void AddUser_Test6()
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
            List<string> TestItem6_Locations = new List<string> { "002F06" };

            Item TestItem7 = new Item("007", "Skjorte", "Grey", "XXL");
            List<string> TestItem7_Locations = new List<string> { "002G07" };

            User TestUser1 = new User("1");
            User TestUser2 = new User("2");

            //Expected
            string ExpectedItem1ID = "006";
            string ExpectedItem1Name = "Jacket";
            string ExpectedItem1Color = "Brown";
            string ExpectedItem1Size = "Extra-Small";

            string ExpectedLocation1ID = "002F06";

            int ExpectedPartitionSize = 1;

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

            Partition ActualPartition = TestController.NextPartition(TestUser2); //expected test item 6

            // Assert
            Assert.AreEqual(ExpectedPartitionSize, ActualPartition.Locations.Count);

            Assert.AreEqual(ExpectedLocation1ID, ActualPartition.Locations[0].ID);

            Assert.AreEqual(ExpectedItem1ID, ActualPartition.Locations[0].Items[0].ID);
            Assert.AreEqual(ExpectedItem1Name, ActualPartition.Locations[0].Items[0].Name);
            Assert.AreEqual(ExpectedItem1Color, ActualPartition.Locations[0].Items[0].Color);
            Assert.AreEqual(ExpectedItem1Size, ActualPartition.Locations[0].Items[0].Size);
        }

        [TestMethod]
        public void AddUser_Test7()
        {
            // Arrange
            Controller TestController = new Controller();
            TestController.Location_Comparer = new LocationComparer(19);

            Item TestItem1 = new Item("001", "T-shirt", "White", "Large");
            List<string> TestItem1_Locations = new List<string> { "001A01" };

            Item TestItem2 = new Item("002", "Hoddie", "Blue", "Medium");
            List<string> TestItem2_Locations = new List<string> { "001B02" };

            Item TestItem3 = new Item("003", "Pants", "Green", "Extra-Large");
            List<string> TestItem3_Locations = new List<string> { "002C03" };

            Item TestItem4 = new Item("004", "Shorts", "Red", "Small");
            List<string> TestItem4_Locations = new List<string> { "002D04" };

            Item TestItem5 = new Item("005", "Shoe", "Black", "43");
            List<string> TestItem5_Locations = new List<string> { "002E05" };

            Item TestItem6 = new Item("006", "Jacket", "Brown", "Extra-Small");
            List<string> TestItem6_Locations = new List<string> { "003F06" };

            Item TestItem7 = new Item("007", "Skjorte", "Grey", "XXL");
            List<string> TestItem7_Locations = new List<string> { "003G07" };

            User TestUser1 = new User("1");
            User TestUser2 = new User("2");
            User TestUser3 = new User("3");

            //Expected
            string ExpectedItem1ID = "006";
            string ExpectedItem1Name = "Jacket";
            string ExpectedItem1Color = "Brown";
            string ExpectedItem1Size = "Extra-Small";

            string ExpectedLocation1ID = "003F06";

            int ExpectedPartitionSize = 1;

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

            Partition ActualPartition = TestController.NextPartition(TestUser3); //expected test item 6

            // Assert
            Assert.AreEqual(ExpectedPartitionSize, ActualPartition.Locations.Count);

            Assert.AreEqual(ExpectedLocation1ID, ActualPartition.Locations[0].ID);

            Assert.AreEqual(ExpectedItem1ID, ActualPartition.Locations[0].Items[0].ID);
            Assert.AreEqual(ExpectedItem1Name, ActualPartition.Locations[0].Items[0].Name);
            Assert.AreEqual(ExpectedItem1Color, ActualPartition.Locations[0].Items[0].Color);
            Assert.AreEqual(ExpectedItem1Size, ActualPartition.Locations[0].Items[0].Size);
        }

        [TestMethod]
        public void AddUser_Test8()
        {
            // Arrange
            Controller TestController = new Controller();
            TestController.Location_Comparer = new LocationComparer(19);

            Item TestItem1 = new Item("001", "T-shirt", "White", "Large");
            List<string> TestItem1_Locations = new List<string> { "001A01" };

            Item TestItem2 = new Item("002", "Hoddie", "Blue", "Medium");
            List<string> TestItem2_Locations = new List<string> { "001B02" };

            Item TestItem3 = new Item("003", "Pants", "Green", "Extra-Large");
            List<string> TestItem3_Locations = new List<string> { "002C03" };

            Item TestItem4 = new Item("004", "Shorts", "Red", "Small");
            List<string> TestItem4_Locations = new List<string> { "002D04" };

            Item TestItem5 = new Item("005", "Shoe", "Black", "43");
            List<string> TestItem5_Locations = new List<string> { "002E05" };

            Item TestItem6 = new Item("006", "Jacket", "Brown", "Extra-Small");
            List<string> TestItem6_Locations = new List<string> { "003F06" };

            Item TestItem7 = new Item("007", "Skjorte", "Grey", "XXL");
            List<string> TestItem7_Locations = new List<string> { "003G07" };

            User TestUser1 = new User("1");
            User TestUser2 = new User("2");

            //Expected
            string ExpectedItem1ID = "003";
            string ExpectedItem1Name = "Pants";
            string ExpectedItem1Color = "Green";
            string ExpectedItem1Size = "Extra-Large";

            string ExpectedLocation1ID = "002C03";

            int ExpectedPartitionSize = 1;

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

            Partition ActualPartition = TestController.NextPartition(TestUser2); //expected test item 3

            // Assert
            Assert.AreEqual(ExpectedPartitionSize, ActualPartition.Locations.Count);

            Assert.AreEqual(ExpectedLocation1ID, ActualPartition.Locations[0].ID);

            Assert.AreEqual(ExpectedItem1ID, ActualPartition.Locations[0].Items[0].ID);
            Assert.AreEqual(ExpectedItem1Name, ActualPartition.Locations[0].Items[0].Name);
            Assert.AreEqual(ExpectedItem1Color, ActualPartition.Locations[0].Items[0].Color);
            Assert.AreEqual(ExpectedItem1Size, ActualPartition.Locations[0].Items[0].Size);
        }

        [TestMethod]
        public void AddUser_Test9()
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
            List<string> TestItem6_Locations = new List<string> { "002F06" };

            Item TestItem7 = new Item("007", "Skjorte", "Grey", "XXL");
            List<string> TestItem7_Locations = new List<string> { "002G07" };

            User TestUser1 = new User("1");
            User TestUser2 = new User("2");
            User TestUser3 = new User("3");
            User TestUser4 = new User("4");

            //Expected
            string ExpectedItem1ID = "002";
            string ExpectedItem1Name = "Hoddie";
            string ExpectedItem1Color = "Blue";
            string ExpectedItem1Size = "Medium";

            string ExpectedLocation1ID = "001B02";

            int ExpectedPartitionSize = 1;

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
            TestController.AddUser(TestUser4);

            Partition DumpPartition = TestController.NextPartition(TestUser1);
            DumpPartition = TestController.NextPartition(TestUser2);
            DumpPartition = TestController.NextPartition(TestUser3);

            Partition ActualPartition = TestController.NextPartition(TestUser4); //expected test item 2

            // Assert
            Assert.AreEqual(ExpectedPartitionSize, ActualPartition.Locations.Count);

            Assert.AreEqual(ExpectedLocation1ID, ActualPartition.Locations[0].ID);

            Assert.AreEqual(ExpectedItem1ID, ActualPartition.Locations[0].Items[0].ID);
            Assert.AreEqual(ExpectedItem1Name, ActualPartition.Locations[0].Items[0].Name);
            Assert.AreEqual(ExpectedItem1Color, ActualPartition.Locations[0].Items[0].Color);
            Assert.AreEqual(ExpectedItem1Size, ActualPartition.Locations[0].Items[0].Size);
        }
    }
}
