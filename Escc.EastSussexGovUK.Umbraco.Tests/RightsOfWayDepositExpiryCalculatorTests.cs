using System;
using NUnit.Framework;
using Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    [TestFixture]
    public class RightsOfWayDepositExpiryCalculatorTests
    {
        [Test]
        public void DepositsAreValidFor10YearsUpTo1October2013()
        {
            var calculator = new RightsOfWayDepositExpiryCalculator();
            var dateDeposited = new DateTime(2013, 09, 30);

            var expires = calculator.CalculateExpiry(dateDeposited);

            Assert.AreEqual(dateDeposited.AddYears(10), expires);
        }

        [Test]
        public void DepositsAreValidFor20YearsFrom1October2013()
        {
            var calculator = new RightsOfWayDepositExpiryCalculator();
            var dateDeposited = new DateTime(2013, 10, 2);

            var expires = calculator.CalculateExpiry(dateDeposited);

            Assert.AreEqual(dateDeposited.AddYears(20), expires);
        }
    }
}
