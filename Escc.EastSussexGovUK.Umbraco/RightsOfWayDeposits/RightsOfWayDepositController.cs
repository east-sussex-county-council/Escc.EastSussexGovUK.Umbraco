using System;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Umbraco.Services;
using Escc.Umbraco.ContentExperiments;
using Escc.Umbraco.PropertyTypes;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Escc.Umbraco.Caching;
using Humanizer;
using System.Linq;
using Examine;
using Escc.Umbraco.Expiry;

namespace Escc.EastSussexGovUK.Umbraco.RightsOfWayDeposits
{
    /// <summary>
    /// Controller for pages based on the 'Rights of way Section 31 deposit' Umbraco document type
    /// </summary>
    /// <seealso cref="Umbraco.Web.Mvc.RenderMvcController" />
    public class RightsOfWayDepositController : RenderMvcController
    {
        /// <summary>
        /// The default action to render the front-end view
        /// </summary>
        /// <param name="model"/>
        /// <returns/>
        public override ActionResult Index(RenderModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var viewModel = new RightsOfWayDepositViewModelFromUmbraco(model.Content).BuildModel();

            var where = "in " + viewModel.Parishes.Humanize() + " parish(es)";
            if (viewModel.Addresses.Any())
            {
                where = "at " + viewModel.Addresses[0].BS7666Address.ToString();
                if (viewModel.Addresses.Count > 1) where += " and other addresses";
            }
            viewModel.Metadata.Title = "Rights of way deposit: " + viewModel.Reference;
            viewModel.Metadata.Description = "A declaration of rights of way over land " + where + " deposited with East Sussex County Council under Section 31 (6) of the Highways Act 1980";

            // Add common properties to the model
            var expiryDate = new ExpiryDateFromExamine(model.Content.Id, ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"]);
            var modelBuilder = new BaseViewModelBuilder();
            modelBuilder.PopulateBaseViewModel(viewModel, model.Content, new ContentExperimentSettingsService(),
                expiryDate.ExpiryDate,
                UmbracoContext.Current.InPreviewMode);
            modelBuilder.PopulateBaseViewModelWithInheritedContent(viewModel, 
                new UmbracoLatestService(model.Content), 
                new UmbracoSocialMediaService(model.Content),
                null,
                new UmbracoWebChatSettingsService(model.Content, new UrlListReader()),
                null);


            new HttpCachingService().SetHttpCacheHeadersFromUmbracoContent(model.Content, UmbracoContext.Current.InPreviewMode, Response.Cache, new[] { expiryDate });

            return CurrentTemplate(viewModel);
        }
    }
}