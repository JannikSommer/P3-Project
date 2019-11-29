using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace Unit_Tests
{
    [TestClass]
    public class LocationComparer_Tests
    {
        [TestMethod]
        public void LocationComparer_test1_SortOrder()
        {
            // Arrange
            LocationComparer TestLocationComparer = new LocationComparer(2);

            Location TestLocation1 = new Location("000A01");
            Location TestLocation4 = new Location("002A01");
            Location TestLocation5 = new Location("002A02");
            Location TestLocation2 = new Location("001A03");
            Location TestLocation3 = new Location("001A04");
            Location TestLocation6 = new Location("002B01");
            Location TestLocation7 = new Location("002B02");

            //Expected
            string ExpectedLoc1 = "000A01";
            string ExpectedLoc2 = "001A03";
            string ExpectedLoc3 = "001A04";
            string ExpectedLoc4 = "002A01";
            string ExpectedLoc5 = "002B01";
            string ExpectedLoc6 = "002A02";
            string ExpectedLoc7 = "002B02";

            // Act
            List<Location> ActualSortedLocations = new List<Location> { TestLocation1, TestLocation2, TestLocation3, TestLocation4, TestLocation5, TestLocation6, TestLocation7 };
            ActualSortedLocations.Sort(TestLocationComparer);

            // Assert
            Assert.AreEqual(ExpectedLoc1, ActualSortedLocations[0].ID);
            Assert.AreEqual(ExpectedLoc2, ActualSortedLocations[1].ID);
            Assert.AreEqual(ExpectedLoc3, ActualSortedLocations[2].ID);
            Assert.AreEqual(ExpectedLoc4, ActualSortedLocations[3].ID);
            Assert.AreEqual(ExpectedLoc5, ActualSortedLocations[4].ID);
            Assert.AreEqual(ExpectedLoc6, ActualSortedLocations[5].ID);
            Assert.AreEqual(ExpectedLoc7, ActualSortedLocations[6].ID);
        }

        [TestMethod]
        public void LocationComparer_test2_ChangePriority()
        {
            // Arrange
            LocationComparer TestLocationComparer = new LocationComparer(2);

            Location TestLocation1 = new Location("000A01");
            Location TestLocation4 = new Location("002A01");
            Location TestLocation5 = new Location("002A02");
            Location TestLocation2 = new Location("001A03");
            Location TestLocation3 = new Location("001A04");
            Location TestLocation6 = new Location("002B01");
            Location TestLocation7 = new Location("002B02");

            //Expected
            string ExpectedLoc1 = "002A01";
            string ExpectedLoc2 = "002B01";
            string ExpectedLoc3 = "002A02";
            string ExpectedLoc4 = "002B02";
            string ExpectedLoc5 = "000A01";
            string ExpectedLoc6 = "001A03";
            string ExpectedLoc7 = "001A04";

            // Act
            List<Location> ActualSortedLocations = new List<Location> { TestLocation1, TestLocation2, TestLocation3, TestLocation4, TestLocation5, TestLocation6, TestLocation7 };

            TestLocationComparer.DecreasePriority(2);
            TestLocationComparer.DecreasePriority(2);

            ActualSortedLocations.Sort(TestLocationComparer);

            // Assert
            Assert.AreEqual(ExpectedLoc1, ActualSortedLocations[0].ID);
            Assert.AreEqual(ExpectedLoc2, ActualSortedLocations[1].ID);
            Assert.AreEqual(ExpectedLoc3, ActualSortedLocations[2].ID);
            Assert.AreEqual(ExpectedLoc4, ActualSortedLocations[3].ID);
            Assert.AreEqual(ExpectedLoc5, ActualSortedLocations[4].ID);
            Assert.AreEqual(ExpectedLoc6, ActualSortedLocations[5].ID);
            Assert.AreEqual(ExpectedLoc7, ActualSortedLocations[6].ID);
        }

        [TestMethod]
        public void LocationComparer_test3_ChangePriority()
        {
            // Arrange
            LocationComparer TestLocationComparer = new LocationComparer(2);

            Location TestLocation1 = new Location("000A01");
            Location TestLocation4 = new Location("002A01");
            Location TestLocation5 = new Location("002A02");
            Location TestLocation2 = new Location("001A03");
            Location TestLocation3 = new Location("001A04");
            Location TestLocation6 = new Location("002B01");
            Location TestLocation7 = new Location("002B02");

            //Expected
            string ExpectedLoc1 = "000A01";
            string ExpectedLoc2 = "002A01";
            string ExpectedLoc3 = "002B01";
            string ExpectedLoc4 = "002A02";
            string ExpectedLoc5 = "002B02";
            string ExpectedLoc6 = "001A03";
            string ExpectedLoc7 = "001A04";

            // Act
            List<Location> ActualSortedLocations = new List<Location> { TestLocation1, TestLocation2, TestLocation3, TestLocation4, TestLocation5, TestLocation6, TestLocation7 };

            TestLocationComparer.IncreasePriority(1);

            ActualSortedLocations.Sort(TestLocationComparer);

            // Assert
            Assert.AreEqual(ExpectedLoc1, ActualSortedLocations[0].ID);
            Assert.AreEqual(ExpectedLoc2, ActualSortedLocations[1].ID);
            Assert.AreEqual(ExpectedLoc3, ActualSortedLocations[2].ID);
            Assert.AreEqual(ExpectedLoc4, ActualSortedLocations[3].ID);
            Assert.AreEqual(ExpectedLoc5, ActualSortedLocations[4].ID);
            Assert.AreEqual(ExpectedLoc6, ActualSortedLocations[5].ID);
            Assert.AreEqual(ExpectedLoc7, ActualSortedLocations[6].ID);
        }
    }
}
