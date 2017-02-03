/* Da usare per il debug in management studio
DECLARE @ditta varchar(200)
SELECT @ditta = 'SONN'

tb_desgmer: usato per il primo livello del filtro (es:tabcfam.tb_descfam per famiglia)            
tb_dessgme: usato per il secondo livello dl filtro (es:tabmarc.tb_desmarc pmarca)       
              "        ar_confez2,  (sostituito con ar_unmis)                                                                                            " & _
              "        ar_qtacon2,  (sostituito con ar_conver)                                                                                           " & _

ATTENZIONE: SONN ha personalizzato questa query
Se vuoi escludere le famigli aggiungi una oba tipo:  AND tabcfam.tb_codcfam not in ('90', '99')

'nb: escludo gli articoli descrittivi (eccetto 'd'), gli articoli TCO e gli articoli a varianti non movimentabili (la radice della variante)

*/

SELECT 
    ar_codart + CASE WHEN ar_gesfasi = 'S' THEN '.' + Cast(af_fase AS VARCHAR (5)) ELSE '' END AS ar_codart, 
    ar_descr as ar_descr, 
    ar_desint,
    ar_famprod,
    ar_gruppo,
    ar_sotgru,
    ar_unmis, 
    ar_confez2,
	case when ar_qtacon2 is null then 0 else ar_qtacon2 end as ar_qtacon2, 
    ar_unmis2,
    case when ar_conver  is null then 0 else ar_conver end as ar_conver, 
    CASE
        WHEN ar_prevar = '1' THEN ar_codroot + ar_codvar1 
        ELSE ar_codart
    END         AS ar_codartsconti,
    af_descr,
	tb_desgmer as tb_desgmer,     -- Valori master del filtro. livello 1                                                                                            
    tb_dessgme as tb_dessgme,     -- Valori master del filtro. livello 2   
    tb_descfam as tb_descfam,     -- Valore non usato nell'entity. Da usare per le personalizzazioni
	tb_desmarc as tb_desmarc,     -- Valore non usato nell'entity. Da usare per le personalizzazioni
    isnull(ump.tb_desumis, ar_unmis)   as tb_desunmis,
    isnull(um2.tb_desumis, ar_unmis2)  as tb_desunmis2,
    isnull(conf.tb_desumis,ar_confez2) as tb_desconfez2,
    ar_clascon,
    ar_tipo, -- Usato per determitare l'eventuale slip degli ordini (es: cod deperibilita)
	ar_codalt, 
	0 as xx_prz_min_ven,
	0 as xx_sconto_max_ven,
	tb_desgmer   as des_livello1, -- questo campo des_livello 1 sostituiscono quello sopra as tb_desunmis
    tb_dessgme   as des_livello2, -- questo campo des_livello 2 sostituiscono quello sopra as tb_desunmis2
    tb_descfam   as des_livello3, -- questo campo des_livello 3 sostituiscono quello sopra as tb_desconfez2
    ar_umdapr as ar_umdapr, -- Usato per impostare l'unità di misura preferenziale
	ar_ultagg
FROM   artico WITH (NOLOCK)                                                                                               
    LEFT JOIN artfasi WITH (NOLOCK)                                                                                    
            ON artico.codditt = artfasi.codditt                                                                
                AND artico.ar_codart = artfasi.af_codart                                                        
    LEFT JOIN tabcfam WITH (NOLOCK)                                                                                        
            ON artico.codditt = tabcfam.codditt                                                                
                AND artico.ar_famprod = tabcfam.tb_codcfam                                                      
    LEFT JOIN tabgmer WITH (NOLOCK)                                                                                        
            ON artico.codditt = tabgmer.codditt                                                                
                AND artico.ar_gruppo = tabgmer.tb_codgmer                                                       
    LEFT JOIN tabsgme WITH (NOLOCK)                                                                                       
            ON artico.codditt = tabsgme.codditt                                                                
                AND artico.ar_sotgru = tabsgme.tb_codsgme                                                       
        LEFT JOIN tabmarc WITH (NOLOCK)                                                                                        
            ON artico.codditt = tabmarc.codditt                                                                
                AND artico.ar_codmarc = tabmarc.tb_codmarc                                                      
        LEFT JOIN tabumis ump WITH (NOLOCK)                                                                                   
            ON artico.codditt = ump.codditt                                                                    
                AND artico.ar_unmis = ump.tb_codumis                                                            
        LEFT JOIN tabumis um2 WITH (NOLOCK)                                                                                 
            ON artico.codditt = um2.codditt                                                                    
                AND artico.ar_unmis2 = um2.tb_codumis                                                           
        LEFT JOIN tabumis conf WITH (NOLOCK)                                                                                 
            ON artico.codditt = conf.codditt                                                                   
                AND artico.ar_confez2 = conf.tb_codumis    
WHERE  1=1                                                                                                       
    AND ( ar_blocco != 'S')                                                                                   
    AND ( ar_stainv = 'S' OR ar_codart = 'D' )                                                                
    AND ar_codtagl = 0 
  --AND ( ar_gesvar = 'N' OR ( ar_gesvar = 'S' AND ar_codroot <> '' ) )        
    AND ( ar_gesvar <> 'S' OR ( ar_gesvar = 'S' AND ar_codroot <> '' ))
	AND ( ar_codart is not null )
	AND ( ltrim(rtrim(ar_codart)) <> '')
	AND artico.codditt = @ditta