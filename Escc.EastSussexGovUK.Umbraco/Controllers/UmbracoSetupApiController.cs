using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.CampaignTemplates;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.CouncilPlan;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.FormDownload;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.HomePage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LandingPageWithPictures;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LegacyBase;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Map;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardDownloadPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardLandingPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardTopicPage;
using Escc.EastSussexGovUK.UmbracoDocumentTypes.Css;
using Escc.EastSussexGovUK.UmbracoDocumentTypes.DataTypes;
using Escc.EastSussexGovUK.UmbracoDocumentTypes.Features.Latest;
using Escc.EastSussexGovUK.UmbracoDocumentTypes.Features.SocialMedia;
using Escc.EastSussexGovUK.UmbracoDocumentTypes.Features.WebChat;
using Escc.EastSussexGovUK.UmbracoDocumentTypes.RichTextPropertyEditor;
using Escc.Umbraco.PropertyEditors.Stylesheets;
using Exceptionless;
using ExCSS;
using Umbraco.Inception.CodeFirst;
using Umbraco.Web.WebApi;

namespace Escc.EastSussexGovUK.Umbraco.Controllers
{
    /// <summary>
    /// Create Umbraco document types needed for this project
    /// </summary>
    public class UmbracoSetupApiController : UmbracoApiController
    {
        /// <summary>
        /// Checks the authorisation token passed with the request is valid, so that this method cannot be called without knowing the token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        private static bool CheckAuthorisationToken(string token)
        {
            return token == ConfigurationManager.AppSettings["Escc.Umbraco.Inception.AuthToken"];
        } 
        
        /// <summary>
        /// Creates the supporting types (eg data types) needed for <see cref="CreateUmbracoDocumentTypes"/> to succeed.
        /// </summary>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling"), SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [AcceptVerbs("POST")]
        public HttpResponseMessage CreateUmbracoSupportingTypes([FromUri] string token)
        {
            if (!CheckAuthorisationToken(token)) return Request.CreateResponse(HttpStatusCode.Forbidden);

            try
            {
                // Create stylesheets for properties using rich text editor
                TinyMceStylesheets.CreateStylesheets(new StylesheetService(), new Parser());

                // Insert data types before the document types that use them, otherwise the relevant property is not created
                CheckboxDataType.CreateCheckboxDataType();
                CacheDataType.CreateCacheDataType();
                SocialMediaOrderDataType.CreateSocialMediaOrderDataType();
                FacebookWidgetSettingsDataType.CreateFacebookWidgetSettingsDataType();
                FacebookUrlDataType.CreateDataType();
                UmbracoCodeFirstInitializer.CreateDataType(typeof(ShowWidgetDataType));
                TwitterScriptDataType.CreateDataType();
                MultiNodeTreePickerDataType.CreateDataType();
                UrlDataType.CreateDataType();
            
                RichTextEsccStandardDataType.CreateDataType();
                RichTextEsccWithFormattingDataType.CreateDataType();
                RichTextAuthorNotesDataType.CreateDataType();
                RichTextSingleLinkDataType.CreateDataType();
                RichTextLinksListDataType.CreateDataType();
                RichTextTwoListsOfLinksDataType.CreateDataType();
                LatestDataType.CreateDataType();

                LandingPageColumnsDataType.CreateDataType();
                LandingPageDescriptionsDataType.CreateDataType();

                TopicPageLayoutDataType.CreateDataType();

                // Council Plan
                PriorityDataType.CreateDataType();

                // Campaign templates
                UmbracoCodeFirstInitializer.CreateDataType(typeof(QuoteColourDataType));

                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// Creates the Umbraco document types defined by this project.
        /// </summary>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [AcceptVerbs("POST")]
        public HttpResponseMessage CreateUmbracoDocumentTypes([FromUri] string token)
        {
            if (!CheckAuthorisationToken(token)) return Request.CreateResponse(HttpStatusCode.Forbidden);

            try
            {
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(HomePageItemDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(HomePageItemsDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(HomePageDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(WebChatDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(LegacyBaseDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(LandingPageWithPicturesDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(StandardLandingPageDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(StandardTopicPageDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(StandardDownloadPageDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(MapDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(FormDownloadDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(CouncilPlanHomePageDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(CouncilPlanBudgetPageDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(CouncilPlanMonitoringPageDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(CouncilPlanPrioritiesPageDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(CouncilPlanTopicPageDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(CampaignLandingDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(CampaignContentDocumentType));

                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
