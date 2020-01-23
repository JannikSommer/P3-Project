using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Model.Log;
using Central_Controller.Central_Controller;
using System.Collections.Generic;

namespace Unit_Tests.IOController_Tests
{
    [TestClass]
    public class Log_Tests
    {
        [TestMethod]
        public void Log_Test1_CheckPartition()
        {
            // Arrange
            Controller TestController = new Controller();

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

            //Expected
            LocationLogMessage ExpectedMessage1 = new LocationLogMessage(new DateTime(2020, 1, 1, 12, 30, 45), "Kurt", "001A01", new List<(string itemId, string countedQuantity)> { ("01", "10"), ("02", "9") });
            LocationLogMessage ExpectedMessage2 = new LocationLogMessage(new DateTime(2020, 1, 1, 12, 30, 45), "Kurt", "001B01", new List<(string itemId, string countedQuantity)> { ("03", "14"), ("04", "13") });

            // Act
            TestController.CheckPartition(TestPartition1);

            // Assert
            Assert.AreEqual(ExpectedMessage1.GetMessageString(), TestController.Cycle.Log.Messages[0].GetMessageString());
            Assert.AreEqual(ExpectedMessage2.GetMessageString(), TestController.Cycle.Log.Messages[1].GetMessageString());
        }

        [TestMethod]
        public void Log_Test2_CheckVerificationPartition()
        {
            // Arrange
            Controller TestController = new Controller();

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

            VerificationPartition TestVerificationPartition1 = new VerificationPartition();
            TestVerificationPartition1.AssignedUser = "Kurt";
            TestVerificationPartition1.AddItem(TestItem1);
            TestVerificationPartition1.AddItem(TestItem2);
            TestVerificationPartition1.AddItem(TestItem3);
            TestVerificationPartition1.AddItem(TestItem4);

            //Expected
            VerificationLogMessage ExpectedMessage1 = new VerificationLogMessage("Kurt", "01", true);
            VerificationLogMessage ExpectedMessage2 = new VerificationLogMessage("Kurt", "02", true);
            VerificationLogMessage ExpectedMessage3 = new VerificationLogMessage("Kurt", "03", true);
            VerificationLogMessage ExpectedMessage4 = new VerificationLogMessage("Kurt", "04", true);

            // Act
            TestController.CheckVerificationPartition(TestVerificationPartition1);

            // Assert
            Assert.AreEqual(4, TestVerificationPartition1.Items.Count);

            Assert.AreEqual(ExpectedMessage1.GetMessageString(), TestController.Cycle.Log.Messages[0].GetMessageString());
            Assert.AreEqual(ExpectedMessage2.GetMessageString(), TestController.Cycle.Log.Messages[1].GetMessageString());
            Assert.AreEqual(ExpectedMessage3.GetMessageString(), TestController.Cycle.Log.Messages[2].GetMessageString());
            Assert.AreEqual(ExpectedMessage4.GetMessageString(), TestController.Cycle.Log.Messages[3].GetMessageString());
        }
    }
}
