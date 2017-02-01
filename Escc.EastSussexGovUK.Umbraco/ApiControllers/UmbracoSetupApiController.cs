using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Escc.EastSussexGovUK.Umbraco.CampaignTemplates;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.CouncilPlan;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.CustomerFocusBase;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features.Latest;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features.SocialMedia;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Features.WebChat;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.FormDownload;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Guide;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Landing;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LandingPageWithPictures;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.LegacyBase;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Location;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Map;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Person;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardDownloadPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardLandingPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.StandardTopicPage;
using Escc.EastSussexGovUK.Umbraco.DocumentTypes.Task;
using Escc.EastSussexGovUK.Umbraco.HomePage;
using Escc.EastSussexGovUK.Umbraco.Jobs;
using Escc.Umbraco.PropertyEditors.DataTypes;
using Escc.Umbraco.PropertyEditors.RichTextPropertyEditor;
using Escc.Umbraco.PropertyEditors.UkLocationPropertyEditor;
using Exceptionless;
using Umbraco.Inception.CodeFirst;
using Umbraco.Web.WebApi;

namespace Escc.EastSussexGovUK.Umbraco.ApiControllers
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
        /// Creates the supporting types (eg data types) needed for <see cref="CreateUmbracoDocumentTypes"/> and <see cref="CreateCampaignDocumentTypes"/> to succeed.
        /// </summary>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling"), SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [AcceptVerbs("POST")]
        public HttpResponseMessage CreateUmbracoSupportingTypes([FromUri] string token)
        {
            if (!CheckAuthorisationToken(token)) return Request.CreateResponse(HttpStatusCode.Forbidden);

            try
            {
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

                UmbracoCodeFirstInitializer.CreateDataType(typeof(TopicPageLayoutDataType));

                // Council Plan
                PriorityDataType.CreateDataType();

                // Campaign templates
                UmbracoCodeFirstInitializer.CreateDataType(typeof(ColourPickerDataType));
                UmbracoCodeFirstInitializer.CreateDataType(typeof(AlignmentDataType));
                UmbracoCodeFirstInitializer.CreateDataType(typeof(ShareStyleDataType));

                // Customer focus templates
                EmailAddressDataType.CreateDataType();
                PhoneNumberDataType.CreateDataType();
                LandingPageLayoutDataType.CreateLandingPageLayoutDataType();
                OpeningHoursDataType.CreateDataType();
                UkLocationDataType.CreateDataType(showEastingNorthing: false);

                // For recycling site document type
                ResponsibleAuthorityDataType.CreateDataType();
                UmbracoCodeFirstInitializer.CreateDataType(typeof(WasteTypesDataType));

                // Jobs document types
                UmbracoCodeFirstInitializer.CreateDataType(typeof(PublicOrRedeploymentDataType));

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
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(CustomerFocusBaseDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(LandingDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(TaskDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(LocationDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(GuideStepDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(GuideDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(RecyclingSiteDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(LibraryDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(MobileLibraryStopDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(ChildcareDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(CouncilOfficeDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(SportLocationDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(ParkDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(RegistrationOfficeDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(DayCentreDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(PersonDocumentType));

                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Creates the Umbraco document types for the campaign templates.
        /// </summary>
        /// <remarks>Having separate methods reduces the risk of timeouts when running this code on Azure</remarks>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [AcceptVerbs("POST")]
        public HttpResponseMessage CreateCampaignDocumentTypes([FromUri] string token)
        {
            if (!CheckAuthorisationToken(token)) return Request.CreateResponse(HttpStatusCode.Forbidden);

            try
            {
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(CampaignLandingDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(CampaignContentDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(CampaignTilesDocumentType));

                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// Creates the Umbraco document types for the jobs templates.
        /// </summary>
        /// <remarks>Having separate methods reduces the risk of timeouts when running this code on Azure</remarks>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [AcceptVerbs("POST")]
        public HttpResponseMessage CreateJobsDocumentTypes([FromUri] string token)
        {
            if (!CheckAuthorisationToken(token)) return Request.CreateResponse(HttpStatusCode.Forbidden);

            try
            {
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(JobsHomeDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(JobSearchResultsDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(JobsComponentDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(JobsRssDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(JobsSearchDocumentType));
                UmbracoCodeFirstInitializer.CreateOrUpdateEntity(typeof(JobAdvertDocumentType));

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
