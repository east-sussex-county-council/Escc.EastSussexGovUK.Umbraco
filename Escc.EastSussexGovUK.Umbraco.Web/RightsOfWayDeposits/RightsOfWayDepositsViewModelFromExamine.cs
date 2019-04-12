using Escc.EastSussexGovUK.Umbraco.Web.Services;
using System;
using System.Linq;
using Umbraco.Core.Models;
using Examine;
using Escc.AddressAndPersonalDetails;
using UmbracoExamine;
using System.Collections.Generic;
using Examine.LuceneEngine.SearchCriteria;
using Examine.SearchCriteria;
using Newtonsoft.Json;
using Umbraco.Web;
using Escc.Umbraco.PropertyEditors.PersonNamePropertyEditor;
using System.Globalization;
using Escc.Umbraco.PropertyEditors.UkLocationPropertyEditor;
using Escc.Umbraco.PropertyTypes;
using Escc.EastSussexGovUK.Umbraco.Examine;

namespace Escc.EastSussexGovUK.Umbraco.Web.RightsOfWayDeposits
{
    /// <summary>
    /// Gets data required for the listings page of rights of way Section 31 deposits from Examine data
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Services.IViewModelBuilder{Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits.RightsOfWayDepositsViewModel}" />
    public class RightsOfWayDepositsViewModelFromExamine : IViewModelBuilder<RightsOfWayDepositsViewModel>
    {
        private Uri _baseUrl;
        private readonly UmbracoHelper _umbracoHelper;
        private int _umbracoParentNodeId;
        private string _searchTerm;
        private readonly ISearcher _searcher;
        private int _currentPage;
        private int _pageSize;
        private RightsOfWayDepositsSortOrder _sortOrder;
        private bool _pageResults = false;
        private bool _sortResults = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="RightsOfWayDepositsViewModelFromExamine" /> class.
        /// </summary>
        /// <param name="umbracoParentNodeId">The umbraco parent node identifier.</param>
        /// <param name="baseUrl">The base URL for linking to details of each deposit - expected to be the URL of the deposits listings page.</param>
        /// <param name="searcher">The Examine searcher for rights of way deposits</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="searchFilters">Search filters which modify the search term.</param>
        /// <param name="umbracoHelper">The Umbraco helper.</param>
        /// <exception cref="ArgumentNullException">baseUrl</exception>
        /// <exception cref="System.ArgumentNullException">baseUrl</exception>
        public RightsOfWayDepositsViewModelFromExamine(int umbracoParentNodeId, Uri baseUrl, ISearcher searcher, string searchTerm, IEnumerable<ISearchFilter> searchFilters, UmbracoHelper umbracoHelper)
        {
            if (baseUrl == null) throw new ArgumentNullException(nameof(baseUrl));
            _searcher = searcher ?? throw new ArgumentNullException(nameof(searcher));
            _umbracoParentNodeId = umbracoParentNodeId;
            _baseUrl = baseUrl;
            _umbracoHelper = umbracoHelper;
            AddFilteredSearchTerm(searchTerm, searchFilters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RightsOfWayDepositsViewModelFromExamine" /> class.
        /// </summary>
        /// <param name="umbracoParentNodeId">The umbraco parent node identifier.</param>
        /// <param name="baseUrl">The base URL for linking to details of each deposit - expected to be the URL of the deposits listings page.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="searcher">The Examine searcher for rights of way deposits</param>
        /// <param name="searchFilters">The search filters.</param>
        /// <param name="currentPage">The current page in paged search results.</param>
        /// <param name="pageSize">Size of the page in paged search results.</param>
        /// <param name="sortOrder">The sort order applied to search results.</param>
        /// <exception cref="System.ArgumentNullException">baseUrl</exception>
        public RightsOfWayDepositsViewModelFromExamine(int umbracoParentNodeId, Uri baseUrl, ISearcher searcher, string searchTerm, IEnumerable<ISearchFilter> searchFilters, int currentPage, int pageSize, RightsOfWayDepositsSortOrder sortOrder) : this(umbracoParentNodeId, baseUrl, searcher, searchTerm, searchFilters, null)
        {
            _currentPage = currentPage;
            _pageSize = pageSize;
            _sortOrder = sortOrder;
            _pageResults = true;
            _sortResults = true;
        }

        private void AddFilteredSearchTerm(string searchTerm, IEnumerable<ISearchFilter> searchFilters)
        {
            _searchTerm = searchTerm;

            if (searchFilters != null)
            {
                foreach (var filter in searchFilters)
                {
                    _searchTerm = filter.Filter(_searchTerm);
                }
            }
        }

        public RightsOfWayDepositsViewModel BuildModel()
        {
            var model = new RightsOfWayDepositsViewModel();

            var criteria = _searcher.CreateSearchCriteria(IndexTypes.Content);
            criteria.NodeTypeAlias("RightsOfWayDeposit");
            criteria.ParentId(_umbracoParentNodeId);
            if (!String.IsNullOrEmpty(_searchTerm))
            {
                criteria.Field("RightsOfWayDepositSearch", _searchTerm);
            }

            if (_sortResults)
            {
                switch (_sortOrder)
                {
                    case RightsOfWayDepositsSortOrder.ReferenceAscending:
                        criteria.OrderBy("nodeName");
                        break;
                    case RightsOfWayDepositsSortOrder.ReferenceDescending:
                        criteria.OrderByDescending("nodeName");
                        break;
                    case RightsOfWayDepositsSortOrder.ParishAscending:
                        criteria.OrderBy("Parish_Content");
                        break;
                    case RightsOfWayDepositsSortOrder.ParishDescending:
                        criteria.OrderByDescending("Parish_Content");
                        break;
                    case RightsOfWayDepositsSortOrder.DateDepositedAscending:
                        criteria.OrderBy("DateDeposited_Content");
                        break;
                    case RightsOfWayDepositsSortOrder.DateExpiresAscending:
                        criteria.OrderBy("DateExpires_Content");
                        break;
                    case RightsOfWayDepositsSortOrder.DateExpiresDescending:
                        criteria.OrderByDescending("DateExpires_Content");
                        break;
                    default:
                        criteria.OrderByDescending("DateDeposited_Content");
                        break;
                }
            }

            var results = _searcher.Search(criteria);
            model.TotalDeposits = results.Count();
            IEnumerable<SearchResult> selectedResults;
            if (_pageResults)
            {
                selectedResults = results.Skip((_currentPage - 1) * _pageSize).Take(_pageSize);
            }
            else
            {
                selectedResults = results;
            }

            foreach (var result in selectedResults)
            {
                var deposit = new RightsOfWayDepositViewModel();
                deposit.Reference = result.Fields["nodeName"];
                deposit.PageUrl = new Uri(_baseUrl, result.Fields["urlName"]);

                if (result.Fields.Keys.Contains("DepositDocument_Content") &&_umbracoHelper != null)
                {
                    var nodeIds = result.Fields["DepositDocument_Content"].Split(',');
                    var multiMediaPicker = Enumerable.Empty<IPublishedContent>();
                    if (nodeIds.Length > 0)
                    {
                        multiMediaPicker = _umbracoHelper.TypedMedia(nodeIds).Where(x => x != null);
                    }

                    foreach (var media in multiMediaPicker) { 
                        deposit.DepositDocuments.Add(new HtmlLink() { Text = media.Name, Url = new Uri(_baseUrl, media.Url) });
                    }
                }

                var nameConverter = new PersonNamePropertyValueConverter();
                var addressConverter = new UkLocationPropertyValueConverter();
                for (var i = 1; i <= 5; i++)
                {
                    if (result.Fields.Keys.Contains($"Owner{i}_Content"))
                    {
                        var owner = nameConverter.ConvertDataToSource(null, result.Fields[$"Owner{i}_Content"], false) as PersonName;
                        if (owner != null) { deposit.IndividualOwners.Add(owner); }
                    }

                    if (result.Fields.Keys.Contains($"OrganisationalOwner{i}_Content"))
                    {
                        var org = result.Fields[$"OrganisationalOwner{i}_Content"];
                        if (!String.IsNullOrEmpty(org)) { deposit.OrganisationalOwners.Add(org); }
                    }

                    if (result.Fields.Keys.Contains($"Location{((i > 1) ? i.ToString(CultureInfo.InvariantCulture) : String.Empty)}_Content"))
                    {
                        var addressInfo = addressConverter.ConvertDataToSource(null, result.Fields[$"Location{((i > 1) ? i.ToString(CultureInfo.InvariantCulture) : String.Empty)}_Content"], false) as AddressInfo;
                        if (addressInfo != null && addressInfo.BS7666Address.HasAddress() && addressInfo.BS7666Address.ToString() != addressInfo.BS7666Address.AdministrativeArea)
                        {
                            if (addressInfo.GeoCoordinate.Latitude == 0 && addressInfo.GeoCoordinate.Longitude == 0) addressInfo.GeoCoordinate = null;
                            deposit.Addresses.Add(addressInfo);
                        }
                    }
                }

                if (result.Fields.Keys.Contains("Parish_Content"))
                {
                    var parishData = result.Fields["Parish_Content"];
                    if (!String.IsNullOrEmpty(parishData))
                    {
                        var parishes = parishData.Split(',');
                        foreach (var parish in parishes)
                        {
                            deposit.Parishes.Add(parish);
                        }
                    }
                }

                if (result.Fields.Keys.Contains("GridReference_Content"))
                {
                    deposit.OrdnanceSurveyGridReference = result.Fields["GridReference_Content"];
                }

                if (result.Fields.Keys.Contains("pageDescription_Content"))
                {
                    deposit.Description = result.Fields["pageDescription_Content"];
                }

                if (result.Fields.Keys.Contains("DateDeposited_Content"))
                {
                    deposit.DateDeposited = new DateTime(Int32.Parse(result.Fields["DateDeposited_Content"].Substring(0, 4)), Int32.Parse(result.Fields["DateDeposited_Content"].Substring(4, 2)), Int32.Parse(result.Fields["DateDeposited_Content"].Substring(6, 2)));
                }
                if (result.Fields.Keys.Contains("DateExpires_Content"))
                {
                    deposit.DateExpires = new DateTime(Int32.Parse(result.Fields["DateExpires_Content"].Substring(0, 4)), Int32.Parse(result.Fields["DateExpires_Content"].Substring(4, 2)), Int32.Parse(result.Fields["DateExpires_Content"].Substring(6, 2)));
                }
                model.Deposits.Add(deposit);
            }

            return model;
        }
    }
}