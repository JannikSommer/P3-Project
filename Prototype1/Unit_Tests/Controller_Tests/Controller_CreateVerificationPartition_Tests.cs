using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Central_Controller;

namespace Unit_Tests.Controller_Tests
{
    [TestClass]
    public class Controller_CreateVerificationPartition_Tests //all test created with the assumption that Controller.MaxNumLocationForVerificationPartitions = 20
    {
        [TestMethod]
        public void CreateVerificationPartition_Test1()
        {
            // Arrange
            Controller TestController = new Controller();

            Item TestItem1 = new Item("001", "T-Shirt", "White", "Large");
            Item TestItem2 = new Item("002", "Pants", "Black", "Medium");

            TestController.InitialAddItem(TestItem1, new List<string> { "001A01" });
            TestController.InitialAddItem(TestItem2, new List<string> { "010A01" }); 

            TestController.InitialPartitioningOfLocations(); //makes sure LocationComparer is created with MaxValueShelf = 10

            TestItem1.ServerQuantity = 10;
            TestItem1.CountedQuantity = 10;

            TestItem2.ServerQuantity = 10;
            TestItem2.CountedQuantity = 1;

            //Expected
            int ExpectedLocationCount = 1;
            int ExpectedItemCount = 1;

            // Act
            TestController.CheckCountedItem(TestItem1);
            TestController.CheckCountedItem(TestItem2);

            VerificationPartition ActualVerificationPartition = TestController.CreateVerificationPartition();

            // Assert
            Assert.AreEqual(ExpectedLocationCount, ActualVerificationPartition.Locations.Count);
            Assert.AreEqual(ExpectedItemCount, ActualVerificationPartition.Items.Count);
        }

        [TestMethod]
        public void CreateVerificationPartition_Test2()
        {
            // Arrange
            Controller TestController = new Controller();

            Item TestItem1 = new Item("001", "T-Shirt", "White", "Large");
            Item TestItem2 = new Item("002", "Pants", "Black", "Medium");

            TestController.InitialAddItem(TestItem1, new List<string> { "001A01" });
            TestController.InitialAddItem(TestItem2, new List<string> { "010A01" });

            TestController.InitialPartitioningOfLocations(); //makes sure LocationComparer is created with MaxValueShelf = 10

            TestItem1.ServerQuantity = 10;
            TestItem1.CountedQuantity = 10;

            TestItem2.ServerQuantity = 10;
            TestItem2.CountedQuantity = -1;

            //Expected
            VerificationPartition ExpectedVerificationPartition = null;

            // Act
            TestController.CheckCountedItem(TestItem1);
            TestController.CheckCountedItem(TestItem2);

            VerificationPartition ActualVerificationPartition = TestController.CreateVerificationPartition();

            // Assert
            Assert.AreEqual(ExpectedVerificationPartition, ActualVerificationPartition);
        }

        [TestMethod]
        public void CreateVerificationPartition_Test3()
        {
            // Arrange
            Controller TestController = new Controller();

            Item TestItem1 = new Item("001", "T-Shirt", "White", "Large");
            Item TestItem2 = new Item("002", "Pants", "Black", "Medium");

            TestController.InitialAddItem(TestItem1, new List<string> { "001A01" });
            TestController.InitialAddItem(TestItem2, new List<string> { "010A01" });

            TestController.InitialPartitioningOfLocations(); //makes sure LocationComparer is created with MaxValueShelf = 10

            TestItem1.ServerQuantity = 10;
            TestItem1.CountedQuantity = 9;

            TestItem2.ServerQuantity = 10;
            TestItem2.CountedQuantity = 0;

            //Expected
            int ExpectedLocationCount = 2;
            int ExpectedItemCount = 2;

            // Act
            TestController.CheckCountedItem(TestItem1);
            TestController.CheckCountedItem(TestItem2);

            VerificationPartition ActualVerificationPartition = TestController.CreateVerificationPartition();

            // Assert
            Assert.AreEqual(ExpectedLocationCount, ActualVerificationPartition.Locations.Count);
            Assert.AreEqual(ExpectedItemCount, ActualVerificationPartition.Items.Count);
        }

        [TestMethod]
        public void CreateVerificationPartition_Test4()
        {
            // Arrange
            Controller TestController = new Controller();

            Item TestItem1 = new Item("001", "T-Shirt", "White", "Large");
            Item TestItem2 = new Item("002", "Pants", "Black", "Medium");
            Item TestItem3 = new Item("003");
            Item TestItem4 = new Item("004");
            Item TestItem5 = new Item("005");
            Item TestItem6 = new Item("006");
            Item TestItem7 = new Item("007");
            Item TestItem8 = new Item("008");
            Item TestItem9 = new Item("009");
            Item TestItem10 = new Item("010");
            Item TestItem11 = new Item("011");
            Item TestItem12 = new Item("012");
            Item TestItem13 = new Item("013");
            Item TestItem14 = new Item("014");
            Item TestItem15 = new Item("015");
            Item TestItem16 = new Item("016");

            Location TestLocation1 = new Location("001A01");
            Location TestLocation2 = new Location("001B01");
            Location TestLocation3 = new Location("001C01");
            Location TestLocation4 = new Location("001A02");
            Location TestLocation5 = new Location("001A03");
            Location TestLocation6 = new Location("001A04");
            Location TestLocation7 = new Location("001A05");
            Location TestLocation8 = new Location("002A01");
            Location TestLocation9 = new Location("002A01");
            Location TestLocation10 = new Location("002A01");
            Location TestLocation11 = new Location("002B01");
            Location TestLocation12 = new Location("003B01");
            Location TestLocation13 = new Location("003B01");
            Location TestLocation14 = new Location("003A02");
            Location TestLocation15 = new Location("003A02");
            Location TestLocation16 = new Location("003A02");
            Location TestLocation17 = new Location("004A01");
            Location TestLocation18 = new Location("004A01");
            Location TestLocation19 = new Location("004A03");
            Location TestLocation20 = new Location("005A06");
            Location TestLocation21 = new Location("005A07");
            Location TestLocation22 = new Location("005C07");

            Item DumpItem1 = new Item("DumpItem");

            TestController.InitialAddItem(DumpItem1, new List<string> { "005A01" });

            TestController.InitialPartitioningOfLocations(); //makes sure LocationComparer is created with MaxValueShelf = 5

            TestItem1.AddLocation(TestLocation1);
            TestItem1.AddLocation(TestLocation8);
            TestItem1.AddLocation(TestLocation12);
            TestItem1.ServerQuantity = 10;
            TestItem1.CountedQuantity = 9;

            TestItem2.AddLocation(TestLocation1);
            TestItem2.ServerQuantity = 10;
            TestItem2.CountedQuantity = 9;

            TestItem3.AddLocation(TestLocation1);
            TestItem3.ServerQuantity = 10;
            TestItem3.CountedQuantity = 9;

            TestItem4.AddLocation(TestLocation1);
            TestItem4.ServerQuantity = 10;
            TestItem4.CountedQuantity = 9;

            TestItem5.AddLocation(TestLocation1);
            TestItem5.ServerQuantity = 10;
            TestItem5.CountedQuantity = 9;

            TestItem6.AddLocation(TestLocation1);
            TestItem6.ServerQuantity = 10;
            TestItem6.CountedQuantity = 9;

            TestItem7.AddLocation(TestLocation1);
            TestItem7.ServerQuantity = 10;
            TestItem7.CountedQuantity = 9;

            TestItem8.AddLocation(TestLocation1);
            TestItem8.ServerQuantity = 10;
            TestItem8.CountedQuantity = 9;

            TestItem9.AddLocation(TestLocation1);
            TestItem9.ServerQuantity = 10;
            TestItem9.CountedQuantity = 9;

            TestItem10.AddLocation(TestLocation1);
            TestItem10.ServerQuantity = 10;
            TestItem10.CountedQuantity = 9;

            TestItem11.AddLocation(TestLocation1);
            TestItem11.ServerQuantity = 10;
            TestItem11.CountedQuantity = 9;

            TestItem12.AddLocation(TestLocation1);
            TestItem12.ServerQuantity = 10;
            TestItem12.CountedQuantity = 9;

            TestItem13.AddLocation(TestLocation1);
            TestItem13.ServerQuantity = 10;
            TestItem13.CountedQuantity = 9;

            TestItem14.AddLocation(TestLocation1);
            TestItem14.ServerQuantity = 10;
            TestItem14.CountedQuantity = 9;

            TestItem15.AddLocation(TestLocation1);
            TestItem15.ServerQuantity = 10;
            TestItem15.CountedQuantity = 9;

            TestItem16.AddLocation(TestLocation1);
            TestItem16.ServerQuantity = 10;
            TestItem16.CountedQuantity = 9;


            //Expected
            int ExpectedLocationCount = 3;
            int ExpectedItemCount = 16;

            // Act
            TestController.CheckCountedItem(TestItem1);
            TestController.CheckCountedItem(TestItem2);
            TestController.CheckCountedItem(TestItem3);
            TestController.CheckCountedItem(TestItem4);
            TestController.CheckCountedItem(TestItem5);
            TestController.CheckCountedItem(TestItem6);
            TestController.CheckCountedItem(TestItem7);
            TestController.CheckCountedItem(TestItem8);
            TestController.CheckCountedItem(TestItem9);
            TestController.CheckCountedItem(TestItem10);
            TestController.CheckCountedItem(TestItem11);
            TestController.CheckCountedItem(TestItem12);
            TestController.CheckCountedItem(TestItem13);
            TestController.CheckCountedItem(TestItem14);
            TestController.CheckCountedItem(TestItem15);
            TestController.CheckCountedItem(TestItem16);

            VerificationPartition ActualVerificationPartition = TestController.CreateVerificationPartition();

            // Assert
            Assert.AreEqual(ExpectedLocationCount, ActualVerificationPartition.Locations.Count);
            Assert.AreEqual(ExpectedItemCount, ActualVerificationPartition.Items.Count);
        }

        [TestMethod]
        public void CreateVerificationPartition_Test5()
        {
            // Arrange
            Controller TestController = new Controller();

            Item TestItem1 = new Item("001", "T-Shirt", "White", "Large");
            Item TestItem2 = new Item("002", "Pants", "Black", "Medium");
            Item TestItem3 = new Item("003");
            Item TestItem4 = new Item("004");
            Item TestItem5 = new Item("005");
            Item TestItem6 = new Item("006");
            Item TestItem7 = new Item("007");
            Item TestItem8 = new Item("008");
            Item TestItem9 = new Item("009");
            Item TestItem10 = new Item("010");
            Item TestItem11 = new Item("011");
            Item TestItem12 = new Item("012");
            Item TestItem13 = new Item("013");
            Item TestItem14 = new Item("014");
            Item TestItem15 = new Item("015");
            Item TestItem16 = new Item("016");
            Item TestItem17 = new Item("017");
            Item TestItem18 = new Item("018");
            Item TestItem19 = new Item("019");
            Item TestItem20 = new Item("020");
            Item TestItem21 = new Item("021");
            Item TestItem22 = new Item("022");

            Location TestLocation1 = new Location("001A01");
            Location TestLocation2 = new Location("001B02");
            Location TestLocation3 = new Location("001C03");
            Location TestLocation4 = new Location("001A04");
            Location TestLocation5 = new Location("001A05");
            Location TestLocation6 = new Location("001A06");
            Location TestLocation7 = new Location("001A07");
            Location TestLocation8 = new Location("002A08");
            Location TestLocation9 = new Location("002A09");
            Location TestLocation10 = new Location("002A10");
            Location TestLocation11 = new Location("002B11");
            Location TestLocation12 = new Location("003B12");
            Location TestLocation13 = new Location("003B13");
            Location TestLocation14 = new Location("003A14");
            Location TestLocation15 = new Location("003A15");
            Location TestLocation16 = new Location("003A16");
            Location TestLocation17 = new Location("004A17");
            Location TestLocation18 = new Location("004A18");
            Location TestLocation19 = new Location("004A19");
            Location TestLocation20 = new Location("005A20");
            Location TestLocation21 = new Location("005A21");
            Location TestLocation22 = new Location("005C22");

            Item DumpItem1 = new Item("DumpItem");

            TestController.InitialAddItem(DumpItem1, new List<string> { "005A01" });

            TestController.InitialPartitioningOfLocations(); //makes sure LocationComparer is created with MaxValueShelf = 5

            TestItem1.AddLocation(TestLocation1);
            TestItem1.AddLocation(TestLocation8);
            TestItem1.AddLocation(TestLocation12);
            TestItem1.ServerQuantity = 10;
            TestItem1.CountedQuantity = 9;

            TestItem2.AddLocation(TestLocation2);
            TestItem2.ServerQuantity = 10;
            TestItem2.CountedQuantity = 9;

            TestItem3.AddLocation(TestLocation3);
            TestItem3.ServerQuantity = 10;
            TestItem3.CountedQuantity = 9;

            TestItem4.AddLocation(TestLocation4);
            TestItem4.ServerQuantity = 10;
            TestItem4.CountedQuantity = 9;

            TestItem5.AddLocation(TestLocation5);
            TestItem5.ServerQuantity = 10;
            TestItem5.CountedQuantity = 9;

            TestItem6.AddLocation(TestLocation6);
            TestItem6.ServerQuantity = 10;
            TestItem6.CountedQuantity = 9;

            TestItem7.AddLocation(TestLocation7);
            TestItem7.ServerQuantity = 10;
            TestItem7.CountedQuantity = 9;

            TestItem8.AddLocation(TestLocation8);
            TestItem8.ServerQuantity = 10;
            TestItem8.CountedQuantity = 9;

            TestItem9.AddLocation(TestLocation9);
            TestItem9.ServerQuantity = 10;
            TestItem9.CountedQuantity = 9;

            TestItem10.AddLocation(TestLocation10);
            TestItem10.ServerQuantity = 10;
            TestItem10.CountedQuantity = 9;

            TestItem11.AddLocation(TestLocation11);
            TestItem11.ServerQuantity = 10;
            TestItem11.CountedQuantity = 9;

            TestItem12.AddLocation(TestLocation12);
            TestItem12.ServerQuantity = 10;
            TestItem12.CountedQuantity = 9;

            TestItem13.AddLocation(TestLocation13);
            TestItem13.ServerQuantity = 10;
            TestItem13.CountedQuantity = 9;

            TestItem14.AddLocation(TestLocation14);
            TestItem14.ServerQuantity = 10;
            TestItem14.CountedQuantity = 9;

            TestItem15.AddLocation(TestLocation15);
            TestItem15.ServerQuantity = 10;
            TestItem15.CountedQuantity = 9;

            TestItem16.AddLocation(TestLocation16);
            TestItem16.ServerQuantity = 10;
            TestItem16.CountedQuantity = 9;

            TestItem17.AddLocation(TestLocation17);
            TestItem17.ServerQuantity = 10;
            TestItem17.CountedQuantity = 9;

            TestItem18.AddLocation(TestLocation18);
            TestItem18.ServerQuantity = 10;
            TestItem18.CountedQuantity = 9;

            TestItem19.AddLocation(TestLocation19);
            TestItem19.ServerQuantity = 10;
            TestItem19.CountedQuantity = 9;

            TestItem20.AddLocation(TestLocation20);
            TestItem20.ServerQuantity = 10;
            TestItem20.CountedQuantity = 9;

            TestItem21.AddLocation(TestLocation21);
            TestItem21.ServerQuantity = 10;
            TestItem21.CountedQuantity = 9;

            TestItem22.AddLocation(TestLocation22);
            TestItem22.ServerQuantity = 10;
            TestItem22.CountedQuantity = 9;


            //Expected
            int ExpectedLocationCount = 20;
            int ExpectedItemCount = 20;

            // Act
            TestController.CheckCountedItem(TestItem1);
            TestController.CheckCountedItem(TestItem2);
            TestController.CheckCountedItem(TestItem3);
            TestController.CheckCountedItem(TestItem4);
            TestController.CheckCountedItem(TestItem5);
            TestController.CheckCountedItem(TestItem6);
            TestController.CheckCountedItem(TestItem7);
            TestController.CheckCountedItem(TestItem8);
            TestController.CheckCountedItem(TestItem9);
            TestController.CheckCountedItem(TestItem10);
            TestController.CheckCountedItem(TestItem11);
            TestController.CheckCountedItem(TestItem12);
            TestController.CheckCountedItem(TestItem13);
            TestController.CheckCountedItem(TestItem14);
            TestController.CheckCountedItem(TestItem15);
            TestController.CheckCountedItem(TestItem16);
            TestController.CheckCountedItem(TestItem17);
            TestController.CheckCountedItem(TestItem18);
            TestController.CheckCountedItem(TestItem19);
            TestController.CheckCountedItem(TestItem20);
            TestController.CheckCountedItem(TestItem21);
            TestController.CheckCountedItem(TestItem22);

            VerificationPartition ActualVerificationPartition = TestController.CreateVerificationPartition();

            // Assert
            Assert.AreEqual(ExpectedItemCount, ActualVerificationPartition.Items.Count);
            Assert.AreEqual(ExpectedLocationCount, ActualVerificationPartition.Locations.Count);
        }

        [TestMethod]
        public void CreateVerificationPartition_Test6()
        {
            // Arrange
            Controller TestController = new Controller();

            Item TestItem1 = new Item("001", "T-Shirt", "White", "Large");
            Item TestItem2 = new Item("002", "Pants", "Black", "Medium");
            Item TestItem3 = new Item("003");
            Item TestItem4 = new Item("004");
            Item TestItem5 = new Item("005");
            Item TestItem6 = new Item("006");
            Item TestItem7 = new Item("007");
            Item TestItem8 = new Item("008");
            Item TestItem9 = new Item("009");
            Item TestItem10 = new Item("010");
            Item TestItem11 = new Item("011");
            Item TestItem12 = new Item("012");
            Item TestItem13 = new Item("013");
            Item TestItem14 = new Item("014");
            Item TestItem15 = new Item("015");
            Item TestItem16 = new Item("016");
            Item TestItem17 = new Item("017");
            Item TestItem18 = new Item("018");
            Item TestItem19 = new Item("019");
            Item TestItem20 = new Item("020");
            Item TestItem21 = new Item("021");
            Item TestItem22 = new Item("022");

            Location TestLocation1 = new Location("001A01");
            Location TestLocation2 = new Location("001B02");
            Location TestLocation3 = new Location("001C03");
            Location TestLocation4 = new Location("001A04");
            Location TestLocation5 = new Location("001A05");
            Location TestLocation6 = new Location("001A06");
            Location TestLocation7 = new Location("001A07");
            Location TestLocation8 = new Location("002A08");
            Location TestLocation9 = new Location("002A09");
            Location TestLocation10 = new Location("002A10");
            Location TestLocation11 = new Location("002B11");
            Location TestLocation12 = new Location("003B12");
            Location TestLocation13 = new Location("003B13");
            Location TestLocation14 = new Location("003A14");
            Location TestLocation15 = new Location("003A15");
            Location TestLocation16 = new Location("003A16");
            Location TestLocation17 = new Location("004A17");
            Location TestLocation18 = new Location("004A18");
            Location TestLocation19 = new Location("004A19");
            Location TestLocation20 = new Location("005A20");
            Location TestLocation21 = new Location("005A21");
            Location TestLocation22 = new Location("005C22");

            Item DumpItem1 = new Item("DumpItem");

            TestController.InitialAddItem(DumpItem1, new List<string> { "005A01" });

            TestController.InitialPartitioningOfLocations(); //makes sure LocationComparer is created with MaxValueShelf = 5

            TestItem1.AddLocation(TestLocation1);
            TestItem1.AddLocation(TestLocation8);
            TestItem1.AddLocation(TestLocation12);
            TestItem1.ServerQuantity = 10;
            TestItem1.CountedQuantity = 9;

            TestItem2.AddLocation(TestLocation2);
            TestItem2.ServerQuantity = 10;
            TestItem2.CountedQuantity = 9;

            TestItem3.AddLocation(TestLocation3);
            TestItem3.ServerQuantity = 10;
            TestItem3.CountedQuantity = 9;

            TestItem4.AddLocation(TestLocation4);
            TestItem4.ServerQuantity = 10;
            TestItem4.CountedQuantity = 9;

            TestItem5.AddLocation(TestLocation5);
            TestItem5.ServerQuantity = 10;
            TestItem5.CountedQuantity = 9;

            TestItem6.AddLocation(TestLocation6);
            TestItem6.ServerQuantity = 10;
            TestItem6.CountedQuantity = 9;

            TestItem7.AddLocation(TestLocation7);
            TestItem7.ServerQuantity = 10;
            TestItem7.CountedQuantity = 9;

            TestItem8.AddLocation(TestLocation8);
            TestItem8.ServerQuantity = 10;
            TestItem8.CountedQuantity = 9;

            TestItem9.AddLocation(TestLocation9);
            TestItem9.ServerQuantity = 10;
            TestItem9.CountedQuantity = 9;

            TestItem10.AddLocation(TestLocation10);
            TestItem10.ServerQuantity = 10;
            TestItem10.CountedQuantity = 9;

            TestItem11.AddLocation(TestLocation11);
            TestItem11.ServerQuantity = 10;
            TestItem11.CountedQuantity = 9;

            TestItem12.AddLocation(TestLocation12);
            TestItem12.ServerQuantity = 10;
            TestItem12.CountedQuantity = 9;

            TestItem13.AddLocation(TestLocation13);
            TestItem13.ServerQuantity = 10;
            TestItem13.CountedQuantity = 9;

            TestItem14.AddLocation(TestLocation14);
            TestItem14.ServerQuantity = 10;
            TestItem14.CountedQuantity = 9;

            TestItem15.AddLocation(TestLocation15);
            TestItem15.ServerQuantity = 10;
            TestItem15.CountedQuantity = 9;

            TestItem16.AddLocation(TestLocation16);
            TestItem16.ServerQuantity = 10;
            TestItem16.CountedQuantity = 9;

            TestItem17.AddLocation(TestLocation17);
            TestItem17.ServerQuantity = 10;
            TestItem17.CountedQuantity = 9;

            TestItem18.AddLocation(TestLocation18);
            TestItem18.ServerQuantity = 10;
            TestItem18.CountedQuantity = 9;

            TestItem19.AddLocation(TestLocation19);
            TestItem19.ServerQuantity = 10;
            TestItem19.CountedQuantity = 9;

            TestItem20.AddLocation(TestLocation20);
            TestItem20.ServerQuantity = 10;
            TestItem20.CountedQuantity = 9;

            TestItem21.AddLocation(TestLocation2);
            TestItem21.ServerQuantity = 10;
            TestItem21.CountedQuantity = 9;

            TestItem22.AddLocation(TestLocation21);
            TestItem22.AddLocation(TestLocation22);
            TestItem22.ServerQuantity = 10;
            TestItem22.CountedQuantity = 9;


            //Expected
            int ExpectedLocationCount = 20;
            int ExpectedItemCount = 21;

            // Act
            TestController.CheckCountedItem(TestItem1);
            TestController.CheckCountedItem(TestItem2);
            TestController.CheckCountedItem(TestItem3);
            TestController.CheckCountedItem(TestItem4);
            TestController.CheckCountedItem(TestItem5);
            TestController.CheckCountedItem(TestItem6);
            TestController.CheckCountedItem(TestItem7);
            TestController.CheckCountedItem(TestItem8);
            TestController.CheckCountedItem(TestItem9);
            TestController.CheckCountedItem(TestItem10);
            TestController.CheckCountedItem(TestItem11);
            TestController.CheckCountedItem(TestItem12);
            TestController.CheckCountedItem(TestItem13);
            TestController.CheckCountedItem(TestItem14);
            TestController.CheckCountedItem(TestItem15);
            TestController.CheckCountedItem(TestItem16);
            TestController.CheckCountedItem(TestItem17);
            TestController.CheckCountedItem(TestItem18);
            TestController.CheckCountedItem(TestItem19);
            TestController.CheckCountedItem(TestItem20);
            TestController.CheckCountedItem(TestItem21);
            TestController.CheckCountedItem(TestItem22);

            VerificationPartition ActualVerificationPartition = TestController.CreateVerificationPartition();

            // Assert
            Assert.AreEqual(ExpectedItemCount, ActualVerificationPartition.Items.Count);
            Assert.AreEqual(ExpectedLocationCount, ActualVerificationPartition.Locations.Count);
        }

        [TestMethod]
        public void CreateVerificationPartition_Test7()
        {
            // Arrange
            Controller TestController = new Controller();

            Item TestItem1 = new Item("001", "T-Shirt", "White", "Large");
            Item TestItem2 = new Item("002", "Pants", "Black", "Medium");
            Item TestItem3 = new Item("003");
            Item TestItem4 = new Item("004");
            Item TestItem5 = new Item("005");
            Item TestItem6 = new Item("006");
            Item TestItem7 = new Item("007");
            Item TestItem8 = new Item("008");
            Item TestItem9 = new Item("009");
            Item TestItem10 = new Item("010");
            Item TestItem11 = new Item("011");
            Item TestItem12 = new Item("012");
            Item TestItem13 = new Item("013");
            Item TestItem14 = new Item("014");
            Item TestItem15 = new Item("015");
            Item TestItem16 = new Item("016");
            Item TestItem17 = new Item("017");
            Item TestItem18 = new Item("018");
            Item TestItem19 = new Item("019");
            Item TestItem20 = new Item("020");
            Item TestItem21 = new Item("021");
            Item TestItem22 = new Item("022");

            Location TestLocation1 = new Location("001A01");
            Location TestLocation2 = new Location("001B02");
            Location TestLocation3 = new Location("001C03");
            Location TestLocation4 = new Location("001A04");
            Location TestLocation5 = new Location("001A05");
            Location TestLocation6 = new Location("001A06");
            Location TestLocation7 = new Location("001A07");
            Location TestLocation8 = new Location("002A08");
            Location TestLocation9 = new Location("002A09");
            Location TestLocation10 = new Location("002A10");
            Location TestLocation11 = new Location("002B11");
            Location TestLocation12 = new Location("003B12");
            Location TestLocation13 = new Location("003B13");
            Location TestLocation14 = new Location("003A14");
            Location TestLocation15 = new Location("003A15");
            Location TestLocation16 = new Location("003A16");
            Location TestLocation17 = new Location("004A17");
            Location TestLocation18 = new Location("004A18");
            Location TestLocation19 = new Location("004A19");
            Location TestLocation20 = new Location("005A20");
            Location TestLocation21 = new Location("005A21");
            Location TestLocation22 = new Location("005C22");

            Item DumpItem1 = new Item("DumpItem");

            TestController.InitialAddItem(DumpItem1, new List<string> { "005A01" });

            TestController.InitialPartitioningOfLocations(); //makes sure LocationComparer is created with MaxValueShelf = 5

            TestItem1.AddLocation(TestLocation1);
            TestItem1.AddLocation(TestLocation8);
            TestItem1.AddLocation(TestLocation12);
            TestItem1.ServerQuantity = 10;
            TestItem1.CountedQuantity = 9;

            TestItem2.AddLocation(TestLocation2);
            TestItem2.ServerQuantity = 10;
            TestItem2.CountedQuantity = 9;

            TestItem3.AddLocation(TestLocation3);
            TestItem3.ServerQuantity = 10;
            TestItem3.CountedQuantity = 9;

            TestItem4.AddLocation(TestLocation4);
            TestItem4.ServerQuantity = 10;
            TestItem4.CountedQuantity = 9;

            TestItem5.AddLocation(TestLocation5);
            TestItem5.ServerQuantity = 10;
            TestItem5.CountedQuantity = 9;

            TestItem6.AddLocation(TestLocation6);
            TestItem6.ServerQuantity = 10;
            TestItem6.CountedQuantity = 9;

            TestItem7.AddLocation(TestLocation7);
            TestItem7.ServerQuantity = 10;
            TestItem7.CountedQuantity = 9;

            TestItem8.AddLocation(TestLocation8);
            TestItem8.ServerQuantity = 10;
            TestItem8.CountedQuantity = 9;

            TestItem9.AddLocation(TestLocation9);
            TestItem9.ServerQuantity = 10;
            TestItem9.CountedQuantity = 9;

            TestItem10.AddLocation(TestLocation10);
            TestItem10.ServerQuantity = 10;
            TestItem10.CountedQuantity = 9;

            TestItem11.AddLocation(TestLocation11);
            TestItem11.ServerQuantity = 10;
            TestItem11.CountedQuantity = 9;

            TestItem12.AddLocation(TestLocation12);
            TestItem12.ServerQuantity = 10;
            TestItem12.CountedQuantity = 9;

            TestItem13.AddLocation(TestLocation13);
            TestItem13.ServerQuantity = 10;
            TestItem13.CountedQuantity = 9;

            TestItem14.AddLocation(TestLocation14);
            TestItem14.ServerQuantity = 10;
            TestItem14.CountedQuantity = 9;

            TestItem15.AddLocation(TestLocation15);
            TestItem15.ServerQuantity = 10;
            TestItem15.CountedQuantity = 9;

            TestItem16.AddLocation(TestLocation16);
            TestItem16.ServerQuantity = 10;
            TestItem16.CountedQuantity = 9;

            TestItem17.AddLocation(TestLocation17);
            TestItem17.ServerQuantity = 10;
            TestItem17.CountedQuantity = 9;

            TestItem18.AddLocation(TestLocation18);
            TestItem18.ServerQuantity = 10;
            TestItem18.CountedQuantity = 9;

            TestItem19.AddLocation(TestLocation19);
            TestItem19.ServerQuantity = 10;
            TestItem19.CountedQuantity = 9;

            TestItem20.AddLocation(TestLocation2);
            TestItem20.ServerQuantity = 10;
            TestItem20.CountedQuantity = 9;

            TestItem21.AddLocation(TestLocation2);
            TestItem21.ServerQuantity = 10;
            TestItem21.CountedQuantity = 9;

            TestItem22.AddLocation(TestLocation20);
            TestItem22.AddLocation(TestLocation21);
            TestItem22.AddLocation(TestLocation22);
            TestItem22.ServerQuantity = 10;
            TestItem22.CountedQuantity = 9;


            //Expected
            int ExpectedLocationCount = 22;
            int ExpectedItemCount = 22;

            // Act
            TestController.CheckCountedItem(TestItem1);
            TestController.CheckCountedItem(TestItem2);
            TestController.CheckCountedItem(TestItem3);
            TestController.CheckCountedItem(TestItem4);
            TestController.CheckCountedItem(TestItem5);
            TestController.CheckCountedItem(TestItem6);
            TestController.CheckCountedItem(TestItem7);
            TestController.CheckCountedItem(TestItem8);
            TestController.CheckCountedItem(TestItem9);
            TestController.CheckCountedItem(TestItem10);
            TestController.CheckCountedItem(TestItem11);
            TestController.CheckCountedItem(TestItem12);
            TestController.CheckCountedItem(TestItem13);
            TestController.CheckCountedItem(TestItem14);
            TestController.CheckCountedItem(TestItem15);
            TestController.CheckCountedItem(TestItem16);
            TestController.CheckCountedItem(TestItem17);
            TestController.CheckCountedItem(TestItem18);
            TestController.CheckCountedItem(TestItem19);
            TestController.CheckCountedItem(TestItem20);
            TestController.CheckCountedItem(TestItem21);
            TestController.CheckCountedItem(TestItem22);

            VerificationPartition ActualVerificationPartition = TestController.CreateVerificationPartition();

            // Assert
            Assert.AreEqual(ExpectedItemCount, ActualVerificationPartition.Items.Count);
            Assert.AreEqual(ExpectedLocationCount, ActualVerificationPartition.Locations.Count);
        }

        [TestMethod]
        public void CreateVerificationPartition_Test8()
        {
            // Arrange
            Controller TestController = new Controller();

            Item TestItem1 = new Item("001", "T-Shirt", "White", "Large");
            Item TestItem2 = new Item("002", "Pants", "Black", "Medium");
            Item TestItem3 = new Item("003");
            Item TestItem4 = new Item("004");
            Item TestItem5 = new Item("005");
            Item TestItem6 = new Item("006");
            Item TestItem7 = new Item("007");
            Item TestItem8 = new Item("008");
            Item TestItem9 = new Item("009");
            Item TestItem10 = new Item("010");
            Item TestItem11 = new Item("011");
            Item TestItem12 = new Item("012");
            Item TestItem13 = new Item("013");
            Item TestItem14 = new Item("014");
            Item TestItem15 = new Item("015");
            Item TestItem16 = new Item("016");
            Item TestItem17 = new Item("017");
            Item TestItem18 = new Item("018");
            Item TestItem19 = new Item("019");
            Item TestItem20 = new Item("020");
            Item TestItem21 = new Item("021");
            //Item TestItem22 = new Item("022");

            Location TestLocation1 = new Location("001A01");
            Location TestLocation2 = new Location("001B02");
            Location TestLocation3 = new Location("001C03");
            Location TestLocation4 = new Location("001A04");
            Location TestLocation5 = new Location("001A05");
            Location TestLocation6 = new Location("001A06");
            Location TestLocation7 = new Location("001A07");
            Location TestLocation8 = new Location("002A08");
            Location TestLocation9 = new Location("002A09");
            Location TestLocation10 = new Location("002A10");
            Location TestLocation11 = new Location("002B11");
            Location TestLocation12 = new Location("003B12");
            Location TestLocation13 = new Location("003B13");
            Location TestLocation14 = new Location("003A14");
            Location TestLocation15 = new Location("003A15");
            Location TestLocation16 = new Location("003A16");
            Location TestLocation17 = new Location("004A17");
            Location TestLocation18 = new Location("004A18");
            Location TestLocation19 = new Location("004A19");
            Location TestLocation20 = new Location("001A20");
            Location TestLocation21 = new Location("001A21");
            Location TestLocation22 = new Location("001C22");

            Item DumpItem1 = new Item("DumpItem");

            TestController.InitialAddItem(DumpItem1, new List<string> { "005A01" });

            TestController.InitialPartitioningOfLocations(); //makes sure LocationComparer is initialized with MaxValueShelf = 5

            TestItem1.AddLocation(TestLocation1);
            TestItem1.AddLocation(TestLocation8);
            TestItem1.AddLocation(TestLocation12);
            TestItem1.ServerQuantity = 10;
            TestItem1.CountedQuantity = 9;

            TestItem2.AddLocation(TestLocation2);
            TestItem2.ServerQuantity = 10;
            TestItem2.CountedQuantity = 9;

            TestItem3.AddLocation(TestLocation3);
            TestItem3.ServerQuantity = 10;
            TestItem3.CountedQuantity = 9;

            TestItem4.AddLocation(TestLocation4);
            TestItem4.ServerQuantity = 10;
            TestItem4.CountedQuantity = 9;

            TestItem5.AddLocation(TestLocation5);
            TestItem5.ServerQuantity = 10;
            TestItem5.CountedQuantity = 9;

            TestItem6.AddLocation(TestLocation6);
            TestItem6.ServerQuantity = 10;
            TestItem6.CountedQuantity = 9;

            TestItem7.AddLocation(TestLocation7);
            TestItem7.ServerQuantity = 10;
            TestItem7.CountedQuantity = 9;

            TestItem8.AddLocation(TestLocation8);
            TestItem8.ServerQuantity = 10;
            TestItem8.CountedQuantity = 9;

            TestItem9.AddLocation(TestLocation9);
            TestItem9.ServerQuantity = 10;
            TestItem9.CountedQuantity = 9;

            TestItem10.AddLocation(TestLocation10);
            TestItem10.ServerQuantity = 10;
            TestItem10.CountedQuantity = 9;

            TestItem11.AddLocation(TestLocation11);
            TestItem11.ServerQuantity = 10;
            TestItem11.CountedQuantity = 9;

            TestItem12.AddLocation(TestLocation12);
            TestItem12.ServerQuantity = 10;
            TestItem12.CountedQuantity = 9;

            TestItem13.AddLocation(TestLocation13);
            TestItem13.ServerQuantity = 10;
            TestItem13.CountedQuantity = 9;

            TestItem14.AddLocation(TestLocation14);
            TestItem14.ServerQuantity = 10;
            TestItem14.CountedQuantity = 9;

            TestItem15.AddLocation(TestLocation15);
            TestItem15.ServerQuantity = 10;
            TestItem15.CountedQuantity = 9;

            TestItem16.AddLocation(TestLocation16);
            TestItem16.ServerQuantity = 10;
            TestItem16.CountedQuantity = 9;

            TestItem17.AddLocation(TestLocation17);
            TestItem17.ServerQuantity = 10;
            TestItem17.CountedQuantity = 9;

            TestItem18.AddLocation(TestLocation18);
            TestItem18.ServerQuantity = 10;
            TestItem18.CountedQuantity = 9;

            TestItem19.AddLocation(TestLocation19);
            TestItem19.ServerQuantity = 10;
            TestItem19.CountedQuantity = 9;

            TestItem20.AddLocation(TestLocation20);
            TestItem20.ServerQuantity = 10;
            TestItem20.CountedQuantity = 9;

            TestItem21.AddLocation(TestLocation21); //item will be prioritized, because its multilocation and has has less then 1000 distance from TestItem1's locations, because they are also on Shelf 1
            TestItem21.AddLocation(TestLocation22);
            TestItem21.ServerQuantity = 10;
            TestItem21.CountedQuantity = 9;

            //TestItem22.AddLocation(TestLocation21);
            //TestItem22.AddLocation(TestLocation22);
            //TestItem22.ServerQuantity = 10;
            //TestItem22.CountedQuantity = 9;


            //Expected
            int ExpectedLocationCount = 20;
            int ExpectedItemCount = 19;

            // Act
            TestController.CheckCountedItem(TestItem1);
            TestController.CheckCountedItem(TestItem2);
            TestController.CheckCountedItem(TestItem3);
            TestController.CheckCountedItem(TestItem4);
            TestController.CheckCountedItem(TestItem5);
            TestController.CheckCountedItem(TestItem6);
            TestController.CheckCountedItem(TestItem7);
            TestController.CheckCountedItem(TestItem8);
            TestController.CheckCountedItem(TestItem9);
            TestController.CheckCountedItem(TestItem10);
            TestController.CheckCountedItem(TestItem11);
            TestController.CheckCountedItem(TestItem12);
            TestController.CheckCountedItem(TestItem13);
            TestController.CheckCountedItem(TestItem14);
            TestController.CheckCountedItem(TestItem15);
            TestController.CheckCountedItem(TestItem16);
            TestController.CheckCountedItem(TestItem17);
            TestController.CheckCountedItem(TestItem18);
            TestController.CheckCountedItem(TestItem19);
            TestController.CheckCountedItem(TestItem20);
            TestController.CheckCountedItem(TestItem21);

            VerificationPartition ActualVerificationPartition = TestController.CreateVerificationPartition();

            // Assert
            Assert.AreEqual(ExpectedItemCount, ActualVerificationPartition.Items.Count);
            Assert.AreEqual(ExpectedLocationCount, ActualVerificationPartition.Locations.Count);
        }
    }
}
