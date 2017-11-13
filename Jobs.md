# Jobs

The job search section of our website is powered by [Examine](https://github.com/shazwazza/Examine/), which is based on [Lucene](https://lucene.apache.org/).

## Importing the data

The first step is to get the data from our external jobs provider into Examine. This happens by configuring an index set called `PublicJobsIndexSet` in `~\config\ExamineIndex.config`, and an index provider called `PublicJobsIndexer` in `~\config\ExamineSettings.config`. 

The `PublicJobsIndexer` index provider configuration points to the `PublicJobsIndexer` class, which is instantiated by Umbraco to build or rebuild the index. It fetches the data from our external jobs provider (which is just an instance of `IJobsDataProvider`), using source URLs that come from `TalentLinkPublicJobs*Url` settings in the `appSettings` section of `web.config`, and puts it into Examine.

We then have a second index set called `PublicJobsLookupValuesIndexSet` and an index provider called `PublicJobsLookupValuesIndexer` with a matching class. This reads lookup values from our external jobs provider, such as the list of towns where jobs can be based, but also searches the `PublicJobsIndexSet` for each one and records how many results it finds. For this reason the `PublicJobsLookupValuesIndexSet` must *always* be built or rebuilt after `PublicJobsIndexSet`.

We have a second set of jobs which are open only to applicants who are eligible for redeployment, so we repeat the above process to create two more index sets and indexers with the prefix `RedeploymentJobs` instead of `PublicJobs`. These work in exactly the same way but based on a different data source.

You can recreate the configuration for these index sets and indexes by applying the `ExamineIndex.config.xdt` and `ExamineSettings.config.xdt` transforms to your own installation of this project.

### Updating the data

To update the jobs data you need a trigger a reindex for each of the index sets. You can do this in one of the following ways:

* The recommended way is to call `https://hostname/umbraco/api/jobsindexerapi/updatejobsearch`, which rebuilds all of the jobs indexes in the correct order. This requires you to authenticate using the approach documented for  [Escc.BasicAuthentication.WebApi](https://github.com/east-sussex-county-council/Escc.BasicAuthentication.WebApi). 
* The web API call above is expected to be set up as a scheduled task (or web job on Microsoft Azure), so that jobs data is regularly updated from the external jobs provider. You also have the option of running this task manually to trigger an update. 
* Sign into Umbraco with administrator permissions and navigate to Developer > Examine Management > Indexers > [select the indexer to update] > Index info & tools > Rebuild index

If the data source is unavailable during a reindex the jobs data we already have will be lost, so we will have no data to display. Unfortunately this behaviour is built into the way Umbraco calls the `ISimpleDataService` interface. 

### Read from an API to support load-balancing

When Umbraco updates its internal Examine indexes it does so across all load-balanced servers. This doesn't happen with the `ISimpleDataService` interface used by jobs, so only the local index is updated. This is a problem in a load-balanced scenario unless you are able to trigger an update on every load-balanced server in the farm.

To allow load-balancing to be used, the jobs API built into this application should be hosted on a non-load-balanced server, and the jobs data on that server should be kept up-to-date as described above. Load-balanced front-end servers should specify the API server URL in `web.config`:

	<appSettings>
	    <add key="JobsApiBaseUrl" value="https://hostname"/>
	</appSettings> 

## Searching the data

A `PublicJobsSearcher` and `PublicJobsLookupValuesSearcher` are configured in `~\config\ExamineSettings.config` linking to the index sets set up above. Equivalent searchers are also created for redeployment jobs.

The `JobsDataFromExamine` class implements `IJobsDataProvider` and provides methods to query the jobs data in Examine. You need to provide it a reference to either the `PublicJobsSearcher` or the `RedeploymentJobsSearcher` so that it knows which index set to search.

Searching this data in Examine rather than directly from our external jobs provider allows us to adjust the way the search behaves by writing our own queries in Lucene syntax.

## Umbraco document types for jobs

A series of Umbraco document types and templates can be used to build up a jobs site from Umbraco content. These have properties to connect one page to another (for example, the search page to the search results page) rather than hard-coding the connections between them. This has three advantages:

* Umbraco users can create their own information architecture for jobs
* Parallel sites can be created for public jobs and redeployment jobs
* Features are be turned on or off on each page depending on which connections have been made. For example, linking from the search results page to the RSS feed enables a link to the results of the same search as RSS.

Just one instance of the `Job advert` document type is required to display any job. This works because the `JobAdvertContentFinder`, hooked up in `JobAdvertEventHandler`, recognises the URL of this page when it has an id and job title appended. For example when `/job-advert` is accessed as `/job-advert/12345/teacher-at-example-school`. The id is used to look up and display the job, and the job title is there purely for SEO.

Most of the templates could work with any implementation of `IJobsDataProvider`, but there is also a `Jobs component` template which is designed specifically to host code provided by our external jobs provider. This is used for features we cannot replicate in Examine such as logging in to view job applications. 

## Reporting problems with missing data

Job searches work best when jobs contain complete and consistent data, but this doesn't always happen. To help recruitment staff spot issues created by missing data, the `Problem jobs` document type creates an RSS feed of jobs that may have missing data. Recruitment staff can subscribe to this feed using a site like [Blogtrottr](https://blogtrottr.com/) to get notified of problems as new jobs are posted.