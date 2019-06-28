########################################################################
### BOOTSTRAP. Copy this section into each application setup script  ###
###            to make sure required functions are available.        ###
########################################################################

$pathOfThisScript = Split-Path $MyInvocation.MyCommand.Path -Parent
$parentFolderOfThisScript = $pathOfThisScript | Split-Path -Parent
$scriptsProject = 'Escc.WebApplicationSetupScripts'
$functionsPath = "$pathOfThisScript\..\$scriptsProject\functions.ps1"
if (Test-Path $functionsPath) {
  Write-Host "Checking $scriptsProject is up-to-date"
  Push-Location "$pathOfThisScript\..\$scriptsProject"
  git pull origin master
  Pop-Location
  Write-Host
  .$functionsPath
} else {
  if ($env:GIT_ORIGIN_URL) {
    $repoUrl = $env:GIT_ORIGIN_URL -f $scriptsProject
    git clone $repoUrl "$pathOfThisScript\..\$scriptsProject"
  } 
  else 
  {
    Write-Warning '$scriptsProject project not found. Please set a GIT_ORIGIN_URL environment variable on your system so that it can be downloaded.
  
Example: C:\>set GIT_ORIGIN_URL=https://example-git-server.com/{0}"
  
{0} will be replaced with the name of the repository to download.'
    Exit
  }
}

########################################################################
### END BOOTSTRAP. #####################################################
########################################################################

### 1. Setup website project ###

$projectName = "Escc.EastSussexGovUK.Umbraco.Web"

EnableDotNet40InIIS
CreateApplicationPool $projectName
CreateWebsite $projectName "$pathOfThisScript\$projectName"
CreateHTTPSBinding $projectName
RemoveHTTPBinding $projectName 80

# Give application pool account write access so that it can write temporary files
Write-Host "Granting Modify access to the application pool account"
$acl = Get-Acl "$pathOfThisScript\$projectName"
$rule = New-Object System.Security.AccessControl.FileSystemAccessRule("IIS AppPool\$projectName", "Modify", "ContainerInherit,ObjectInherit", "None", "Allow")
$acl.SetAccessRule($rule)
Set-Acl "$pathOfThisScript\$projectName" $acl

# Run NuGet restore on this project so that packages are available to copy files from
if (Get-Command "nuget.exe" -ErrorAction SilentlyContinue) 
{
	Write-Host "Restoring NuGet packages for $pathOfThisScript\$projectName"
	& nuget restore "$pathOfThisScript\$projectName\packages.config" -PackagesDirectory "$pathOfThisScript\packages"
} else {
	Write-Warning "Unable to restore NuGet packages because nuget.exe was not found in your path. If you get build errors, add nuget.exe to your path and run this script again."
}

# Get the installed versions of Umbraco and relevant packages
$xml = [xml](Get-Content "$pathOfThisScript\$projectName\packages.config")
$umbracoVersion = $xml.SelectSingleNode("/packages/package[@id='UmbracoCms']").GetAttribute("version");
Write-Host "UmbracoCms $umbracoVersion detected"

$modelsBuilderVersion = $xml.SelectSingleNode("/packages/package[@id='Umbraco.ModelsBuilder']").GetAttribute("version");
Write-Host "Umbraco.ModelsBuilder $modelsBuilderVersion detected"

$formsVersion = $xml.SelectSingleNode("/packages/package[@id='UmbracoForms']").GetAttribute("version");
Write-Host "UmbracoForms $formsVersion detected"

$securityConfigVersion = $xml.SelectSingleNode("/packages/package[@id='Escc.Web.SecurityConfig']").GetAttribute("version");
Write-Host "Escc.Web.SecurityConfig $securityConfigVersion detected"

$egukSecurityConfigVersion = $xml.SelectSingleNode("/packages/package[@id='Escc.EastSussexGovUK.SecurityConfig']").GetAttribute("version");
Write-Host "Escc.EastSussexGovUK.SecurityConfig $egukSecurityConfigVersion detected"

$egukClientDependencyVersion = $xml.SelectSingleNode("/packages/package[@id='Escc.EastSussexGovUK.ClientDependency']").GetAttribute("version");
Write-Host "Escc.EastSussexGovUK.ClientDependency $egukClientDependencyVersion detected"

$propertyEditorsVersion = $xml.SelectSingleNode("/packages/package[@id='Escc.Umbraco.PropertyEditors']").GetAttribute("version");
Write-Host "Escc.Umbraco.PropertyEditors $propertyEditorsVersion detected"

$imageProcessorConfigVersion = $xml.SelectSingleNode("/packages/package[@id='ImageProcessor.Web.Config']").GetAttribute("version");
Write-Host "ImageProcessor.Web.Config $securityConfigVersion detected"

$uSyncCoreVersion = $xml.SelectSingleNode("/packages/package[@id='uSync.Core']").GetAttribute("version");
Write-Host "uSync.Core $uSyncCoreVersion detected"

$uSyncVersion = $xml.SelectSingleNode("/packages/package[@id='uSync']").GetAttribute("version");
Write-Host "uSync $uSyncVersion detected"

$uSyncSnapshotsVersion = $xml.SelectSingleNode("/packages/package[@id='uSync.Snapshots']").GetAttribute("version");
Write-Host "uSync.Snapshots $uSyncSnapshotsVersion detected"

# Copy files from the Umbraco package if missing
& robocopy "packages\UmbracoCms.$umbracoVersion\Content\" "$pathOfThisScript\$projectName\" /E /XC /XN /XO /XF *.transform *.xdt
CopyConfig "packages\UmbracoCms.$umbracoVersion\UmbracoFiles\web.config" "$projectName\web.config"
CopyConfig "packages\UmbracoCms.$umbracoVersion\Content\Views\web.config.transform" "$projectName\Views\web.config"
& robocopy "packages\Umbraco.ModelsBuilder.$modelsBuilderVersion\Content\App_Plugins\" "$pathOfThisScript\$projectName\App_Plugins\" /E /XC /XN /XO
& robocopy "packages\UmbracoForms.$formsVersion\content\App_Plugins\" "$pathOfThisScript\$projectName\App_Plugins\" /E /XC /XN /XO /XF *.xdt

# Transform the web.config with the XDT for the project
TransformConfig "$pathOfThisScript\$projectName\web.config" "$pathOfThisScript\$projectName\web.config" "$pathOfThisScript\$projectName\web.config.xdt"

# If a debug-only transform for web.config is found, apply it
if (Test-Path "$pathOfThisScript\$projectName\Web.Debug.config")
{
	TransformConfig "$pathOfThisScript\$projectName\web.config" "$pathOfThisScript\$projectName\web.config" "$pathOfThisScript\$projectName\Web.Debug.config"
}

# Apply security headers and content security policy to project
TransformConfig "$pathOfThisScript\$projectName\web.config" "$pathOfThisScript\$projectName\web.config" "$pathOfThisScript\packages\Escc.Web.SecurityConfig.$securityConfigVersion\Content\web.config.install.xdt"
TransformConfig "$pathOfThisScript\$projectName\web.config" "$pathOfThisScript\$projectName\web.config" "$pathOfThisScript\packages\Escc.EastSussexGovUK.SecurityConfig.$egukSecurityConfigVersion\Content\web.config.install.xdt"

# Update web.config with settings only appropriate for development
$xml = [xml](Get-Content "$pathOfThisScript\$projectName\web.config")

# Remove the removeServerHeader attribute inserted by Escc.Web.SecurityConfig, which will cause the site to fail to load under IIS < 10 (likely in development)
$requestFilteringElement = $xml.SelectSingleNode("/configuration/system.webServer/security/requestFiltering")
$requestFilteringElement.RemoveAttribute("removeServerHeader")

# Remove the umbracoConfigurationStatus so that the Umbraco installer is triggered. If umbracoConfigurationStatus is set but umbracoDbDSN is not, you just get a login that doesn't work.
$umbracoDbDSNElement = $xml.SelectSingleNode("/configuration/connectionStrings/add[@name='umbracoDbDSN']")
$umbracoDbDSN = $umbracoDbDSNElement.GetAttribute("connectionString")
if (!$umbracoDbDSN) {
    $xml.SelectSingleNode("/configuration/appSettings/add[@key='umbracoConfigurationStatus']").SetAttribute("value", "")
}

# Reset fcnMode to Single which is the recommended value for Umbraco. We use the Disabled setting on Azure to avoid application restarts, but in development you want it to restart when you modify files.
$xml.SelectSingleNode("/configuration/system.web/httpRuntime").SetAttribute("fcnMode", "Single")

# Set debug=true and disable custom errors and caching for development
$xml.SelectSingleNode("/configuration/system.web/compilation").SetAttribute("debug", "true")
$xml.SelectSingleNode("/configuration/system.web/customErrors").SetAttribute("mode", "Off")
$xml.SelectSingleNode("/configuration/system.webServer/httpErrors").SetAttribute("errorMode", "Detailed")
$xml.SelectSingleNode("/configuration/Escc.Umbraco/GeneralSettings/add[@key='HttpCachingEnabled']").SetAttribute("value", "false")

# Always set IsUmbracoBackOffice as development is unlikely to be load-balanced, but this ensures RegisterServerRoleEventHandler sets the web project as master and the API project as slave
if (!$xml.SelectSingleNode("/configuration/appSettings/add[@key='IsUmbracoBackOffice']"))
{
	$isUmbracoBackOfficeElement = $xml.CreateElement("add")
	$isUmbracoBackOfficeElement.SetAttribute("key", "IsUmbracoBackOffice")
	$isUmbracoBackOfficeElement.SetAttribute("value", "true")
	$appSettings = $xml.SelectSingleNode("/configuration/appSettings")
	$appSettings.AppendChild($isUmbracoBackOfficeElement)
}

# Set ApiUser and ApiKey to simple values for development. These are used by Escc.BasicAuthentication.WebApi when connecting to some custom Umbraco web apis.
if (!$xml.SelectSingleNode("/configuration/appSettings/add[@key='ApiUser']"))
{
	$apiUserElement = $xml.CreateElement("add")
	$apiUserElement.SetAttribute("key", "ApiUser")
	$apiUserElement.SetAttribute("value", "dev")
	$appSettings = $xml.SelectSingleNode("/configuration/appSettings")
	$appSettings.AppendChild($apiUserElement)
}

if (!$xml.SelectSingleNode("/configuration/appSettings/add[@key='ApiKey']"))
{
	$apiUserElement = $xml.CreateElement("add")
	$apiUserElement.SetAttribute("key", "ApiKey")
	$apiUserElement.SetAttribute("value", "dev")
	$appSettings = $xml.SelectSingleNode("/configuration/appSettings")
	$appSettings.AppendChild($apiUserElement)
}

$xml.Save("$pathOfThisScript\$projectName\web.temp.config")
copy "$pathOfThisScript\$projectName\web.temp.config" "$pathOfThisScript\$projectName\web.config"
del "$pathOfThisScript\$projectName\web.temp.config"

# Configure sitewide CSS and JS files for project
TransformConfig "$pathOfThisScript\$projectName\web.config" "$pathOfThisScript\$projectName\web.config" "$pathOfThisScript\packages\Escc.EastSussexGovUK.ClientDependency.$egukClientDependencyVersion\Content\web.config.install.xdt"

# Copy files into /config if not already present
& robocopy "packages\UmbracoCms.$umbracoVersion\Content\Config" "$pathOfThisScript\$projectName\config" /E /XC /XN /XO /XF *.xdt
if ((Test-Path "$pathOfThisScript\$projectName\config\imageprocessor\cache.config") -eq 0)
{
	& robocopy "packages\ImageProcessor.Web.Config.$imageProcessorConfigVersion\content\config\imageprocessor" "$pathOfThisScript\$projectName\config\imageprocessor" /E /XC /XN /XO
	Get-ChildItem "$pathOfThisScript\$projectName\config\imageprocessor\*.config.transform" | Rename-Item -NewName { $_.Name.Replace('.config.transform','.config') }
}
TransformConfig "$pathOfThisScript\$projectName\config\imageprocessor\cache.config" "$pathOfThisScript\$projectName\config\imageprocessor\cache.config" "$pathOfThisScript\packages\UmbracoCms.$umbracoVersion\Content\Config\imageprocessor\cache.config.install.xdt"
TransformConfig "$pathOfThisScript\$projectName\config\imageprocessor\processing.config" "$pathOfThisScript\$projectName\config\imageprocessor\processing.config" "$pathOfThisScript\packages\UmbracoCms.$umbracoVersion\Content\Config\imageprocessor\processing.config.install.xdt"
CopyConfig "packages\uSync.Core.$uSyncCoreVersion\content\config\uSyncCore.Config" "$pathOfThisScript\$projectName\config\uSyncCore.Config"
& robocopy "packages\uSync.$uSyncVersion\content\" "$pathOfThisScript\$projectName\" /E /XC /XN /XO /XF *.xdt
& robocopy "packages\uSync.Snapshots.$uSyncSnapshotsVersion\content\" "$pathOfThisScript\$projectName\" /E /XC /XN /XO
TransformConfig "$pathOfThisScript\$projectName\config\dashboard.config" "$pathOfThisScript\$projectName\config\dashboard.config" "$pathOfThisScript\packages\uSync.$uSyncVersion\content\config\dashboard.config.install.xdt"

# Configure custom Examine indexes
TransformConfig "$pathOfThisScript\$projectName\config\ExamineIndex.config" "$pathOfThisScript\$projectName\config\ExamineIndex.config" "$pathOfThisScript\$projectName\RightsOfWayDeposits\ExamineIndex.config.xdt"
TransformConfig "$pathOfThisScript\$projectName\config\ExamineSettings.config" "$pathOfThisScript\$projectName\config\ExamineSettings.config" "$pathOfThisScript\$projectName\RightsOfWayDeposits\ExamineSettings.config.xdt"
TransformConfig "$pathOfThisScript\$projectName\config\ExamineIndex.config" "$pathOfThisScript\$projectName\config\ExamineIndex.config" "$pathOfThisScript\$projectName\RightsOfWayModifications\ExamineIndex.config.xdt"
TransformConfig "$pathOfThisScript\$projectName\config\ExamineSettings.config" "$pathOfThisScript\$projectName\config\ExamineSettings.config" "$pathOfThisScript\$projectName\RightsOfWayModifications\ExamineSettings.config.xdt"

# Configure file types which can be uploaded - use a whitelist rather than blacklist as it's more secure, and remove .xml from the blacklist to allow school term dates to be uploaded
$xml = [xml](Get-Content "$pathOfThisScript\$projectName\config\umbracoSettings.config")
$contentElement = $xml.SelectSingleNode("/settings/content")
$contentElement.SelectSingleNode("disallowedUploadFiles")."#text" = "ashx,aspx,ascx,config,cshtml,vbhtml,asmx,air,axd,swf,html,htm,php,htaccess"
$allowedUploads = $contentElement.SelectSingleNode("allowedUploadFiles")
if (!$allowedUploads) {
	$allowedUploads = $xml.CreateElement("allowedUploadFiles")
	$allowedUploads.AppendChild($xml.CreateTextNode("csv,doc,docx,gif,jpg,pdf,png,ppsx,ppt,pptx,rtf,svg,xls,xlsm,xlsx,xml"))
	$contentElement.AppendChild($allowedUploads)
}
$xml.Save("$pathOfThisScript\$projectName\config\umbracoSettings.temp.config")
copy "$pathOfThisScript\$projectName\config\umbracoSettings.temp.config" "$pathOfThisScript\$projectName\config\umbracoSettings.config"
del "$pathOfThisScript\$projectName\config\umbracoSettings.temp.config"

# Configure TinyMCE editor, including adding the blockquote button used only by 'Standard topic page'
TransformConfig "$pathOfThisScript\$projectName\config\tinyMceConfig.config" "$pathOfThisScript\$projectName\config\tinyMceConfig.config" "$pathOfThisScript\packages\Escc.Umbraco.PropertyEditors.$propertyEditorsVersion\Content\config\tinymceconfig.config.install.xdt" 

# Create an /img virtual directory to load template elements from the tightly-coupled Escc.EastSussexGovUK.TemplateSource project
CheckApplicationExists "$pathOfThisScript\.." "Escc.EastSussexGovUK"
if (Test-Path "IIS:\Sites\$projectName\img") 
{
    Write-Host "Virtual directory img already exists"
} 
else 
{
    Write-Host "Creating virtual directory img"
    New-WebVirtualDirectory -Site $projectName -Name "img" -PhysicalPath "$pathOfThisScript\..\Escc.EastSussexGovUK\Escc.EastSussexGovUK.TemplateSource\img"
}

CopyConfig "$pathOfThisScript\$projectName\images\web.example.config" "$pathOfThisScript\$projectName\images\web.config"


### 2. Setup API project ###

$projectName = "Escc.EastSussexGovUK.Umbraco.Api"

CreateApplicationPool $projectName
CreateWebsite $projectName "$pathOfThisScript\$projectName"
CreateHTTPSBinding $projectName
RemoveHTTPBinding $projectName 80

# Give application pool account write access so that it can write temporary files
Write-Host "Granting Modify access to the application pool account"
$acl = Get-Acl "$pathOfThisScript\$projectName"
$rule = New-Object System.Security.AccessControl.FileSystemAccessRule("IIS AppPool\$projectName", "Modify", "ContainerInherit,ObjectInherit", "None", "Allow")
$acl.SetAccessRule($rule)
Set-Acl "$pathOfThisScript\$projectName" $acl

# Run NuGet restore on this project so that packages are available to copy files from
if (Get-Command "nuget.exe" -ErrorAction SilentlyContinue) 
{
	Write-Host "Restoring NuGet packages for $pathOfThisScript\$projectName"
	& nuget restore "$pathOfThisScript\$projectName\packages.config" -PackagesDirectory "$pathOfThisScript\packages"
} else {
	Write-Warning "Unable to restore NuGet packages because nuget.exe was not found in your path. If you get build errors, add nuget.exe to your path and run this script again."
}

# Get the installed Umbraco version
$xml = [xml](Get-Content "$pathOfThisScript\$projectName\packages.config")
$umbracoVersion = $xml.SelectSingleNode("/packages/package[@id='UmbracoCms']").GetAttribute("version");
Write-Host "UmbracoCms $umbracoVersion detected"

$modelsBuilderVersion = $xml.SelectSingleNode("/packages/package[@id='Umbraco.ModelsBuilder']").GetAttribute("version");
Write-Host "Umbraco.ModelsBuilder $modelsBuilderVersion detected"

# Copy files from the Umbraco package if missing
& robocopy "packages\UmbracoCms.$umbracoVersion\Content\" "$pathOfThisScript\$projectName\" /E /XC /XN /XO /XF *.transform *.xdt
CopyConfig "packages\UmbracoCms.$umbracoVersion\UmbracoFiles\web.config" "$projectName\web.config"
CopyConfig "packages\UmbracoCms.$umbracoVersion\Content\Views\web.config.transform" "$projectName\Views\web.config"
& robocopy "packages\Umbraco.ModelsBuilder.$modelsBuilderVersion\Content\App_Plugins\" "$pathOfThisScript\$projectName\App_Plugins\" /E /XC /XN /XO

# Transform the web.config with the XDT for the project
TransformConfig "$pathOfThisScript\$projectName\web.config" "$pathOfThisScript\$projectName\web.config" "$pathOfThisScript\$projectName\web.config.xdt"

# If a debug-only transform for web.config is found, apply it
if (Test-Path "$pathOfThisScript\$projectName\Web.Debug.config")
{
	TransformConfig "$pathOfThisScript\$projectName\web.config" "$pathOfThisScript\$projectName\web.config" "$pathOfThisScript\$projectName\Web.Debug.config"
}

# Update web.config with settings only appropriate for development
$xml = [xml](Get-Content "$pathOfThisScript\$projectName\web.config")

# Remove the removeServerHeader attribute inserted by web.config.xdt, which will cause the site to fail to load under IIS < 10 (likely in development)
$requestFilteringElement = $xml.SelectSingleNode("/configuration/system.webServer/security/requestFiltering")
$requestFilteringElement.RemoveAttribute("removeServerHeader")

# Reset fcnMode to Single which is the recommended value for Umbraco. We use the Disabled setting on Azure to avoid application restarts, but in development you want it to restart when you modify files.
$xml.SelectSingleNode("/configuration/system.web/httpRuntime").SetAttribute("fcnMode", "Single")

# Set debug=true and disable custom errors for development
$xml.SelectSingleNode("/configuration/system.web/compilation").SetAttribute("debug", "true")
$xml.SelectSingleNode("/configuration/system.web/customErrors").SetAttribute("mode", "Off")
$xml.SelectSingleNode("/configuration/system.webServer/httpErrors").SetAttribute("errorMode", "Detailed")

# Set ApiUser and ApiKey to simple values for development. These are used by Escc.BasicAuthentication.WebApi when connecting to some custom Umbraco web apis.
if (!$xml.SelectSingleNode("/configuration/appSettings/add[@key='ApiUser']"))
{
	$apiUserElement = $xml.CreateElement("add")
	$apiUserElement.SetAttribute("key", "ApiUser")
	$apiUserElement.SetAttribute("value", "dev")
	$appSettings = $xml.SelectSingleNode("/configuration/appSettings")
	$appSettings.AppendChild($apiUserElement)
}

if (!$xml.SelectSingleNode("/configuration/appSettings/add[@key='ApiKey']"))
{
	$apiUserElement = $xml.CreateElement("add")
	$apiUserElement.SetAttribute("key", "ApiKey")
	$apiUserElement.SetAttribute("value", "dev")
	$appSettings = $xml.SelectSingleNode("/configuration/appSettings")
	$appSettings.AppendChild($apiUserElement)
}

# Leave media links in job adverts as absolute URLs, because they'll point at media on the live site but this application will point at a development account for media
if (!$xml.SelectSingleNode("/configuration/appSettings/add[@key='DoNotRemoveMediaDomainInJobAdverts']"))
{
	$apiUserElement = $xml.CreateElement("add")
	$apiUserElement.SetAttribute("key", "DoNotRemoveMediaDomainInJobAdverts")
	$apiUserElement.SetAttribute("value", "true")
	$appSettings = $xml.SelectSingleNode("/configuration/appSettings")
	$appSettings.AppendChild($apiUserElement)
}

$xml.Save("$pathOfThisScript\$projectName\web.temp.config")
copy "$pathOfThisScript\$projectName\web.temp.config" "$pathOfThisScript\$projectName\web.config"
del "$pathOfThisScript\$projectName\web.temp.config"

# Copy files into /config if not already present
& robocopy "packages\UmbracoCms.$umbracoVersion\Content\Config" "$pathOfThisScript\$projectName\config" /E /XC /XN /XO /XF *.xdt
if ((Test-Path "$pathOfThisScript\$projectName\config\imageprocessor\cache.config") -eq 0)
{
	& robocopy "packages\ImageProcessor.Web.Config.$imageProcessorConfigVersion\content\config\imageprocessor" "$pathOfThisScript\$projectName\config\imageprocessor" /E /XC /XN /XO
	Get-ChildItem "$pathOfThisScript\$projectName\config\imageprocessor\*.config.transform" | Rename-Item -NewName { $_.Name.Replace('.config.transform','.config') }
}
TransformConfig "$pathOfThisScript\$projectName\config\imageprocessor\cache.config" "$pathOfThisScript\$projectName\config\imageprocessor\cache.config" "$pathOfThisScript\packages\UmbracoCms.$umbracoVersion\Content\Config\imageprocessor\cache.config.install.xdt"
TransformConfig "$pathOfThisScript\$projectName\config\imageprocessor\processing.config" "$pathOfThisScript\$projectName\config\imageprocessor\processing.config" "$pathOfThisScript\packages\UmbracoCms.$umbracoVersion\Content\Config\imageprocessor\processing.config.install.xdt"

# Configure custom Examine indexes
TransformConfig "$pathOfThisScript\$projectName\config\ExamineIndex.config" "$pathOfThisScript\$projectName\config\ExamineIndex.config" "$pathOfThisScript\$projectName\Jobs\Examine\ExamineIndex.config.xdt"
TransformConfig "$pathOfThisScript\$projectName\config\ExamineSettings.config" "$pathOfThisScript\$projectName\config\ExamineSettings.config" "$pathOfThisScript\$projectName\Jobs\Examine\ExamineSettings.config.xdt"

### 3. Connect web application to API ### 

$projectName = "Escc.EastSussexGovUK.Umbraco.Web"

$xml = [xml](Get-Content "$pathOfThisScript\$projectName\web.config")
$apiBaseUrlElement = $xml.SelectSingleNode("/configuration/appSettings/add[@key='JobsApiBaseUrl']")
if (!$apiBaseUrlElement)
{
	Write-Host Connecting web application to API
	$apiPort = @(Get-WebBinding -Name "Escc.EastSussexGovUK.Umbraco.Api" -Protocol https)[0].BindingInformation -replace "[^0-9]", ""
		
	$apiBaseUrlElement = $xml.CreateElement("add")
	$apiBaseUrlElement.SetAttribute("key", "JobsApiBaseUrl")
	$apiBaseUrlElement.SetAttribute("value", "https://localhost:$apiPort")
	$appSettings = $xml.SelectSingleNode("/configuration/appSettings")
	$appSettings.AppendChild($apiBaseUrlElement)

	$xml.Save("$pathOfThisScript\$projectName\web.temp.config")
	copy "$pathOfThisScript\$projectName\web.temp.config" "$pathOfThisScript\$projectName\web.config"
	del "$pathOfThisScript\$projectName\web.temp.config"
}
$apiBaseUrl = $apiBaseUrlElement.GetAttribute("value")

### 4. Setup console apps ###

$projectName = "Escc.Jobs.SendAlerts"

CopyConfig "$projectName\app.example.config" "$projectName\app.config"
$xml = [xml](Get-Content "$pathOfThisScript\$projectName\app.config")
$xml.SelectSingleNode("/configuration/appSettings/add[@key='JobsApiBaseUrl']").SetAttribute("value", $apiBaseUrl)
$xml.SelectSingleNode("/configuration/appSettings/add[@key='Frequency']").SetAttribute("value", "1")
$xml.SelectSingleNode("/configuration/appSettings/add[@key='Precondition']").SetAttribute("value", "")

$xml.Save("$pathOfThisScript\$projectName\app.temp.config")
copy "$pathOfThisScript\$projectName\app.temp.config" "$pathOfThisScript\$projectName\app.config"
del "$pathOfThisScript\$projectName\app.temp.config"


$projectName = "Escc.Jobs.UpdateIndexes"

CopyConfig "$projectName\app.example.config" "$projectName\app.config"
$xml = [xml](Get-Content "$pathOfThisScript\$projectName\app.config")
$xml.SelectSingleNode("/configuration/appSettings/add[@key='HostnameEnvironmentVariable']").SetAttribute("value", "")
$xml.SelectSingleNode("/configuration/appSettings/add[@key='ApiUser']").SetAttribute("value", "dev")
$xml.SelectSingleNode("/configuration/appSettings/add[@key='ApiPassword']").SetAttribute("value", "dev")
$apiBaseUrlElement = $xml.SelectSingleNode("/configuration/appSettings/add[@key='JobsApiBaseUrl']")
if (!$apiBaseUrlElement)
{
	Write-Host Connecting Escc.Jobs.UpdateIndexes to API
	$apiBaseUrlElement = $xml.CreateElement("add")
	$apiBaseUrlElement.SetAttribute("key", "JobsApiBaseUrl")
	$apiBaseUrlElement.SetAttribute("value", $apiBaseUrl)
	$appSettings = $xml.SelectSingleNode("/configuration/appSettings")
	$appSettings.AppendChild($apiBaseUrlElement)
}
$xml.Save("$pathOfThisScript\$projectName\app.temp.config")
copy "$pathOfThisScript\$projectName\app.temp.config" "$pathOfThisScript\$projectName\app.config"
del "$pathOfThisScript\$projectName\app.temp.config"

Write-Host
Write-Host "Done." -ForegroundColor "Green"