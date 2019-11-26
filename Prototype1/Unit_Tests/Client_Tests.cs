using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF_PC.Central_Controller;
using Model;

namespace Unit_Tests
{
    [TestClass]
    public class Client_Tests
    {
        [TestMethod]
        public void ConstructorTest1()
        {
            // Arrange
            Client TestClient;

            //Expected
            string Expected_ID = "01";

            // Act
            TestClient = new Client("01");

            // Assert
            Assert.AreEqual(Expected_ID, TestClient.ID);
        }

        [TestMethod]
        public void AFK_test1()
        {
            // Arrange
            Client TestClient = new Client("01");
            TimeSpan TimeBeforeAFK = new TimeSpan(0, 30, 0);

            //Expected
            bool ExpectedAFKStatus = false;

            // Act
            TestClient.CurrentPartition = new Partition();

            // Assert
            Assert.AreEqual(ExpectedAFKStatus, TestClient.IsAFK(TimeBeforeAFK));
        }

        [TestMethod]
        public void AFK_test2()
        {
            // Arrange
            Client TestClient = new Client("01");
            TimeSpan TimeBeforeAFK = new TimeSpan(0, 0, 0);

            //Expected
            bool ExpectedAFKStatus = true;

            // Act
            TestClient.CurrentPartition = new Partition();

            // Assert
            Assert.AreEqual(ExpectedAFKStatus, TestClient.IsAFK(TimeBeforeAFK));
        }
    }
}
