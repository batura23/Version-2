using Microsoft.Analytics.Interfaces;
using Microsoft.Analytics.Interfaces.Streaming;
using Microsoft.Analytics.Types.Sql;
using Microsoft.Analytics.UnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WpfApp3.Classes;

namespace USqlCSharpUdoUnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestValidInput()
        {
            TicketPriceCalculator calculator = new TicketPriceCalculator();
            double totalPrice = calculator.CalculateTotalPrice(TicketPriceCalculator.Show.LittleRedRidingHood,
                TicketPriceCalculator.Zone.VIP, 5);
            Assert.AreEqual(7500, totalPrice);
        }

        [TestMethod]
        public void TestLargeInput()
        {
            TicketPriceCalculator calculator = new TicketPriceCalculator();
            int maxTicketCount = 1000;
            double basePrice = 1500; 
            double orchestraIncrease = 1.07;
            double discount = 1 - 0.25;
            double expectedPrice = maxTicketCount * basePrice * orchestraIncrease * discount;
            double totalPrice = calculator.CalculateTotalPrice(TicketPriceCalculator.Show.SwanLake,
                TicketPriceCalculator.Zone.Orchestra, maxTicketCount, maxTicketCount);
            Assert.AreEqual(expectedPrice, totalPrice);
        }
    }
}
