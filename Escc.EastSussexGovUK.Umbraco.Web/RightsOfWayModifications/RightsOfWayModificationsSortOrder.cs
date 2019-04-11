using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.EastSussexGovUK.Umbraco.Web.RightsOfWayModifications
{
    /// <summary>
    /// Possible sort orders which can be applied when viewing a list of rights of way definitive map modification order applications
    /// </summary>
    public enum RightsOfWayModificationsSortOrder
    {
        DateReceivedAscending,
        DateReceivedDescending,
        ReferenceAscending,
        ReferenceDescending,
        ParishAscending,
        ParishDescending,
        StatusAscending,
        StatusDescending
    }
}