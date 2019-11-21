using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace Unit_Tests
{
    [TestClass]
    public class Partition_Tests
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
        public void Constroctor_test1()
        {
            // Arrange
            Partition TestPartition;

            //Expected
            PartitionState ExpectedState = PartitionState.NotCounted;
            PartitionRequsitionState ExpectedRequsitionState = PartitionRequsitionState.Requested;
            int ExpectedTotalNrOfItems = 0;
            int ExpectedItemsCounted = 0;
            int ExpectedLocationCount = 0;
            bool ExpectedIsMultiLocationItemPartition = false;

            // Act
            TestPartition = new Partition();

            PartitionState ActualState = TestPartition.State;
            PartitionRequsitionState ActualRequsitionState = TestPartition.RequsitionState;
            int ActualTotalNrOfItems = TestPartition.TotalNrOFItems;
            int ActualItemsCounted = TestPartition.ItemsCounted;
            int ActualLocationCount = TestPartition.Locations.Count;
            bool ActualIsMultiLocationItemPartition = TestPartition.IsMultiLocationItemPartition;

            // Assert
            Assert.AreEqual(ExpectedState, ActualState);
            Assert.AreEqual(ExpectedRequsitionState, ActualRequsitionState);
            Assert.AreEqual(ExpectedTotalNrOfItems, ActualTotalNrOfItems);
            Assert.AreEqual(ExpectedItemsCounted, ActualItemsCounted);
            Assert.AreEqual(ExpectedLocationCount, ActualLocationCount);
            Assert.AreEqual(ExpectedIsMultiLocationItemPartition, ActualIsMultiLocationItemPartition);
        }

        [TestMethod]
        public void Constroctor_test2()
        {
            // Arrange
            Partition TestPartition;

            //Expected
            PartitionState ExpectedState = PartitionState.NotCounted;
            PartitionRequsitionState ExpectedRequsitionState = PartitionRequsitionState.Requested;
            int ExpectedTotalNrOfItems = 0;
            int ExpectedItemsCounted = 0;
            int ExpectedLocationCount = 0;
            bool ExpectedIsMultiLocationItemPartition = true;

            // Act
            TestPartition = new Partition(true);

            PartitionState ActualState = TestPartition.State;
            PartitionRequsitionState ActualRequsitionState = TestPartition.RequsitionState;
            int ActualTotalNrOfItems = TestPartition.TotalNrOFItems;
            int ActualItemsCounted = TestPartition.ItemsCounted;
            int ActualLocationCount = TestPartition.Locations.Count;
            bool ActualIsMultiLocationItemPartition = TestPartition.IsMultiLocationItemPartition;

            // Assert
            Assert.AreEqual(ExpectedState, ActualState);
            Assert.AreEqual(ExpectedRequsitionState, ActualRequsitionState);
            Assert.AreEqual(ExpectedTotalNrOfItems, ActualTotalNrOfItems);
            Assert.AreEqual(ExpectedItemsCounted, ActualItemsCounted);
            Assert.AreEqual(ExpectedLocationCount, ActualLocationCount);
            Assert.AreEqual(ExpectedIsMultiLocationItemPartition, ActualIsMultiLocationItemPartition);
        }

        [TestMethod]
        public void Constroctor_test3()
        {
            // Arrange
            Partition TestPartition;
            Location TestLocation;

            //Expected
            PartitionState ExpectedState = PartitionState.NotCounted;
            PartitionRequsitionState ExpectedRequsitionState = PartitionRequsitionState.Requested;
            int ExpectedTotalNrOfItems = 0;
            int ExpectedItemsCounted = 0;
            int ExpectedLocationCount = 1;
            bool ExpectedIsMultiLocationItemPartition = false;

            // Act
            TestLocation = new Location("001A01");
            TestPartition = new Partition(TestLocation);

            PartitionState ActualState = TestPartition.State;
            PartitionRequsitionState ActualRequsitionState = TestPartition.RequsitionState;
            int ActualTotalNrOfItems = TestPartition.TotalNrOFItems;
            int ActualItemsCounted = TestPartition.ItemsCounted;
            int ActualLocationCount = TestPartition.Locations.Count;
            bool ActualIsMultiLocationItemPartition = TestPartition.IsMultiLocationItemPartition;

            // Assert
            Assert.AreEqual(ExpectedState, ActualState);
            Assert.AreEqual(ExpectedRequsitionState, ActualRequsitionState);
            Assert.AreEqual(ExpectedTotalNrOfItems, ActualTotalNrOfItems);
            Assert.AreEqual(ExpectedItemsCounted, ActualItemsCounted);
            Assert.AreEqual(ExpectedLocationCount, ActualLocationCount);
            Assert.AreEqual(ExpectedIsMultiLocationItemPartition, ActualIsMultiLocationItemPartition);
        }

        [TestMethod]
        public void Constroctor_test4()
        {
            // Arrange
            Partition TestPartition;
            Location TestLocation;
            Item TestItem;

            //Expected
            PartitionState ExpectedState = PartitionState.NotCounted;
            PartitionRequsitionState ExpectedRequsitionState = PartitionRequsitionState.Requested;
            int ExpectedTotalNrOfItems = 1;
            int ExpectedItemsCounted = 0;
            int ExpectedLocationCount = 1;
            bool ExpectedIsMultiLocationItemPartition = true;

            // Act
            TestItem = new Item("01", "T-shirt", "white", "Large", new List<Location> { new Location("001A02"), new Location("001A03") });
            TestLocation = new Location("001A01", new List<Item> { TestItem });
            TestPartition = new Partition(TestLocation);

            PartitionState ActualState = TestPartition.State;
            PartitionRequsitionState ActualRequsitionState = TestPartition.RequsitionState;
            int ActualTotalNrOfItems = TestPartition.TotalNrOFItems;
            int ActualItemsCounted = TestPartition.ItemsCounted;
            int ActualLocationCount = TestPartition.Locations.Count;
            bool ActualIsMultiLocationItemPartition = TestPartition.IsMultiLocationItemPartition;

            // Assert
            Assert.AreEqual(ExpectedState, ActualState);
            Assert.AreEqual(ExpectedRequsitionState, ActualRequsitionState);
            Assert.AreEqual(ExpectedTotalNrOfItems, ActualTotalNrOfItems);
            Assert.AreEqual(ExpectedItemsCounted, ActualItemsCounted);
            Assert.AreEqual(ExpectedLocationCount, ActualLocationCount);
            Assert.AreEqual(ExpectedIsMultiLocationItemPartition, ActualIsMultiLocationItemPartition);
        }

        [TestMethod]
        public void AddLocation_test1()
        {
            // Arrange
            Partition TestPartition;
            Location TestLocation;
            Item TestItem;

            //Expected
            PartitionState ExpectedState = PartitionState.NotCounted;
            PartitionRequsitionState ExpectedRequsitionState = PartitionRequsitionState.Requested;
            int ExpectedTotalNrOfItems = 1;
            int ExpectedItemsCounted = 0;
            int ExpectedLocationCount = 1;
            bool ExpectedIsMultiLocationItemPartition = false;

            // Act
            TestItem = new Item("01", "T-shirt", "white", "Large", new List<Location> { new Location("001A01")});
            TestLocation = new Location("001A01", new List<Item> { TestItem });
            TestPartition = new Partition();

            TestPartition.AddLocation(TestLocation);

            PartitionState ActualState = TestPartition.State;
            PartitionRequsitionState ActualRequsitionState = TestPartition.RequsitionState;
            int ActualTotalNrOfItems = TestPartition.TotalNrOFItems;
            int ActualItemsCounted = TestPartition.ItemsCounted;
            int ActualLocationCount = TestPartition.Locations.Count;
            bool ActualIsMultiLocationItemPartition = TestPartition.IsMultiLocationItemPartition;

            // Assert
            Assert.AreEqual(ExpectedState, ActualState);
            Assert.AreEqual(ExpectedRequsitionState, ActualRequsitionState);
            Assert.AreEqual(ExpectedTotalNrOfItems, ActualTotalNrOfItems);
            Assert.AreEqual(ExpectedItemsCounted, ActualItemsCounted);
            Assert.AreEqual(ExpectedLocationCount, ActualLocationCount);
            Assert.AreEqual(ExpectedIsMultiLocationItemPartition, ActualIsMultiLocationItemPartition);
        }

        [TestMethod]
        public void AddLocation_test2()
        {
            // Arrange
            Partition TestPartition;
            Location TestLocation;
            Item TestItem;

            //Expected
            PartitionState ExpectedState = PartitionState.NotCounted;
            PartitionRequsitionState ExpectedRequsitionState = PartitionRequsitionState.Requested;
            int ExpectedTotalNrOfItems = 1;
            int ExpectedItemsCounted = 0;
            int ExpectedLocationCount = 1;
            bool ExpectedIsMultiLocationItemPartition = true;

            // Act
            TestItem = new Item("01", "T-shirt", "white", "Large", new List<Location> { new Location("001A02"), new Location("001A03") });
            TestLocation = new Location("001A01", new List<Item> { TestItem });
            TestPartition = new Partition(true);

            TestPartition.AddLocation(TestLocation);

            PartitionState ActualState = TestPartition.State;
            PartitionRequsitionState ActualRequsitionState = TestPartition.RequsitionState;
            int ActualTotalNrOfItems = TestPartition.TotalNrOFItems;
            int ActualItemsCounted = TestPartition.ItemsCounted;
            int ActualLocationCount = TestPartition.Locations.Count;
            bool ActualIsMultiLocationItemPartition = TestPartition.IsMultiLocationItemPartition;

            // Assert
            Assert.AreEqual(ExpectedState, ActualState);
            Assert.AreEqual(ExpectedRequsitionState, ActualRequsitionState);
            Assert.AreEqual(ExpectedTotalNrOfItems, ActualTotalNrOfItems);
            Assert.AreEqual(ExpectedItemsCounted, ActualItemsCounted);
            Assert.AreEqual(ExpectedLocationCount, ActualLocationCount);
            Assert.AreEqual(ExpectedIsMultiLocationItemPartition, ActualIsMultiLocationItemPartition);
        }
    }
}
