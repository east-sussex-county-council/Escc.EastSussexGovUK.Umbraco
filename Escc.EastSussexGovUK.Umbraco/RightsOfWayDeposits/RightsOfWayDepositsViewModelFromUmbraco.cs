using Escc.EastSussexGovUK.Umbraco.Services;
using System;
using System.Linq;
using Umbraco.Core.Models;
using Examine;
using Escc.AddressAndPersonalDetails;
using UmbracoExamine;

namespace Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits
{
    /// <summary>
    /// Gets data required for the listings page of rights of way Section 31 deposits from Umbraco properties and Examine data
    /// </summary>
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Services.BaseViewModelFromUmbracoBuilder" />
    /// <seealso cref="Escc.EastSussexGovUK.Umbraco.Services.IViewModelBuilder{Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits.RightsOfWayDepositsViewModel}" />
    public class RightsOfWayDepositsViewModelFromUmbraco : BaseViewModelFromUmbracoBuilder, IViewModelBuilder<RightsOfWayDepositsViewModel>
    {
        private Uri _baseUrl;
        private int _currentPage;
        private int _pageSize;
        private RightsOfWayDepositsSortOrder _sortOrder;

        /// <summary>
        /// Initializes a new instance of the <see cref="RightsOfWayDepositsViewModelFromUmbraco"/> class.
        /// </summary>
        /// <param name="umbracoContent">Content of the umbraco.</param>
        /// <param name="baseUrl">The base URL for linking to details of each deposit - expected to be the URL of the deposits listings page.</param>
        /// <param name="currentPage">The current page in paged search results.</param>
        /// <param name="pageSize">Size of the page in paged search results.</param>
        /// <param name="sortOrder">The sort order applied to search results.</param>
        /// <exception cref="System.ArgumentNullException">baseUrl</exception>
        public RightsOfWayDepositsViewModelFromUmbraco(IPublishedContent umbracoContent, Uri baseUrl, int currentPage, int pageSize, RightsOfWayDepositsSortOrder sortOrder) : base(umbracoContent, null, null)
        {
            if (baseUrl == null) throw new ArgumentNullException(nameof(baseUrl));
            _baseUrl = baseUrl;
            _currentPage = currentPage;
            _pageSize = pageSize;
            _sortOrder = sortOrder;
        }

        public RightsOfWayDepositsViewModel BuildModel()
        {
            var model = new RightsOfWayDepositsViewModel();

            var searcher = ExamineManager.Instance.SearchProviderCollection["RightsOfWayDepositsSearcher"];
            var criteria = searcher.CreateSearchCriteria(IndexTypes.Content);
            criteria.NodeTypeAlias("RightsOfWayDeposit");
            criteria.ParentId(UmbracoContent.Id);

            switch (_sortOrder)
            {
                case RightsOfWayDepositsSortOrder.ReferenceAscending:
                    criteria.OrderBy("nodeName");
                    break;
                case RightsOfWayDepositsSortOrder.ReferenceDescending:
                    criteria.OrderByDescending("nodeName");
                    break;
                case RightsOfWayDepositsSortOrder.OwnerAscending:
                    criteria.OrderBy("FamilyName_Content");
                    break;
                case RightsOfWayDepositsSortOrder.OwnerDescending:
                    criteria.OrderByDescending("FamilyName_Content");
                    break;
                case RightsOfWayDepositsSortOrder.ParishAscending:
                    criteria.OrderBy("Parish_Content");
                    break;
                case RightsOfWayDepositsSortOrder.ParishDecending:
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

            var results = searcher.Search(criteria);
            model.TotalDeposits = results.Count(); 
            var pagedResults = results.Skip((_currentPage - 1) * _pageSize).Take(_pageSize);

            foreach (var result in pagedResults)
            {
                var deposit = new RightsOfWayDepositViewModel();
                deposit.Reference = result.Fields["nodeName"];
                deposit.DepositUrl = new Uri(_baseUrl, result.Fields["urlName"]);
                deposit.Owner = new PersonName();
                if (result.Fields.Keys.Contains("HonorificTitle_Content")) deposit.Owner.Titles.Add(result.Fields["HonorificTitle_Content"]);
                deposit.Owner.GivenNames.Add(result.Fields["GivenName_Content"]);
                deposit.Owner.FamilyName = result.Fields["FamilyName_Content"];
                if (result.Fields.Keys.Contains("HonorificSuffix_Content")) deposit.Owner.Suffixes.Add(result.Fields["HonorificSuffix_Content"]);
                deposit.Parish = result.Fields["Parish_Content"];
                if (result.Fields.Keys.Contains("DateDeposited_Content"))
                {
                    deposit.DateDeposited = new DateTime(Int32.Parse(result.Fields["DateDeposited_Content"].Substring(0,4)), Int32.Parse(result.Fields["DateDeposited_Content"].Substring(4,2)), Int32.Parse(result.Fields["DateDeposited_Content"].Substring(6,2)));
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