<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" indent="yes" encoding="UTF-8"/>

  <!-- ============================================================= -->
  <!--  Root – render blocks in the order they appear in the XML      -->
  <!-- ============================================================= -->
  <xsl:template match="/">
    <html lang="en">
      <head>
        <meta charset="UTF-8"/>
        <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
        <title>Blockchain Explorer</title>
        <style>
          /* ────────────────────── Global ────────────────────── */
          body{
            font-family:'Segoe UI',Tahoma,Geneva,Verdana,sans-serif;
            background:#f0f2f5;color:#4a4a4a;margin:0;padding:20px;
            display:flex;justify-content:center;min-height:100vh;
          }
          .blockchain-container{
            max-width:1200px;width:100%;display:flex;flex-direction:column;align-items:center;
            position:relative;
          }

          /* ────────────────────── Card ────────────────────── */
          .block-card{
            background:#fff;border:1px solid #d1d5db;border-radius:12px;
            padding:24px;margin:20px 0;box-shadow:0 4px 12px rgba(0,0,0,.05);
            max-width:600px;width:100%;position:relative;
            transition:transform .3s ease,box-shadow .3s ease;
          }
          .block-card:hover{transform:translateY(-5px);box-shadow:0 8px 20px rgba(0,0,0,.1);}

          /* ────────────────────── Chain links ────────────────────── */
          .block-card:not(:last-child)::after{
            content:'';position:absolute;bottom:-30px;left:50%;transform:translateX(-50%);
            width:4px;height:30px;background:linear-gradient(to bottom,#a0a0a0,#d1d5db);
            border-radius:2px;
          }
          .block-card:not(:last-child)::before{
            content:'';position:absolute;bottom:-45px;left:50%;transform:translateX(-50%);
            font-size:24px;color:#a0a0a0;animation:pulse 2s infinite;
          }
          @keyframes pulse{0%{opacity:.6}50%{opacity:1}100%{opacity:.6}}

          /* ────────────────────── Header ────────────────────── */
          h2{font-size:1.5em;margin:0 0 16px;color:#333;border-bottom:2px solid #e5e7eb;padding-bottom:8px;}
          .header{display:grid;grid-template-columns:repeat(auto-fit,minmax(200px,1fr));gap:12px;margin-bottom:20px;}
          .header-item{background:#f9fafb;padding:12px;border-radius:8px;font-size:.9em;word-break:break-all;}
          .header-item strong{display:block;color:#6b7280;font-weight:600;}

          /* ────────────────────── Hash ────────────────────── */
          .hash{font-size:1em;margin-bottom:20px;padding:12px;background:#e5e7eb;border-radius:8px;word-break:break-all;}

          /* ────────────────────── Accordion ────────────────────── */
          details.transactions{border:1px solid #d1d5db;border-radius:8px;overflow:hidden;transition:all .3s ease;}
          summary{
            background:#f3f4f6;padding:16px;font-weight:600;cursor:pointer;
            display:flex;justify-content:space-between;align-items:center;color:#4a4a4a;
            transition:background .3s ease;
          }
          summary:hover{background:#e5e7eb;}
          summary::after{content:'---->';transition:transform .3s ease;font-size:.8em;}
          details[open] summary::after{transform:rotate(90deg);}

          .transaction-list{padding:16px;display:flex;flex-direction:column;gap:12px;background:#fff;}
          .transaction{
            background:#f9fafb;padding:12px;border-radius:8px;font-size:.9em;
            word-break:break-word;               /* wrap long wallet strings */
          }
          .transaction strong{color:#6b7280;}

          /* ────────────────────── Responsive ────────────────────── */
          @media (max-width:768px){
            .block-card{padding:16px;}
            .header{grid-template-columns:1fr;}
          }
        </style>
      </head>
      <body>
        <div class="blockchain-container">
          <!-- Render blocks in source order – no sorting -->
          <xsl:apply-templates select="Blockchain/Block"/>
        </div>
      </body>
    </html>
  </xsl:template>

  <!-- ============================================================= -->
  <!--  Block template                                               -->
  <!-- ============================================================= -->
  <xsl:template match="Block">
    <div class="block-card">
      <h2>Block #<xsl:value-of select="@Index"/></h2>

      <div class="header">
        <div class="header-item"><strong>Previous Hash:</strong><xsl:value-of select="Header/PreviousHash"/></div>
        <div class="header-item"><strong>Timestamp:</strong><xsl:value-of select="Header/Timestamp"/></div>
        <div class="header-item"><strong>Version:</strong><xsl:value-of select="Header/Version"/></div>
        <div class="header-item"><strong>Transactions Hash:</strong><xsl:value-of select="Header/TransactionsHash"/></div>
        <div class="header-item"><strong>Nonce:</strong><xsl:value-of select="Header/Nonce"/></div>
        <div class="header-item"><strong>Difficulty:</strong><xsl:value-of select="Header/Difficulty"/></div>
      </div>

      <div class="hash"><strong>Block Hash:</strong> <xsl:value-of select="Hash"/></div>

      <details class="transactions">
        <summary>Transactions (<xsl:value-of select="count(Transactions/Transaction)"/>)</summary>
        <div class="transaction-list">
          <xsl:apply-templates select="Transactions/Transaction"/>
        </div>
      </details>
    </div>
  </xsl:template>

  <!-- ============================================================= -->
  <!--  Transaction template                                         -->
  <!-- ============================================================= -->
  <xsl:template match="Transaction">
    <div class="transaction">
      <strong>ID:</strong> <xsl:value-of select="Id"/><br/>
      <strong>Sender:</strong> <span style="word-break:break-all;"><xsl:value-of select="Sender"/></span><br/>
      <strong>Receiver:</strong> <span style="word-break:break-all;"><xsl:value-of select="Receiver"/></span><br/>
      <strong>Amount:</strong> <xsl:value-of select="Amount"/>
    </div>
  </xsl:template>

</xsl:stylesheet>