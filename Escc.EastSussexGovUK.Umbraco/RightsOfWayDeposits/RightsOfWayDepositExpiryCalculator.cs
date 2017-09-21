using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits
{
    /// <summary>
    /// A rights of way Section 31 deposit is valid for a set period from the date of deposit, but that date changes depending on the legislation in force at the time.
    /// </summary>
    public class RightsOfWayDepositExpiryCalculator
    {
        /// <summary>
        /// Calculates the expiry date based on the period set out in the legislation at the time of deposit.
        /// </summary>
        /// <param name="dateDeposited">The date deposited.</param>
        /// <returns>The date the deposit expires</returns>
        public DateTime CalculateExpiry(DateTime dateDeposited)
        {
            if (dateDeposited < new DateTime(2004, 2, 13))
            {
                return dateDeposited.AddYears(6);
            }
            else if (dateDeposited < new DateTime(2013, 10, 1))
            {
                return dateDeposited.AddYears(10);
            }
            else
            {
                return dateDeposited.AddYears(20);
            }
        }
    }
}