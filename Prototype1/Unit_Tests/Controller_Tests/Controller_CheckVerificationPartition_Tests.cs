using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Central_Controller;

namespace Unit_Tests.Controller_Tests
{
    [TestClass]
    public class Controller_CheckVerificationPartition_Tests
    {
        [TestMethod]
        public void CheckVerificationPartition_Test1()
        {
            // Arrange
            VerificationPartition TestVerificationPartition = new VerificationPartition();

            Controller TestController = new Controller();

            Item TestItem1 = new Item("001");
            TestVerificationPartition.AddItem(TestItem1);
            TestItem1.ServerQuantity = 10;
            TestItem1.CountedQuantity = 10;
            Location TestLocation1 = new Location("001A01");
            TestItem1.AddLocation(TestLocation1);

            Item TestItem2 = new Item("002");
            TestVerificationPartition.AddItem(TestItem2);
            TestItem2.ServerQuantity = 10;
            TestItem2.CountedQuantity = 8;
            Location TestLocation2 = new Location("002A01");
            TestItem2.AddLocation(TestLocation2);

            Item TestItem3 = new Item("003");
            TestVerificationPartition.AddItem(TestItem3);
            TestItem3.ServerQuantity = 10;
            TestItem3.CountedQuantity = 8;
            Location TestLocation3 = new Location("003A01");
            TestItem3.AddLocation(TestLocation3);

            Item TestItem4 = new Item("004");
            TestVerificationPartition.AddItem(TestItem4);
            TestItem4.ServerQuantity = 10;
            Location TestLocation4 = new Location("004A01");
            TestItem4.AddLocation(TestLocation4);

            Item TestItem5 = new Item("005");
            TestVerificationPartition.AddItem(TestItem5);
            TestItem5.ServerQuantity = 10;
            TestItem5.CountedQuantity = 10;
            Location TestLocation5 = new Location("005A01");
            TestItem5.AddLocation(TestLocation5);

            //Expected
            int Expected_NumOfVerifiedItems = 4;
            int Expected_VerifiedItemsCount = 2;

            // Act
            TestController.CheckVerificationPartition(TestVerificationPartition);

            // Assert
            Assert.AreEqual(Expected_NumOfVerifiedItems, TestController.NumOfItemsVerified);
            Assert.AreEqual(Expected_VerifiedItemsCount, TestController.VerifiedItems.Count);
        }
    }
}
