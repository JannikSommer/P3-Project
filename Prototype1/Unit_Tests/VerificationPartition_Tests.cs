using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace Unit_Tests
{
    [TestClass]
    public class VerificationPartition_Tests
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
        public void VerificationPartition_Test1_AddItem()
        {
            // Arrange
            VerificationPartition TestVerificationPartition = new VerificationPartition();

            Item TestItem1 = new Item("001", "T-Shirt", "White", "Large");
            Item TestItem2 = new Item("002", "Pants", "Black", "Medium");

            Location TestLocation1 = new Location("001A01");
            Location TestLocation2 = new Location("002A01");
            Location TestLocation3 = new Location("003A01");

            TestItem1.AddLocation(TestLocation1);
            TestItem1.AddLocation(TestLocation2);
            TestItem2.AddLocation(TestLocation2);
            TestItem2.AddLocation(TestLocation3);

            //Expected
            int ExpectedItemCount = 1;
            int ExpectedLocationCount = 2;

            string ExpectedLocation1 = "001A01";
            string ExpectedLocation2 = "002A01";

            string ExpectedItem = "001";

            // Act
            TestVerificationPartition.AddItem(TestItem1);

            // Assert
            Assert.AreNotEqual(TestLocation2, TestVerificationPartition.Locations[1]);

            Assert.AreEqual(ExpectedItemCount, TestVerificationPartition.Items.Count);
            Assert.AreEqual(ExpectedLocationCount, TestVerificationPartition.Locations.Count);
            Assert.AreEqual(ExpectedLocation1, TestVerificationPartition.Locations[0].ID);
            Assert.AreEqual(ExpectedLocation2, TestVerificationPartition.Locations[1].ID);
            Assert.AreEqual(ExpectedItem, TestVerificationPartition.Items[0].ID);
        }

        [TestMethod]
        public void VerificationPartition_Test2_AddItem()
        {
            // Arrange
            VerificationPartition TestVerificationPartition = new VerificationPartition();

            Item TestItem1 = new Item("001", "T-Shirt", "White", "Large");
            Item TestItem2 = new Item("002", "Pants", "Black", "Medium");

            Location TestLocation1 = new Location("001A01");
            Location TestLocation2 = new Location("002A01");
            Location TestLocation3 = new Location("003A01");

            TestItem1.AddLocation(TestLocation1);
            TestItem1.AddLocation(TestLocation2);
            TestItem2.AddLocation(TestLocation2);
            TestItem2.AddLocation(TestLocation3);

            //Expected
            int ExpectedItemCount = 2;
            int ExpectedLocationCount = 3;

            string ExpectedLocation1 = "001A01";
            string ExpectedLocation2 = "002A01";

            string ExpectedItem = "001";

            // Act
            TestVerificationPartition.AddItem(TestItem1);
            TestVerificationPartition.AddItem(TestItem2);

            // Assert
            Assert.AreNotEqual(TestLocation2, TestVerificationPartition.Locations[1]);

            Assert.AreEqual(ExpectedItemCount, TestVerificationPartition.Items.Count);
            Assert.AreEqual(ExpectedLocationCount, TestVerificationPartition.Locations.Count);
            Assert.AreEqual(ExpectedLocation1, TestVerificationPartition.Locations[0].ID);
            Assert.AreEqual(ExpectedLocation2, TestVerificationPartition.Locations[1].ID);
            Assert.AreEqual(ExpectedItem, TestVerificationPartition.Items[0].ID);
        }

        [TestMethod]
        public void VerificationPartition_Test3_CompareDistance()
        {
            // Arrange
            VerificationPartition TestVerificationPartition = new VerificationPartition();
            LocationComparer TestLocationComparer = new LocationComparer(3);

            Item TestItem1 = new Item("001", "T-Shirt", "White", "Large");
            Item TestItem2 = new Item("002", "Pants", "Black", "Medium");

            Location TestLocation1 = new Location("001A01");
            Location TestLocation2 = new Location("002A01");
            Location TestLocation3 = new Location("003A01");

            TestItem1.AddLocation(TestLocation1);
            TestItem1.AddLocation(TestLocation2);
            TestItem2.AddLocation(TestLocation2);
            TestItem2.AddLocation(TestLocation3);

            TestVerificationPartition.AddItem(TestItem1);

            //Expected
            int ExpectedDistance = 1000;

            // Act
            int ActualDistance = TestVerificationPartition.CompareDistance(TestItem2, TestLocationComparer);

            // Assert
            Assert.AreEqual(ExpectedDistance, ActualDistance);
        }

        [TestMethod]
        public void VerificationPartition_Test4_CompareDistance()
        {
            // Arrange
            VerificationPartition TestVerificationPartition = new VerificationPartition();
            LocationComparer TestLocationComparer = new LocationComparer(3);

            Item TestItem1 = new Item("001", "T-Shirt", "White", "Large");
            Item TestItem2 = new Item("002", "Pants", "Black", "Medium");

            Location TestLocation1 = new Location("001A01");
            Location TestLocation2 = new Location("002A01");
            Location TestLocation3 = new Location("002A01");

            TestItem1.AddLocation(TestLocation1);
            TestItem1.AddLocation(TestLocation2);
            TestItem2.AddLocation(TestLocation2);
            TestItem2.AddLocation(TestLocation3);

            TestVerificationPartition.AddItem(TestItem1);

            //Expected
            int ExpectedDistance = 0;

            // Act
            int ActualDistance = TestVerificationPartition.CompareDistance(TestItem2, TestLocationComparer);

            // Assert
            Assert.AreEqual(ExpectedDistance, ActualDistance);
        }

        [TestMethod]
        public void VerificationPartition_Test5_CompareDistance()
        {
            // Arrange
            VerificationPartition TestVerificationPartition = new VerificationPartition();
            LocationComparer TestLocationComparer = new LocationComparer(3);

            Item TestItem1 = new Item("001", "T-Shirt", "White", "Large");
            Item TestItem2 = new Item("002", "Pants", "Black", "Medium");

            Location TestLocation1 = new Location("001A01");
            Location TestLocation2 = new Location("002A01");
            Location TestLocation3 = new Location("002A11");

            TestItem1.AddLocation(TestLocation1);
            TestItem1.AddLocation(TestLocation2);
            TestItem2.AddLocation(TestLocation2);
            TestItem2.AddLocation(TestLocation3);

            TestVerificationPartition.AddItem(TestItem1);

            //Expected
            int ExpectedDistance = 10;

            // Act
            int ActualDistance = TestVerificationPartition.CompareDistance(TestItem2, TestLocationComparer);

            // Assert
            Assert.AreEqual(ExpectedDistance, ActualDistance);
        }

        [TestMethod]
        public void VerificationPartition_Test6_CompareDistance()
        {
            // Arrange
            VerificationPartition TestVerificationPartition = new VerificationPartition();
            LocationComparer TestLocationComparer = new LocationComparer(3);

            Item TestItem1 = new Item("001", "T-Shirt", "White", "Large");
            Item TestItem2 = new Item("002", "Pants", "Black", "Medium");

            Location TestLocation1 = new Location("001A01");
            Location TestLocation2 = new Location("002A01");
            Location TestLocation3 = new Location("002A11");
            Location TestLocation4 = new Location("003A11");

            TestItem1.AddLocation(TestLocation1);
            TestItem1.AddLocation(TestLocation2);
            TestItem2.AddLocation(TestLocation3);
            TestItem2.AddLocation(TestLocation4);

            TestVerificationPartition.AddItem(TestItem1);

            //Expected
            int ExpectedDistance = 1010;

            // Act
            int ActualDistance = TestVerificationPartition.CompareDistance(TestItem2, TestLocationComparer);

            // Assert
            Assert.AreEqual(ExpectedDistance, ActualDistance);
        }
    }
}
