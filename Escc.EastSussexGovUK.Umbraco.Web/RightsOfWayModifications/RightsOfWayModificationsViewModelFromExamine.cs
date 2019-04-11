﻿using Escc.EastSussexGovUK.Umbraco.Web.Services;
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

namespace Escc.EastSussexGovUK.Umbraco.Web.RightsOfWayModifications
{
    /// <summary>
    /// Gets data required for the listings page of rights of way definitive map modification order applications from Examine data
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Services.IViewModelBuilder{Escc.EastSussexGovUK.Umbraco.RightsOfWayModifications.RightsOfWayModificationsViewModel}" />
    public class RightsOfWayModificationsViewModelFromExamine : IViewModelBuilder<RightsOfWayModificationsViewModel>
    {
        private Uri _baseUrl;
        private readonly ISearcher _searcher;
        private readonly UmbracoHelper _umbracoHelper;
        private int _umbracoParentNodeId;
        private string _searchTerm;
        private int _currentPage;
        private int _pageSize;
        private RightsOfWayModificationsSortOrder _sortOrder;
        private bool _pageResults = false;
        private bool _sortResults = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="RightsOfWayModificationsViewModelFromExamine" /> class.
        /// </summary>
        /// <param name="umbracoParentNodeId">The umbraco parent node identifier.</param>
        /// <param name="baseUrl">The base URL for linking to details of each modification order - expected to be the URL of the modification orders listings page.</param>
        /// <param name="searcher">The Examine searcher for rights of way deposits</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="searchFilters">The search filters.</param>
        /// <param name="umbracoHelper">The Umbraco helper.</param>
        /// <exception cref="ArgumentNullException">baseUrl</exception>
        /// <exception cref="System.ArgumentNullException">baseUrl</exception>
        public RightsOfWayModificationsViewModelFromExamine(int umbracoParentNodeId, Uri baseUrl, ISearcher searcher, string searchTerm, IEnumerable<ISearchFilter> searchFilters, UmbracoHelper umbracoHelper)
        {
            if (baseUrl == null) throw new ArgumentNullException(nameof(baseUrl));
            _umbracoParentNodeId = umbracoParentNodeId;
            _baseUrl = baseUrl;
            _searcher = searcher ?? throw new ArgumentNullException(nameof(searcher));
            _umbracoHelper = umbracoHelper;
            AddFilteredSearchTerm(searchTerm, searchFilters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RightsOfWayModificationsViewModelFromExamine" /> class.
        /// </summary>
        /// <param name="umbracoParentNodeId">The umbraco parent node identifier.</param>
        /// <param name="baseUrl">The base URL for linking to details of each modification order - expected to be the URL of the modification orders listings page.</param>
        /// <param name="searcher">The Examine searcher for rights of way deposits</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="searchFilters">The search filters.</param>
        /// <param name="currentPage">The current page in paged search results.</param>
        /// <param name="pageSize">Size of the page in paged search results.</param>
        /// <param name="sortOrder">The sort order applied to search results.</param>
        /// <exception cref="System.ArgumentNullException">baseUrl</exception>
        public RightsOfWayModificationsViewModelFromExamine(int umbracoParentNodeId, Uri baseUrl, ISearcher searcher, string searchTerm, IEnumerable<ISearchFilter> searchFilters, int currentPage, int pageSize, RightsOfWayModificationsSortOrder sortOrder) : this(umbracoParentNodeId, baseUrl, searcher, searchTerm, searchFilters, null)
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

        public RightsOfWayModificationsViewModel BuildModel()
        {
            var model = new RightsOfWayModificationsViewModel();

            var criteria = _searcher.CreateSearchCriteria(IndexTypes.Content);
            criteria.NodeTypeAlias("RightsOfWayModification");
            criteria.ParentId(_umbracoParentNodeId);
            if (!String.IsNullOrEmpty(_searchTerm))
            {
                criteria.Field("RightsOfWayModificationSearch", _searchTerm);
            }

            if (_sortResults)
            {
                switch (_sortOrder)
                {
                    case RightsOfWayModificationsSortOrder.ReferenceAscending:
                        criteria.OrderBy("nodeName");
                        break;
                    case RightsOfWayModificationsSortOrder.ReferenceDescending:
                        criteria.OrderByDescending("nodeName");
                        break;
                    case RightsOfWayModificationsSortOrder.ParishAscending:
                        criteria.OrderBy("Parish");
                        break;
                    case RightsOfWayModificationsSortOrder.ParishDescending:
                        criteria.OrderByDescending("Parish");
                        break;
                    case RightsOfWayModificationsSortOrder.DateReceivedAscending:
                        criteria.OrderBy("DateReceived");
                        break;
                    case RightsOfWayModificationsSortOrder.StatusAscending:
                        criteria.OrderBy("Status");
                        break;
                    case RightsOfWayModificationsSortOrder.StatusDescending:
                        criteria.OrderByDescending("Status");
                        break;
                    default:
                        criteria.OrderByDescending("DateReceived");
                        break;
                }
            }

            var results = _searcher.Search(criteria);
            model.TotalModificationOrderApplications = results.Count();
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
                var modificationOrder = new RightsOfWayModificationViewModel();
                modificationOrder.Reference = result.Fields["nodeName"];
                modificationOrder.PageUrl = new Uri(_baseUrl, result.Fields["urlName"]);

                if (result.Fields.Keys.Contains("Documents") &&_umbracoHelper != null)
                {
                    var nodeIds = result.Fields["Documents"].Split(',');
                    var multiMediaPicker = Enumerable.Empty<IPublishedContent>();
                    if (nodeIds.Length > 0)
                    {
                        multiMediaPicker = _umbracoHelper.TypedMedia(nodeIds).Where(x => x != null);
                    }

                    foreach (var media in multiMediaPicker) { 
                        modificationOrder.ApplicationDocuments.Add(new HtmlLink() { Text = media.Name, Url = new Uri(_baseUrl, media.Url) });
                    }
                }

                var nameConverter = new PersonNamePropertyValueConverter();
                var addressConverter = new UkLocationPropertyValueConverter();
                for (var i = 1; i <= 5; i++)
                {
                    if (result.Fields.Keys.Contains($"Owner{i}"))
                    {
                        var owner = nameConverter.ConvertDataToSource(null, result.Fields[$"Owner{i}"], false) as PersonName;
                        if (owner != null) { modificationOrder.IndividualOwners.Add(owner); }
                    }

                    if (result.Fields.Keys.Contains($"OrganisationalOwner{i}"))
                    {
                        var org = result.Fields[$"OrganisationalOwner{i}"];
                        if (!String.IsNullOrEmpty(org)) { modificationOrder.OrganisationalOwners.Add(org); }
                    }

                    if (result.Fields.Keys.Contains($"Location{i}"))
                    {
                        var addressInfo = addressConverter.ConvertDataToSource(null, result.Fields[$"Location{i}"], false) as AddressInfo;
                        if (addressInfo != null && addressInfo.BS7666Address.HasAddress() && addressInfo.BS7666Address.ToString() != addressInfo.BS7666Address.AdministrativeArea)
                        {
                            if (addressInfo.GeoCoordinate.Latitude == 0 && addressInfo.GeoCoordinate.Longitude == 0) addressInfo.GeoCoordinate = null;
                            modificationOrder.Addresses.Add(addressInfo);
                        }
                    }
                }

                if (result.Fields.Keys.Contains("Parish"))
                {
                    var parishData = result.Fields["Parish"];
                    if (!String.IsNullOrEmpty(parishData))
                    {
                        var parishes = parishData.Split(',');
                        foreach (var parish in parishes)
                        {
                            modificationOrder.Parishes.Add(parish);
                        }
                    }
                }

                if (result.Fields.Keys.Contains("GridReference"))
                {
                    modificationOrder.OrdnanceSurveyGridReference = result.Fields["GridReference"];
                }

                if (result.Fields.Keys.Contains("pageDescription"))
                {
                    modificationOrder.DescriptionOfRoute = result.Fields["pageDescription"];
                }

                if (result.Fields.Keys.Contains("DateReceived"))
                {
                    modificationOrder.DateReceived = new DateTime(Int32.Parse(result.Fields["DateReceived"].Substring(0, 4)), Int32.Parse(result.Fields["DateReceived"].Substring(4, 2)), Int32.Parse(result.Fields["DateReceived"].Substring(6, 2)));
                }
                if (result.Fields.Keys.Contains("DateDetermined"))
                {
                    modificationOrder.DateDetermined = new DateTime(Int32.Parse(result.Fields["DateDetermined"].Substring(0, 4)), Int32.Parse(result.Fields["DateDetermined"].Substring(4, 2)), Int32.Parse(result.Fields["DateDetermined"].Substring(6, 2)));
                }
                model.ModificationOrderApplications.Add(modificationOrder);
            }

            return model;
        }
    }
}