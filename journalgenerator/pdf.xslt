<?xml version="1.0" encoding="UTF-8" ?>

<xsl:stylesheet version="1.0"
   xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
   xmlns="http://www.w3.org/TR/REC-html40">


  <xsl:template match="pdf">
    pdf: <xsl:value-of select="text()"/>
  </xsl:template>

  <xsl:template match="*/text()" />


</xsl:stylesheet>
