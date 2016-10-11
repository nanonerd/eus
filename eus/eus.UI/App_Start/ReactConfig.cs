using React;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(eus.UI.ReactConfig), "Configure")]

namespace eus.UI
{
	public static class ReactConfig
	{
		public static void Configure()
		{
			// If you want to use server-side rendering of React components, 
			// add all the necessary JavaScript files here. This includes 
			// your components as well as all of their dependencies.
			// See http://reactjs.net/ for more information. Example:
            ReactSiteConfiguration.Configuration
                .AddScript("~/Scripts/internal/eus/api.js")              // API calls
                .AddScript("~/Scripts/internal/eus/common.js")           // Common JS functions
                .AddScript("~/Scripts/internal/vote/topicAnswers.jsx")   // React topic page
                .AddScript("~/Scripts/internal/vote/comments.jsx");	    // React comments page	                                  
			
			// If you use an external build too (for example, Babel, Webpack,
			// Browserify or Gulp), you can improve performance by disabling 
			// ReactJS.NET's version of Babel and loading the pre-transpiled 
			// scripts. Example:
			//ReactSiteConfiguration.Configuration
			//	.SetLoadBabel(false)
			//	.AddScriptWithoutTransform("~/Scripts/bundle.server.js")
		}
	}
}