using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Model.Log;
using Central_Controller.Central_Controller;
using System.Collections.Generic;
using Central_Controller.IO;

namespace Unit_Tests.IOController_Tests
{
    [TestClass]
    public class SaveLogMessages_Tests
    {
        [TestMethod]
        public void SaveLog_test1()
        {
            // Arrange
            Controller TestController = new Controller();
            LogWriter TestLogWriter = new LogWriter(Environment.CurrentDirectory + "Test.txt",TestController.Cycle.Log);
            LogReader TestlogReader = new LogReader();

            LogFile TestLogFile = null;

            Item TestItem1 = new Item("01");
            TestItem1.CountedQuantity = 10;
            TestItem1.ServerQuantity = 10;

            Item TestItem2 = new Item("02");
            TestItem2.CountedQuantity = 9;
            TestItem2.ServerQuantity = 9;

            Item TestItem3 = new Item("03");
            TestItem3.CountedQuantity = 14;
            TestItem3.ServerQuantity = 14;

            Item TestItem4 = new Item("04");
            TestItem4.CountedQuantity = 13;
            TestItem4.ServerQuantity = 13;

            Location TestLocation1 = new Location("001A01");
            TestLocation1.AddItem(TestItem1);
            TestLocation1.AddItem(TestItem2);

            Location TestLocation2 = new Location("001B01");
            TestLocation2.AddItem(TestItem3);
            TestLocation2.AddItem(TestItem4);

            Partition TestPartition1 = new Partition();
            TestPartition1.AssignedUser = "Kurt";
            TestPartition1.AddLocation(TestLocation1);
            TestPartition1.AddLocation(TestLocation2);

            // Act
            TestController.CheckPartition(TestPartition1);
            TestLogWriter.SaveNewMessages();
            TestLogFile = TestlogReader.GetLogFromFile(Environment.CurrentDirectory + "Test.txt");

            // Assert
            Assert.AreEqual(2, TestLogFile.Messages.Count);
        }
    }
}
