﻿using Escc.EastSussexGovUK.Umbraco.Services;
using System;
using System.Linq;
using Umbraco.Core.Models;
using Examine;
using Escc.AddressAndPersonalDetails;
using UmbracoExamine;
using Escc.EastSussexGovUK.Umbraco.Examine;
using System.Collections.Generic;
using Examine.LuceneEngine.SearchCriteria;
using Examine.SearchCriteria;
using Newtonsoft.Json;
using Umbraco.Web;

namespace Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits
{
    /// <summary>
    /// Gets data required for the listings page of rights of way Section 31 deposits from Examine data
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Services.IViewModelBuilder{Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits.RightsOfWayDepositsViewModel}" />
    public class RightsOfWayDepositsViewModelFromExamine : IViewModelBuilder<RightsOfWayDepositsViewModel>
    {
        private Uri _baseUrl;
        private int _umbracoParentNodeId;
        private string _searchTerm;
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
        /// <param name="searchTerm">The search term.</param>
        /// <param name="searchFilters">The search filters.</param>
        /// <exception cref="System.ArgumentNullException">baseUrl</exception>
        public RightsOfWayDepositsViewModelFromExamine(int umbracoParentNodeId, Uri baseUrl, string searchTerm, IEnumerable<ISearchFilter> searchFilters)
        {
            if (baseUrl == null) throw new ArgumentNullException(nameof(baseUrl));
            _umbracoParentNodeId = umbracoParentNodeId;
            _baseUrl = baseUrl;
            AddFilteredSearchTerm(searchTerm, searchFilters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RightsOfWayDepositsViewModelFromExamine" /> class.
        /// </summary>
        /// <param name="umbracoParentNodeId">The umbraco parent node identifier.</param>
        /// <param name="baseUrl">The base URL for linking to details of each deposit - expected to be the URL of the deposits listings page.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="searchFilters">The search filters.</param>
        /// <param name="currentPage">The current page in paged search results.</param>
        /// <param name="pageSize">Size of the page in paged search results.</param>
        /// <param name="sortOrder">The sort order applied to search results.</param>
        /// <exception cref="System.ArgumentNullException">baseUrl</exception>
        public RightsOfWayDepositsViewModelFromExamine(int umbracoParentNodeId, Uri baseUrl, string searchTerm, IEnumerable<ISearchFilter> searchFilters, int currentPage, int pageSize, RightsOfWayDepositsSortOrder sortOrder) : this(umbracoParentNodeId, baseUrl, searchTerm, searchFilters)
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

            var searcher = ExamineManager.Instance.SearchProviderCollection["RightsOfWayDepositsSearcher"];
            var criteria = searcher.CreateSearchCriteria(IndexTypes.Content);
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

            var results = searcher.Search(criteria);
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