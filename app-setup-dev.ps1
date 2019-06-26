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

# Run NuGet restore on this project so that the Umbraco package is available
if (Get-Command "nuget.exe" -ErrorAction SilentlyContinue) 
{
	Write-Host "Restoring NuGet packages for $pathOfThisScript\$projectName"
	& nuget restore "$pathOfThisScript\$projectName\packages.config" -PackagesDirectory "$pathOfThisScript\packages"
} else {
	Write-Warning "Unable to restore NuGet packages because nuget.exe was not found in your path. If you get build errors, add nuget.exe to your path and run this script again."
}

# Get the installed versions of Umbraco and relevant packages
$xml = [xml](Get-Content "$pathOfThisScript\$projectName\packages.config")
$element = $xml.SelectSingleNode("/packages/package[@id='UmbracoCms']")
$umbracoVersion = $element.GetAttribute("version");
Write-Host "UmbracoCms $umbracoVersion detected"

$element = $xml.SelectSingleNode("/packages/package[@id='Escc.EastSussexGovUK.SecurityConfig']")
$securityConfigVersion = $element.GetAttribute("version");
Write-Host "Escc.EastSussexGovUK.SecurityConfig $securityConfigVersion detected"

$element = $xml.SelectSingleNode("/packages/package[@id='ImageProcessor.Web.Config']")
$imageProcessorConfigVersion = $element.GetAttribute("version");
Write-Host "ImageProcessor.Web.Config $securityConfigVersion detected"

$element = $xml.SelectSingleNode("/packages/package[@id='uSync.Core']")
$uSyncCoreVersion = $element.GetAttribute("version");
Write-Host "uSync.Core $uSyncCoreVersion detected"

$element = $xml.SelectSingleNode("/packages/package[@id='uSync']")
$uSyncVersion = $element.GetAttribute("version");
Write-Host "uSync $uSyncVersion detected"

$element = $xml.SelectSingleNode("/packages/package[@id='uSync.Snapshots']")
$uSyncSnapshotsVersion = $element.GetAttribute("version");
Write-Host "uSync.Snapshots $uSyncSnapshotsVersion detected"

# Copy files from the Umbraco package if missing
& robocopy "packages\UmbracoCms.$umbracoVersion\Content\" "$pathOfThisScript\$projectName\" /E /XC /XN /XO /XF *.transform *.xdt
CopyConfig "packages\UmbracoCms.$umbracoVersion\UmbracoFiles\web.config" "$projectName\web.config"

# Transform the web.config with the XDT for the project, via a temp file to avoid locking issues
TransformConfig "$pathOfThisScript\$projectName\web.config" "$pathOfThisScript\$projectName\web.temp.config" "$pathOfThisScript\$projectName\web.config.xdt"
copy "$pathOfThisScript\$projectName\web.temp.config" "$pathOfThisScript\$projectName\web.config"
del "$pathOfThisScript\$projectName\web.temp.config"

# Remove the removeServerHeader attribute, which will cause the site to fail to load under IIS < 10 (likely in development)
# Remove the umbracoConfigurationStatus so that the Umbraco installer is triggered. If umbracoConfigurationStatus is set but umbracoDbDSN is not, you just get a login that doesn't work.
$xml = [xml](Get-Content "$pathOfThisScript\$projectName\web.config")
$element = $xml.SelectSingleNode("/configuration/system.webServer/security/requestFiltering")
$element.RemoveAttribute("removeServerHeader")
$element = $xml.SelectSingleNode("/configuration/connectionStrings/add[@name='umbracoDbDSN']")
$umbracoDbDSN = $element.GetAttribute("connectionString")
if (!$umbracoDbDSN) {
    $element = $xml.SelectSingleNode("/configuration/appSettings/add[@key='umbracoConfigurationStatus']")
    $element.SetAttribute("value", "")
}
$xml.Save("$pathOfThisScript\$projectName\web.temp.config")
copy "$pathOfThisScript\$projectName\web.temp.config" "$pathOfThisScript\$projectName\web.config"
del "$pathOfThisScript\$projectName\web.temp.config"

# Apply content security policy to project
TransformConfig "$pathOfThisScript\$projectName\web.config" "$pathOfThisScript\$projectName\web.temp.config" "$pathOfThisScript\packages\Escc.EastSussexGovUK.SecurityConfig.$securityConfigVersion\Content\web.config.install.xdt"
copy "$pathOfThisScript\$projectName\web.temp.config" "$pathOfThisScript\$projectName\web.config"
del "$pathOfThisScript\$projectName\web.temp.config"

# Copy files into /config if not already present
& robocopy "packages\UmbracoCms.$umbracoVersion\Content\Config" "$pathOfThisScript\$projectName\config" /E /XC /XN /XO /XF *.xdt
if ((Test-Path "$pathOfThisScript\$projectName\config\imageprocessor\cache.config") -eq 0)
{
	& robocopy "packages\ImageProcessor.Web.Config.$imageProcessorConfigVersion\content\config\imageprocessor" "$pathOfThisScript\$projectName\config\imageprocessor" /E /XC /XN /XO
	Get-ChildItem "$pathOfThisScript\$projectName\config\imageprocessor\*.config.transform" | Rename-Item -NewName { $_.Name.Replace('.config.transform','.config') }
}
CopyConfig "packages\uSync.Core.$uSyncCoreVersion\content\config\uSyncCore.Config" "$pathOfThisScript\$projectName\config\uSyncCore.Config"
& robocopy "packages\uSync.$uSyncVersion\content\" "$pathOfThisScript\$projectName\" /E /XC /XN /XO /XF *.xdt
& robocopy "packages\uSync.Snapshots.$uSyncSnapshotsVersion\content\" "$pathOfThisScript\$projectName\" /E /XC /XN /XO

TransformConfig "$pathOfThisScript\$projectName\config\dashboard.config" "$pathOfThisScript\$projectName\config\dashboard.temp.config" "$pathOfThisScript\packages\uSync.$uSyncVersion\content\config\dashboard.config.install.xdt"
copy "$pathOfThisScript\$projectName\config\dashboard.temp.config" "$pathOfThisScript\$projectName\config\dashboard.config"
del "$pathOfThisScript\$projectName\config\dashboard.temp.config"

# Create an /img virtual directory to load template elements from the tightly-coupled Escc.EastSussexGovUK.TemplateSource project
CheckApplicationExists "$pathOfThisScript\.." "Escc.EastSussexGovUK"
CreateVirtualDirectory $projectName "img" "$pathOfThisScript\..\Escc.EastSussexGovUK\Escc.EastSussexGovUK.TemplateSource\img" true


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

# Run NuGet restore on this project so that the Umbraco package is available
if (Get-Command "nuget.exe" -ErrorAction SilentlyContinue) 
{
	Write-Host "Restoring NuGet packages for $pathOfThisScript\$projectName"
	& nuget restore "$pathOfThisScript\$projectName\packages.config" -PackagesDirectory "$pathOfThisScript\packages"
} else {
	Write-Warning "Unable to restore NuGet packages because nuget.exe was not found in your path. If you get build errors, add nuget.exe to your path and run this script again."
}

# Get the installed Umbraco version
$xml = [xml](Get-Content "$pathOfThisScript\$projectName\packages.config")
$element = $xml.SelectSingleNode("/packages/package[@id='UmbracoCms']")
$umbracoVersion = $element.GetAttribute("version");
Write-Host "Umbraco $umbracoVersion detected"

# Copy files from the Umbraco package if missing
& robocopy "packages\UmbracoCms.$umbracoVersion\Content\" "$pathOfThisScript\$projectName\" /E /XC /XN /XO /XF *.transform *.xdt
CopyConfig "packages\UmbracoCms.$umbracoVersion\UmbracoFiles\web.config" "$projectName\web.config"

# Transform the web.config with the XDT for the project, via a temp file to avoid locking issues
TransformConfig "$pathOfThisScript\$projectName\web.config" "$pathOfThisScript\$projectName\web.temp.config" "$pathOfThisScript\$projectName\web.config.xdt"
copy "$pathOfThisScript\$projectName\web.temp.config" "$pathOfThisScript\$projectName\web.config"
del "$pathOfThisScript\$projectName\web.temp.config"

# Remove the umbracoConfigurationStatus so that the Umbraco installer is triggered. If umbracoConfigurationStatus is set but umbracoDbDSN is not, you just get a login that doesn't work.
$xml = [xml](Get-Content "$pathOfThisScript\$projectName\web.config")
$element = $xml.SelectSingleNode("/configuration/system.webServer/security/requestFiltering")
$element.RemoveAttribute("removeServerHeader")
$element = $xml.SelectSingleNode("/configuration/connectionStrings/add[@name='umbracoDbDSN']")
$umbracoDbDSN = $element.GetAttribute("connectionString")
if (!$umbracoDbDSN) {
    $element = $xml.SelectSingleNode("/configuration/appSettings/add[@key='umbracoConfigurationStatus']")
    $element.SetAttribute("value", "")
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

### 3. Setup console apps ###

CopyConfig "Escc.Jobs.SendAlerts\app.example.config" "Escc.Jobs.SendAlerts\app.config"
CopyConfig "Escc.Jobs.UpdateIndexes\app.example.config" "Escc.Jobs.UpdateIndexes\app.config"
#>
Write-Host
Write-Host "Done." -ForegroundColor "Green"