<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/">
		<html>
			<body>
				<table border="1">
					<tr>
						<th>Ticket</th>
						<th>Document</th>
						<th>Naam</th>
						<th>Bedrag</th>
					</tr>
					<tr>
						<xsl:for-each select="/GenerationRequest">
							<td>
								<xsl:value-of select="Ticket"/>
							</td>
							<td>
								<xsl:value-of select="DocumentType"/>
							</td>
						</xsl:for-each>
				
						<xsl:for-each select="GenerationRequest/Payload">

							<td>
								<xsl:value-of select="Naam"/>
							</td>
							<td>
								<xsl:value-of select="Bedrag"/>
							</td>
						</xsl:for-each>
					</tr>
				</table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>