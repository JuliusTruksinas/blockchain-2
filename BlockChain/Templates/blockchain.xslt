<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="html" indent="yes"/>

  <xsl:template match="/">
    <html>
      <head>
        <title>Blockchain Viewer</title>
        <style>
          body {
            font-family: 'Arial', sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f4f4f9;
            color: #333;
          }
          header {
            background-color: #2c3e50;
            color: white;
            padding: 20px;
            text-align: center;
          }
          h1 {
            font-size: 2.5em;
            margin: 0;
          }
          .container {
            width: 90%;
            margin: 30px auto;
          }
          .block {
            background-color: #fff;
            border: 1px solid #ddd;
            margin-bottom: 20px;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
            transition: all 0.3s ease;
          }
          .block:hover {
            box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
          }
          .block-header {
            background-color: #ecf0f1;
            padding: 15px;
            border-radius: 8px;
            margin-bottom: 10px;
            font-weight: bold;
          }
          .block-content {
            margin-left: 15px;
          }
          .hash, .timestamp {
            color: #2980b9;
          }
          .collapsible {
            background-color: #3498db;
            color: white;
            cursor: pointer;
            padding: 10px;
            border: none;
            text-align: left;
            width: 100%;
            border-radius: 5px;
            font-size: 1.1em;
            transition: all 0.3s ease;
          }
          .collapsible:hover {
            background-color: #2980b9;
          }
          .content {
            padding: 0 15px;
            display: none;
            overflow: hidden;
            background-color: #f9f9f9;
            border-left: 2px solid #ddd;
          }
          .transaction {
            margin-bottom: 10px;
            padding: 10px;
            background-color: #ecf0f1;
            border-radius: 5px;
            border: 1px solid #ccc;
          }
          .transaction-id {
            font-weight: bold;
            color: #2c3e50;
          }
        </style>

        <!-- JavaScript for Collapsible Sections -->
        <script>
          function toggleCollapsible(event) {
            var content = event.target.nextElementSibling;
            if (content.style.display === "block") {
              content.style.display = "none";
            } else {
              content.style.display = "block";
            }
          }
        </script>
      </head>
      <body>
        <header>
          <h1>Blockchain Viewer</h1>
          <p>A beautiful, interactive view of the blockchain</p>
        </header>

        <div class="container">
          <xsl:for-each select="Blockchain/Block">
            <div class="block">
              <!-- Block Header -->
              <div class="block-header">
                <xsl:value-of select="@Index" /> - Block <span class="hash">Hash: <xsl:value-of select="Hash" /></span>
              </div>
              
              <!-- Block Details (Header) -->
              <div class="block-content">
                <button class="collapsible" onclick="toggleCollapsible(event)">Block Details</button>
                <div class="content">
                  <p><strong>Previous Hash:</strong> <xsl:value-of select="Header/PreviousHash" /></p>
                  <p><strong>Timestamp:</strong> <xsl:value-of select="Header/Timestamp" /></p>
                  <p><strong>Version:</strong> <xsl:value-of select="Header/Version" /></p>
                  <p><strong>Transactions Hash:</strong> <xsl:value-of select="Header/TransactionsHash" /></p>
                  <p><strong>Nonce:</strong> <xsl:value-of select="Header/Nonce" /></p>
                  <p><strong>Difficulty:</strong> <xsl:value-of select="Header/Difficulty" /></p>
                </div>
              </div>
              
              <!-- Transactions Section -->
              <button class="collapsible" onclick="toggleCollapsible(event)">Transactions</button>
              <div class="content">
                <xsl:for-each select="Transactions/Transaction">
                  <div class="transaction">
                    <p><span class="transaction-id">Transaction ID: <xsl:value-of select="Id" /></span></p>
                    <p><strong>Sender:</strong> <xsl:value-of select="Sender" /></p>
                    <p><strong>Receiver:</strong> <xsl:value-of select="Receiver" /></p>
                    <p><strong>Amount:</strong> <xsl:value-of select="Amount" /></p>
                  </div>
                </xsl:for-each>
              </div>
            </div>
          </xsl:for-each>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>