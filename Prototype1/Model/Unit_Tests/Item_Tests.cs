using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace Unit_Tests
{
    [TestClass]
    public class Item_Tests
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
        public void Constructor_Test1()
        {
            // Arrange
            List<Location> TestLocations = new List<Location> { new Location("001A01") , new Location("002A01") };

            Item TestItem = new Item("001", "T-shirt", "White", "Large", TestLocations);

            //Expected
            string Expected_ID = "001";
            string Expected_Name = "T-shirt";
            string Expected_Color = "White";
            string Expected_Size = "Large";
            int Expected_Count = 2;
            bool Expected_HasMultiLocation = true;

            // Act
            string Actual_ID = TestItem.ID;
            string Actual_Name = TestItem.Name;
            string Actual_Color = TestItem.Color;
            string Actual_Size = TestItem.Size;
            int Actual_Count = TestItem.Locations.Count;
            bool Actual_HasMultiLocation = TestItem.HasMultiLocation;

            // Assert
            Assert.AreEqual(Expected_ID, Actual_ID);
            Assert.AreEqual(Expected_Name, Actual_Name);
            Assert.AreEqual(Expected_Color, Actual_Color);
            Assert.AreEqual(Expected_Size, Actual_Size);
            Assert.AreEqual(Expected_Count, Actual_Count);
            Assert.AreEqual(Expected_HasMultiLocation, Actual_HasMultiLocation);
        }

        [TestMethod]
        public void Constructor_Test2()
        {
            // Arrange
            Item TestItem = new Item("001", "T-shirt", "White", "Large");

            //Expected
            string Expected_ID = "001";
            string Expected_Name = "T-shirt";
            string Expected_Color = "White";
            string Expected_Size = "Large";
            int Expected_Count = 0;
            bool Expected_HasMultiLocation = false;

            // Act
            string Actual_ID = TestItem.ID;
            string Actual_Name = TestItem.Name;
            string Actual_Color = TestItem.Color;
            string Actual_Size = TestItem.Size;
            int Actual_Count = TestItem.Locations.Count;
            bool Actual_HasMultiLocation = TestItem.HasMultiLocation;

            // Assert
            Assert.AreEqual(Expected_ID, Actual_ID);
            Assert.AreEqual(Expected_Name, Actual_Name);
            Assert.AreEqual(Expected_Color, Actual_Color);
            Assert.AreEqual(Expected_Size, Actual_Size);
            Assert.AreEqual(Expected_Count, Actual_Count);
            Assert.AreEqual(Expected_HasMultiLocation, Actual_HasMultiLocation);
        }

        [TestMethod]
        public void Constructor_Test3()
        {
            // Arrange
            List<Location> TestLocations = new List<Location> { new Location("001A01"), new Location("001A01") };

            Item TestItem = new Item("001", "T-shirt", "White", "Large", TestLocations);

            //Expected
            string Expected_ID = "001";
            string Expected_Name = "T-shirt";
            string Expected_Color = "White";
            string Expected_Size = "Large";
            int Expected_Count = 1;
            bool Expected_HasMultiLocation = false;

            // Act
            string Actual_ID = TestItem.ID;
            string Actual_Name = TestItem.Name;
            string Actual_Color = TestItem.Color;
            string Actual_Size = TestItem.Size;
            int Actual_Count = TestItem.Locations.Count;
            bool Actual_HasMultiLocation = TestItem.HasMultiLocation;

            // Assert
            Assert.AreEqual(Expected_ID, Actual_ID);
            Assert.AreEqual(Expected_Name, Actual_Name);
            Assert.AreEqual(Expected_Color, Actual_Color);
            Assert.AreEqual(Expected_Size, Actual_Size);
            Assert.AreEqual(Expected_Count, Actual_Count);
            Assert.AreEqual(Expected_HasMultiLocation, Actual_HasMultiLocation);
        }

        [TestMethod]
        public void HasLocationTest()
        {
            // Arrange
            Item TestItem = new Item("001", "T-shirt", "White", "Large");
            Location TestLocation1 = new Location("001A01");
            Location TestLocation2 = new Location("002A01");
            Location TestLocation3 = new Location("003A01");

            TestItem.AddLocation(TestLocation1);
            TestItem.AddLocation(TestLocation2);
            TestItem.AddLocation(TestLocation3);

            //Expected
            bool Expected = true;

            // Act
            bool Actual = TestItem.HasLocation(TestLocation2);

            // Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestMethod]
        public void HasMultiLocationTest1()
        {
            // Arrange
            Item TestItem = new Item("001", "T-shirt", "White", "Large");
            Location TestLocation1 = new Location("001A01");
            Location TestLocation2 = new Location("002A01");
            Location TestLocation3 = new Location("003A01");

            TestItem.AddLocation(TestLocation1);
            TestItem.AddLocation(TestLocation2);
            TestItem.AddLocation(TestLocation3);

            //Expected
            bool Expected = true;

            // Act
            bool Actual = TestItem.HasMultiLocation;

            // Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestMethod]
        public void HasMultiLocationTest2()
        {
            // Arrange
            Item TestItem = new Item("001", "T-shirt", "White", "Large");
            Location TestLocation1 = new Location("001A01");

            TestItem.AddLocation(TestLocation1);

            //Expected
            bool Expected = false;

            // Act
            bool Actual = TestItem.HasMultiLocation;

            // Assert
            Assert.AreEqual(Expected, Actual);
        }

        [TestMethod]
        public void AddItemDoesntAddTwiceTest()
        {
            // Arrange
            Item TestItem = new Item("001", "T-shirt", "White", "Large");
            Location TestLocation1 = new Location("001A01");

            TestItem.AddLocation(TestLocation1);
            TestItem.AddLocation(TestLocation1);

            //Expected
            int ExpectedCount = 1;
            bool ExpectedHasMultipleLocations = false;

            // Act
            int ActualCount = TestItem.Locations.Count;
            bool ActualHasMultiLocations = TestItem.HasMultiLocation;

            // Assert
            Assert.AreEqual(ExpectedCount, ActualCount);
            Assert.AreEqual(ExpectedHasMultipleLocations, ActualHasMultiLocations);
        }

        [TestMethod]
        public void SetsMultiLocationInLocations()
        {
            // Arrange
            Item TestItem = new Item("001", "T-shirt", "White", "Large");
            Location TestLocation1 = new Location("001A01");
            Location TestLocation2 = new Location("002A01");
            Location TestLocation3 = new Location("003A01");

            TestItem.AddLocation(TestLocation1);
            TestItem.AddLocation(TestLocation1);
            TestItem.AddLocation(TestLocation2);
            TestItem.AddLocation(TestLocation3);

            //Expected
            int ExpectedCount = 3;
            bool ExpectedHasMultipleLocations = true;
            bool ExpectedLocation1_HasMultipleLocationItems = true;
            bool ExpectedLocation2_HasMultipleLocationItems = true;
            bool ExpectedLocation3_HasMultipleLocationItems = true;

            // Act
            int ActualCount = TestItem.Locations.Count;
            bool ActualHasMultiLocations = TestItem.HasMultiLocation;
            bool ActualLocation1_HasMultipleLocationItems = TestLocation1.HasMultilocationItem;
            bool ActualLocation2_HasMultipleLocationItems = TestLocation2.HasMultilocationItem;
            bool ActualLocation3_HasMultipleLocationItems = TestLocation3.HasMultilocationItem;

            // Assert
            Assert.AreEqual(ExpectedCount, ActualCount);
            Assert.AreEqual(ExpectedHasMultipleLocations, ActualHasMultiLocations);
            Assert.AreEqual(ExpectedLocation1_HasMultipleLocationItems, ActualLocation1_HasMultipleLocationItems);
            Assert.AreEqual(ExpectedLocation2_HasMultipleLocationItems, ActualLocation2_HasMultipleLocationItems);
            Assert.AreEqual(ExpectedLocation3_HasMultipleLocationItems, ActualLocation3_HasMultipleLocationItems);
        }
    }
}
