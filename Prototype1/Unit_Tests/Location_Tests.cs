using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace Unit_Tests
{
    [TestClass]
    public class Location_Tests
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
        public void Constructor_test1()
        {
            // Arrange
            Location TestLocation = new Location("001A01");

            //Expected
            int ExpectedShelf = 1;
            char ExpectedRow = 'A';
            int ExpectedPosistion = 1;

            // Act
            int ActualShelf = TestLocation.Shelf;
            char ActualRow = TestLocation.Row;
            int ActualPosistion = TestLocation.Posistion;

            // Assert
            Assert.AreEqual(ExpectedShelf, ActualShelf);
            Assert.AreEqual(ExpectedRow, ActualRow);
            Assert.AreEqual(ExpectedPosistion, ActualPosistion);
        }

        [TestMethod]
        public void Constructor_test2()
        {
            // Arrange
            List<Item> Items = new List<Item> { new Item("001", "T-shirt", "White", "Large"), new Item("002", "Pants", "blue", "Medium") };

            Location TestLocation = new Location("001A01", Items);

            //Expected
            int ExpectedShelf = 1;
            char ExpectedRow = 'A';
            int ExpectedPosistion = 1;
            bool ExpectedHasMultiLocationItems = false;

            // Act
            int ActualShelf = TestLocation.Shelf;
            char ActualRow = TestLocation.Row;
            int ActualPosistion = TestLocation.Posistion;
            bool ActualHasMultiLocationItems = TestLocation.HasMultilocationItem;

            // Assert
            Assert.AreEqual(ExpectedShelf, ActualShelf);
            Assert.AreEqual(ExpectedRow, ActualRow);
            Assert.AreEqual(ExpectedPosistion, ActualPosistion);
            Assert.AreEqual(ExpectedHasMultiLocationItems, ActualHasMultiLocationItems);
        }

        [TestMethod]
        public void Constructor_test3()
        {
            // Arrange
            List<Item> Items = new List<Item> { new Item("001", "T-shirt", "White", "Large", new List<Location> { new Location("001A01"), new Location("001A02")}), new Item("002", "Pants", "blue", "Medium") };

            Location TestLocation = new Location("001A01", Items);

            //Expected
            int ExpectedShelf = 1;
            char ExpectedRow = 'A';
            int ExpectedPosistion = 1;
            bool ExpectedHasMultiLocationItems = true;

            // Act
            int ActualShelf = TestLocation.Shelf;
            char ActualRow = TestLocation.Row;
            int ActualPosistion = TestLocation.Posistion;
            bool ActualHasMultiLocationItems = TestLocation.HasMultilocationItem;

            // Assert
            Assert.AreEqual(ExpectedShelf, ActualShelf);
            Assert.AreEqual(ExpectedRow, ActualRow);
            Assert.AreEqual(ExpectedPosistion, ActualPosistion);
            Assert.AreEqual(ExpectedHasMultiLocationItems, ActualHasMultiLocationItems);
        }

        [TestMethod]
        public void Constructor_test4()
        {
            // Arrange
            List<Item> Items = new List<Item> { new Item("001", "T-shirt", "White", "Large", new List<Location> { new Location("001A01"), new Location("001A01") }),
                                                new Item("002", "Pants", "blue", "Medium") };

            Location TestLocation = new Location("001A01", Items);

            //Expected
            int ExpectedShelf = 1;
            char ExpectedRow = 'A';
            int ExpectedPosistion = 1;
            bool ExpectedHasMultiLocationItems = false;
            int ExpectedItemCount = 2;

            // Act
            int ActualShelf = TestLocation.Shelf;
            char ActualRow = TestLocation.Row;
            int ActualPosistion = TestLocation.Posistion;
            bool ActualHasMultiLocationItems = TestLocation.HasMultilocationItem;
            int ActualItemCount = TestLocation.Items.Count;

            // Assert
            Assert.AreEqual(ExpectedShelf, ActualShelf);
            Assert.AreEqual(ExpectedRow, ActualRow);
            Assert.AreEqual(ExpectedPosistion, ActualPosistion);
            Assert.AreEqual(ExpectedHasMultiLocationItems, ActualHasMultiLocationItems);
            Assert.AreEqual(ExpectedItemCount, ActualItemCount);
        }

        [TestMethod]
        public void Constructor_test5()
        {
            // Arrange
            List<Item> Items = new List<Item> { new Item("001", "T-shirt", "White", "Large"), new Item("001", "T-shirt", "White", "Large") };

            Location TestLocation = new Location("001A01", Items);

            //Expected
            int ExpectedShelf = 1;
            char ExpectedRow = 'A';
            int ExpectedPosistion = 1;
            bool ExpectedHasMultiLocationItems = false;
            int ExpectedItemCount = 1;

            // Act
            int ActualShelf = TestLocation.Shelf;
            char ActualRow = TestLocation.Row;
            int ActualPosistion = TestLocation.Posistion;
            bool ActualHasMultiLocationItems = TestLocation.HasMultilocationItem;
            int ActualItemCount = TestLocation.Items.Count;

            // Assert
            Assert.AreEqual(ExpectedShelf, ActualShelf);
            Assert.AreEqual(ExpectedRow, ActualRow);
            Assert.AreEqual(ExpectedPosistion, ActualPosistion);
            Assert.AreEqual(ExpectedHasMultiLocationItems, ActualHasMultiLocationItems);
            Assert.AreEqual(ExpectedItemCount, ActualItemCount);
        }

        [TestMethod]
        public void HasItemTest1()
        {
            // Arrange
            Item TestItem1 = new Item("001", "T-shirt", "White", "Large");
            Item TestItem2 = new Item("002", "Pants", "blue", "Small");

            List<Item> Items = new List<Item> { TestItem1, TestItem2 };

            Location TestLocation1 = new Location("001A01", Items);

            //Expected
            bool Expected = true;

            // Act
            bool Actual = TestLocation1.HasItem(TestItem2);

            // Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestMethod]
        public void HasItemTest2()
        {
            // Arrange
            Item TestItem1 = new Item("001", "T-shirt", "White", "Large");
            Item TestItem2 = new Item("002", "Pants", "blue", "Small");

            List<Item> Items = new List<Item> { TestItem1 };

            Location TestLocation1 = new Location("001A01", Items);

            //Expected
            bool Expected = false;

            // Act
            bool Actual = TestLocation1.HasItem(TestItem2);

            // Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestMethod]
        public void AddItemTest1()
        {
            // Arrange
            Item TestItem = new Item("001", "T-shirt", "White", "Large");

            Location TestLocation = new Location("001A01");

            //Expected
            Item Expected = TestItem;
            bool ExpectedHasMultilocationItem = false;

            // Act
            TestLocation.AddItem(TestItem);

            // Assert
            Assert.AreEqual(Expected, TestLocation.Items[0]);
            Assert.AreEqual(ExpectedHasMultilocationItem, TestLocation.HasMultilocationItem);
        }

        [TestMethod]
        public void AddItemTest2()
        {
            // Arrange
            Item TestItem1 = new Item("001", "T-shirt", "White", "Large");
            Item TestItem2 = new Item("002", "Pants", "blue", "Small");

            Location TestLocation = new Location("001A01");

            //Expected
            Item ExpectedItem1 = TestItem1;
            Item ExpectedItem2 = TestItem2;
            bool ExpectedHasMultilocationItem = false;

            // Act
            TestLocation.AddItem(TestItem1);
            TestLocation.AddItem(TestItem2);

            // Assert
            Assert.AreEqual(ExpectedItem1, TestLocation.Items[0]);
            Assert.AreEqual(ExpectedItem2, TestLocation.Items[1]);
            Assert.AreEqual(ExpectedHasMultilocationItem, TestLocation.HasMultilocationItem);
        }

        [TestMethod]
        public void AddItemTest3()
        {
            // Arrange
            Item TestItem1 = new Item("001", "T-shirt", "White", "Large");
            Item TestItem2 = new Item("002", "Pants", "blue", "Small", new List<Location> { new Location("001A01"), new Location("001A02") });

            Location TestLocation = new Location("001A01");

            //Expected
            Item ExpectedItem1 = TestItem1;
            Item ExpectedItem2 = TestItem2;
            bool ExpectedHasMultilocationItem = true;

            // Act
            TestLocation.AddItem(TestItem1);
            TestLocation.AddItem(TestItem2);

            // Assert
            Assert.AreEqual(ExpectedItem1, TestLocation.Items[0]);
            Assert.AreEqual(ExpectedItem2, TestLocation.Items[1]);
            Assert.AreEqual(ExpectedHasMultilocationItem, TestLocation.HasMultilocationItem);
        }

        [TestMethod]
        public void CompareDistance_Test1()
        {
            // Arrange
            int ActualDistance1;
            int ActualDistance2;

            LocationComparer TestLocationComparer = new LocationComparer(19);
            
            Location TestLocation1 = new Location("001A01");
            Location TestLocation2 = new Location("001A05");

            //Expected
            int ExpectedDistance = 4;

            // Act
            ActualDistance1 = TestLocation1.CompareDistance(TestLocation2, TestLocationComparer);
            ActualDistance2 = TestLocation2.CompareDistance(TestLocation1, TestLocationComparer);

            // Assert
            Assert.AreEqual(ExpectedDistance, ActualDistance1);
            Assert.AreEqual(ExpectedDistance, ActualDistance2);
        }

        [TestMethod]
        public void CompareDistance_Test2()
        {
            // Arrange
            int ActualDistance1;
            int ActualDistance2;

            LocationComparer TestLocationComparer = new LocationComparer(19);

            Location TestLocation1 = new Location("001A01");
            Location TestLocation2 = new Location("002A05");

            //Expected
            int ExpectedDistance = 1000;

            // Act
            ActualDistance1 = TestLocation1.CompareDistance(TestLocation2, TestLocationComparer);
            ActualDistance2 = TestLocation2.CompareDistance(TestLocation1, TestLocationComparer);

            // Assert
            Assert.AreEqual(ExpectedDistance, ActualDistance1);
            Assert.AreEqual(ExpectedDistance, ActualDistance2);
        }

        [TestMethod]
        public void CompareDistance_Test3()
        {
            // Arrange
            int ActualDistance1;
            int ActualDistance2;

            LocationComparer TestLocationComparer = new LocationComparer(19);

            Location TestLocation1 = new Location("001A01");
            Location TestLocation2 = new Location("002A05");

            //Expected
            int ExpectedDistance = 3000;

            // Act
            TestLocationComparer.IncreasePriority(2);
            TestLocationComparer.IncreasePriority(2);

            ActualDistance1 = TestLocation1.CompareDistance(TestLocation2, TestLocationComparer);
            ActualDistance2 = TestLocation2.CompareDistance(TestLocation1, TestLocationComparer);

            // Assert
            Assert.AreEqual(ExpectedDistance, ActualDistance1);
            Assert.AreEqual(ExpectedDistance, ActualDistance2);
        }
    }
}
