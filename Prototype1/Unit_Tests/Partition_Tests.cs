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

        [TestMethod]
        public void Sort_Test1()
        {
            // Arrange
            Partition TestPartition1 = new Partition(new Location("001A01"));
            Partition TestPartition2 = new Partition(new Location("002A01"));
            Partition TestPartition3 = new Partition(new Location("003A01"));

            //Expected
            int ExpectedShelf1 = 1;
            int ExpectedPosistion1 = 1;

            int ExpectedShelf2 = 2;
            int ExpectedPosistion2 = 1;

            int ExpectedShelf3 = 3;
            int ExpectedPosistion3 = 1;

            // Act
            List<Partition> ActualPartitionList = new List<Partition>();

            ActualPartitionList.Add(TestPartition1);
            ActualPartitionList.Add(TestPartition2);
            ActualPartitionList.Add(TestPartition3);

            ActualPartitionList.Sort();

            // Assert
            Assert.AreEqual(ExpectedShelf1, ActualPartitionList[0].Span.Shelf);
            Assert.AreEqual(ExpectedPosistion1, ActualPartitionList[0].Span.Position);

            Assert.AreEqual(ExpectedShelf2, ActualPartitionList[1].Span.Shelf);
            Assert.AreEqual(ExpectedPosistion2, ActualPartitionList[1].Span.Position);

            Assert.AreEqual(ExpectedShelf3, ActualPartitionList[2].Span.Shelf);
            Assert.AreEqual(ExpectedPosistion3, ActualPartitionList[2].Span.Position);
        }

        [TestMethod]
        public void Sort_Test2()
        {
            // Arrange
            Partition TestPartition1 = new Partition(new Location("001A01"));
            Partition TestPartition2 = new Partition(new Location("001A02"));
            Partition TestPartition3 = new Partition(new Location("001A03"));

            //Expected
            int ExpectedShelf1 = 1;
            int ExpectedPosistion1 = 1;

            int ExpectedShelf2 = 1;
            int ExpectedPosistion2 = 2;

            int ExpectedShelf3 = 1;
            int ExpectedPosistion3 = 3;

            // Act
            List<Partition> ActualPartitionList = new List<Partition>();

            ActualPartitionList.Add(TestPartition1);
            ActualPartitionList.Add(TestPartition2);
            ActualPartitionList.Add(TestPartition3);

            ActualPartitionList.Sort();

            // Assert
            Assert.AreEqual(ExpectedShelf1, ActualPartitionList[0].Span.Shelf);
            Assert.AreEqual(ExpectedPosistion1, ActualPartitionList[0].Span.Position);

            Assert.AreEqual(ExpectedShelf2, ActualPartitionList[1].Span.Shelf);
            Assert.AreEqual(ExpectedPosistion2, ActualPartitionList[1].Span.Position);

            Assert.AreEqual(ExpectedShelf3, ActualPartitionList[2].Span.Shelf);
            Assert.AreEqual(ExpectedPosistion3, ActualPartitionList[2].Span.Position);
        }

        [TestMethod]
        public void Sort_Test3()
        {
            // Arrange
            Partition TestPartition1 = new Partition(new Location("001A01"));
            Partition TestPartition2 = new Partition(new Location("001A02"));
            Partition TestPartition3 = new Partition(new Location("002A01"));

            //Expected
            int ExpectedShelf1 = 1;
            int ExpectedPosistion1 = 1;

            int ExpectedShelf2 = 1;
            int ExpectedPosistion2 = 2;

            int ExpectedShelf3 = 2;
            int ExpectedPosistion3 = 1;

            // Act
            List<Partition> ActualPartitionList = new List<Partition>();

            ActualPartitionList.Add(TestPartition1);
            ActualPartitionList.Add(TestPartition2);
            ActualPartitionList.Add(TestPartition3);

            ActualPartitionList.Sort();

            // Assert
            Assert.AreEqual(ExpectedShelf1, ActualPartitionList[0].Span.Shelf);
            Assert.AreEqual(ExpectedPosistion1, ActualPartitionList[0].Span.Position);

            Assert.AreEqual(ExpectedShelf2, ActualPartitionList[1].Span.Shelf);
            Assert.AreEqual(ExpectedPosistion2, ActualPartitionList[1].Span.Position);

            Assert.AreEqual(ExpectedShelf3, ActualPartitionList[2].Span.Shelf);
            Assert.AreEqual(ExpectedPosistion3, ActualPartitionList[2].Span.Position);
        }
    }
}
