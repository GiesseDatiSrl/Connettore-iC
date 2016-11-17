/* Da usare per il debug in management studio
DECLARE @release varchar(200)
SELECT @release = '1.0'
*/

SELECT 
	an_tipo, 
	an_conto,
	moviva.mi_codvalu as xx_codvalu,
	Sum(moviva.mi_imponib) * -1 AS xx_fatturato, 
	month(mi_datreg) as xx_mese, 
	Year(mi_datreg) as xx_anno,
	getdate() as xx_ultagg
FROM moviva WITH (NOLOCK)
    INNER JOIN anagra WITH (NOLOCK)
	      ON moviva.codditt = anagra.codditt 
		 AND moviva.mi_contocf = anagra.an_conto 
WHERE 1=1
	AND anagra.codditt =  @ditta
	AND (mi_tregiva=(CASE WHEN an_tipo='F' then 'A' else 'V' END) OR mi_tregiva=(CASE WHEN an_tipo='F' then 'A' else 'C' END)) 
	AND Year(mi_datreg) >=  Year(Getdate()) -2
	AND Year(mi_datreg) <=  Year(Getdate())
	AND mi_tipoivaed <> 'P'
	AND an_tipo <> 'S'
	AND an_status = 'A'
GROUP BY 
	an_tipo, 
	an_conto, 
	moviva.mi_codvalu,
	Year(mi_datreg), 
	month(mi_datreg)
ORDER BY 
	an_tipo, 
	an_conto,
	moviva.mi_codvalu,
	Year(mi_datreg), 
	month(mi_datreg)           