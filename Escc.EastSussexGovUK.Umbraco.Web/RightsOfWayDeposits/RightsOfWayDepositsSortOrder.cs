using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.RightsOfWayDeposits
{
    /// <summary>
    /// Possible sort orders which can be applied when viewing a list of rights of way Section 31 deposits
    /// </summary>
    public enum RightsOfWayDepositsSortOrder
    {
        DateDepositedAscending,
        DateDepositedDescending,
        ReferenceAscending,
        ReferenceDescending,
        ParishAscending,
        ParishDescending,
        DateExpiresAscending,
        DateExpiresDescending
    }
}