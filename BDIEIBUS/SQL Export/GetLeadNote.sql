﻿/* Da usare per il debug in management studio
DECLARE @release varchar(200)
SELECT @release = '1.0'
*/

SELECT  
       leads.codditt + '§0§' + CAST(leads.le_codlead AS VARCHAR) AS xx_chiave,
       leads.le_codlead,
	   0 as xx_progressivo,
       'Note generiche' as xx_codnote, 
       leads.le_note2 as xx_desnote,
	   leads.le_ultagg as xx_ultagg
FROM   dbo.leads WITH (NOLOCK)
WHERE  1=1
AND    codditt = @ditta 
AND    le_note IS NOT null
AND    leads.le_note2  IS NOT null
AND    CAST(leads.le_note2 AS VARCHAR)  <> ''
AND    le_status <> 'I' 
AND    leads.le_servdeleted <> 'S'
ORDER BY
	leads.le_codlead
