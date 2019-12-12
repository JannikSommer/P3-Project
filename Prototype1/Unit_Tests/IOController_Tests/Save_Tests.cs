using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Central_Controller;
using Central_Controller.IO;
using System.Collections.Generic;

namespace Unit_Tests.IOController_Tests
{
    [TestClass]
    public class Save_Tests
    {
        [TestMethod]
        public void Save_Test1()
        {
            // Arrange
            Location TestLocation1 = new Location("000A00");
            Location TestLocation2 = new Location("001A00");
            Location TestLocation3 = new Location("002A00");

            List<Location> TestLocationList = new List<Location> { TestLocation1, TestLocation2, TestLocation3 };

            Controller TestController = new Controller();
            TestController.Location_Comparer = new LocationComparer(2);

            //Expected
            List<string> Expected_List1 = new List<string> { "000A00", "001A00", "002A00" };
            List<string> Expected_List2 = new List<string> { "001A00", "000A00", "002A00" };
            List<string> Expected_List3 = new List<string> { "000A00", "002A00", "001A00" };

            // Act save 1
            new IOController(TestController.Cycle.Id).Save(TestController.Cycle, TestController.Location_Comparer.ShelfHierarchy);
            TestController = new Controller();

            TestLocationList.Sort(TestController.Location_Comparer);

            // Assert save 1
            for(int x = 0; x <= 2; x++)
            {
                Assert.AreEqual(Expected_List1[x], TestLocationList[x].ID);
            }

            // Act save 2
            TestController.Location_Comparer.DecreasePriority(1);

            new IOController(TestController.Cycle.Id).Save(TestController.Cycle, TestController.Location_Comparer.ShelfHierarchy);
            TestController = new Controller();

            TestLocationList.Sort(TestController.Location_Comparer);

            // Assert save 2
            for (int x = 0; x <= 2; x++)
            {
                Assert.AreEqual(Expected_List2[x], TestLocationList[x].ID);
            }

            // Act save 3
            TestController.Location_Comparer.IncreasePriority(1);
            TestController.Location_Comparer.IncreasePriority(1);

            new IOController(TestController.Cycle.Id).Save(TestController.Cycle, TestController.Location_Comparer.ShelfHierarchy);
            TestController = new Controller();

            TestLocationList.Sort(TestController.Location_Comparer);

            // Assert save 3
            for (int x = 0; x <= 2; x++)
            {
                Assert.AreEqual(Expected_List3[x], TestLocationList[x].ID);
            }
        }
    }
}
