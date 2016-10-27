<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
    <xsl:output method="html" indent="yes"/>
    
    <!-- Accept date as external parameter, because easier to format it there -->
    <xsl:param name="displayDate" />
  
    <!-- Format flood section, but don't include surrounding tags that limit where on the page it can appear -->
    <xsl:template match="/">
      <h2>Flood warnings for the south-east</h2>
        <p>From the Environment Agency at <xsl:value-of select="$displayDate"/>.</p>
        <xsl:apply-templates select="rss/channel/item" />
        <p class="flood-signup">
          <a href="http://www.environment-agency.gov.uk/homeandleisure/floods/38289.aspx">Sign up to Floodline Warnings Direct</a>
        </p>
    </xsl:template>

  <!-- Match each level of flood warning -->
  <xsl:template match="/rss/channel/item">
    
    <!-- Get the type of warning, and the number in force -->
    <xsl:variable name="warningText">
      <xsl:value-of select="title"/>
    </xsl:variable>
    <xsl:variable name="number" select="substring-before($warningText,' ')" />
    
    <!-- For each type of warning:
         - display the symbol as an img tag rather than CSS background so that it prints 
         - give the type of warning, correctly pluralised unlike Environment Agency version
         - show a link to the actual warnings only if there are any in force
         -->
    <xsl:choose>
      <xsl:when test="contains($warningText, 'Severe Flood Warning')">
        <p class="flood severe">
          <img src="/images/flood-severe.png" alt="Severe flood warning sign" width="45" height="42" />
          <xsl:value-of select="$number"/>
          <xsl:choose>
            <xsl:when test="$number = 1">
              severe flood warning
            </xsl:when>
            <xsl:otherwise>
              severe flood warnings
            </xsl:otherwise>
          </xsl:choose>
          <xsl:if test="$number > 0">
          <xsl:text disable-output-escaping="yes">&lt;a href=&quot;</xsl:text>
          <xsl:value-of select="link"/>
          <xsl:text disable-output-escaping="yes">&quot;&gt;View severe warnings&lt;/a&gt;</xsl:text>
          </xsl:if>
        </p>
      </xsl:when>

      <xsl:when test="contains($warningText, 'Flood Warning')">
        <p class="flood">
          <img src="/images/flood-warning.png" alt="Flood warning sign" width="45" height="42" />
          <xsl:value-of select="$number"/>
          <xsl:choose>
            <xsl:when test="$number = 1">
              flood warning
            </xsl:when>
            <xsl:otherwise>
              flood warnings
            </xsl:otherwise>
          </xsl:choose>
          <xsl:if test="$number > 0">
            <xsl:text disable-output-escaping="yes">&lt;a href=&quot;</xsl:text>
            <xsl:value-of select="link"/>
            <xsl:text disable-output-escaping="yes">&quot;&gt;View warnings&lt;/a&gt;</xsl:text>
          </xsl:if>
        </p>
      </xsl:when>
      
      <xsl:when test="contains($warningText, 'Flood Alert')">
        <p class="flood">
          <img src="/images/flood-alert.png" alt="Flood alert sign" width="45" height="42" />
          <xsl:value-of select="$number"/>
          <xsl:choose>
            <xsl:when test="$number = 1">
              flood alert
            </xsl:when>
            <xsl:otherwise>
              flood alerts
            </xsl:otherwise>
          </xsl:choose>
          <xsl:if test="$number > 0">
            <xsl:text disable-output-escaping="yes">&lt;a href=&quot;</xsl:text>
            <xsl:value-of select="link"/>
            <xsl:text disable-output-escaping="yes">&quot;&gt;View alerts&lt;/a&gt;</xsl:text>
          </xsl:if>
        </p>
      </xsl:when>      
        </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
