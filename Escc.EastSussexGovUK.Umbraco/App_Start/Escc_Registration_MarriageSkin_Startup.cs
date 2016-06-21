[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Escc.EastSussexGovUK.Umbraco.Escc_Registration_MarriageSkin_Startup), "PostStart")]

namespace Escc.EastSussexGovUK.Umbraco {

    /// <summary>
    /// Register the virtual path provider which makes available the embedded views from Escc.EastSussexGovUK.Mvc
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
    public static class Escc_Registration_MarriageSkin_Startup
	{
  		/// <summary>
		/// Wire up the provider at the end of the application startup process 
		/// </summary>
	    public static void PostStart() 
		{
            System.Web.Hosting.HostingEnvironment.RegisterVirtualPathProvider(new EmbeddedResourceVirtualPathProvider.Vpp(typeof(Escc.Registration.MarriageSkin.MarriageSkin).Assembly));
        }
    }
}