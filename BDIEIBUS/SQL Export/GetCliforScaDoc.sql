/* Da usare per il debug in management studio
DECLARE @ditta varchar(200), @tipocf varchar(2), @conti_esclusi varchar(10), @anni integer
SELECT @ditta = 'SONN' , @tipocf='C', @conti_esclusi = '0', @anni = 0
*/
-- scadenze
SELECT
    upper(scaden.codditt + '§' + CAST(sc_conto AS VARCHAR)+ '§' + CAST(sc_annpar as varchar) + '§' + sc_alfpar + '§' + CAST(sc_numdoc as varchar)) as xx_numreg,
   -- scaden.codditt + '§' + CAST(sc_conto AS VARCHAR)+ '§' + CAST(sc_annpar as varchar) + '§' + sc_alfpar + '§' + CAST(sc_numdoc as varchar)  + '§' + CAST(sc_numpar as varchar) as xx_numreg,
	an_tipo,
	an_conto,
	sc_annpar,
	scaden.codditt,
	sc_alfpar,
	sc_numpar,
	sc_numrata,
	sc_datsca,
	CASE sc_codvalu WHEN 0 THEN sc_importo ELSE sc_impval END AS sc_importo,
	sc_integr,
	sc_flsaldato,
	case sc_flsaldato
	   when 'S' then 'Chiuso'
	   else 'Aperto'
	end as xx_flsaldato,
	sc_tippaga,
	case sc_tippaga
	   when 1 then 'Tratta'
	   when 2 then 'Riba'
	   when 3 then 'Rim.dir.'
	   when 4 then 'Contanti'
	   when 5 then 'Accr.banc.'
	end as xx_tippaga,
	case sc_flsaldato
	   when 'S' then LTRIM(ISNULL(tabpaga.tb_despaga,'n.d.'))
	   else ltrim(ISNULL(tabpaga.tb_despaga,'n.d.') + CASE sc_insolu when 'S' then ' (Insol.)' else '' end)
	end AS tb_despaga,
	sc_banc1,
	sc_banc2,
	sc_insolu,
	sc_datdoc,
	sc_numdoc,
	sc_annpar,
	sc_codvalu,
	sc_alfpar,
	sc_numpar,
	'Fattura' + ' ' + ISNULL(sc_descr,'') as xx_des_scad,
	getdate() as xx_ultagg
FROM  
    scaden WITH (NOLOCK)
	   INNER JOIN anagra WITH (NOLOCK) 
	       ON scaden.codditt = anagra.codditt
		   AND scaden.sc_conto = anagra.an_conto
	   LEFT JOIN tabpaga  WITH (NOLOCK) 
	       ON  dbo.scaden.sc_codpaga = dbo.tabpaga.tb_codpaga
WHERE  1=1
    AND anagra.codditt =  @ditta
    AND scaden.codditt = anagra.codditt
    AND scaden.sc_conto = anagra.an_conto
    AND an_tipo <> 'S'
    AND an_status = 'A'
	-- AND sc_codincdiff = 0 Se si gestiscono incassi differiti usare questo filtro
    AND sc_annpar > 0 
	-- AND sc_numpar > 0
	    AND ( ( an_tipo = 'C' AND @tipocf = 'C' ) OR ( an_tipo = 'F'  AND @tipocf = 'F' ) OR ( an_tipo <> 'S'  AND @tipocf = 'CF' ) )
        AND
        (
         	( sc_flsaldato = 'N' )
         	or
         	( sc_annpar >=  (YEAR(getdate()) - cast('2' as integer)) and  sc_flsaldato = 'S')
        )