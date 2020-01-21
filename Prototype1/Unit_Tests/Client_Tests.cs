using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Central_Controller.Central_Controller;

namespace Unit_Tests
{
    [TestClass]
    public class User_Tests
    {
        [TestMethod]
        public void ConstructorTest1()
        {
            // Arrange
            User TestUser;

            //Expected
            string Expected_ID = "01";

            // Act
            TestUser = new User("01");

            // Assert
            Assert.AreEqual(Expected_ID, TestUser.ID);
        }

        [TestMethod]
        public void AFK_test1()
        {
            // Arrange
            User TestUser = new User("01");
            TimeSpan TimeBeforeAFK = new TimeSpan(0, 30, 0);

            //Expected
            bool ExpectedAFKStatus = false;

            // Act
            TestUser.CurrentPartition = new Partition();

            // Assert
            Assert.AreEqual(ExpectedAFKStatus, TestUser.IsAFK(TimeBeforeAFK));
        }

        [TestMethod]
        public void AFK_test2()
        {
            // Arrange
            User TestUser = new User("01");
            TimeSpan TimeBeforeAFK = new TimeSpan(0, 0, 0);

            //Expected
            bool ExpectedAFKStatus = true;

            // Act
            TestUser.CurrentPartition = new Partition();

            // Assert
            Assert.AreEqual(ExpectedAFKStatus, TestUser.IsAFK(TimeBeforeAFK));
        }
    }
}
