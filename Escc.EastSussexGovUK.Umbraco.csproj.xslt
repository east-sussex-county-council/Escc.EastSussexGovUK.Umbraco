<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
    xmlns="http://schemas.microsoft.com/developer/msbuild/2003"
    xmlns:msbuild="http://schemas.microsoft.com/developer/msbuild/2003"
>
  <xsl:output method="xml" indent="yes"/>

  <!-- TransformProjectFile.xslt is from Escc.AzureDeployment and will be copied into this folder at deploy time  -->
  <xsl:include href="TransformProjectFile.xslt"/>

  <xsl:template match="msbuild:Project/msbuild:ItemGroup/msbuild:Reference/msbuild:HintPath">
    <xsl:call-template name="UpdateHintPath">
      <xsl:with-param name="ref_1" select="'Microsoft.ApplicationBlocks.ExceptionManagement.dll'" />
      <xsl:with-param name="ref_2" select="'AST.AzureBlobStorage.dll'" />
      <xsl:with-param name="ref_3" select="'AzureDirectory.dll'" />
      <xsl:with-param name="ref_4" select="'Examine.Azure.dll'" />
      <xsl:with-param name="ref_6" select="'Microsoft.WindowsAzure.ServiceRuntime.dll'" />
      <xsl:with-param name="ref_7" select="'Microsoft.WindowsAzure.StorageClient.dll'" />
      <xsl:with-param name="ref_8" select="'UmbracoExamine.Azure.dll'" />
      <!-- Temporarily disabled as not working with Umbraco 7.1.x
      <xsl:with-param name="ref_9" select="'Examine.dll'" />
      <xsl:with-param name="ref_10" select="'UmbracoExamine.dll'" />-->
    </xsl:call-template>
  </xsl:template>

</xsl:stylesheet>
