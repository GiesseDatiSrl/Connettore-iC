/* Da usare per il debug in management studio
DECLARE @ditta varchar(200)
SELECT @ditta = 'SONN'
*/

SELECT 
    ar_codart + CASE WHEN ar_gesfasi = 'S' THEN '.' + Cast(af_fase AS VARCHAR(5)) ELSE '' END AS ar_codart,   
    ar_unmis,  
    ap_magaz,  
    ap_esist,  
    ap_ordin,  
    ap_impeg,  
    ap_prenot,  
    tb_desmaga,
	getdate() as xx_ultagg  
FROM artico WITH (NOLOCK) 
		LEFT JOIN artfasi WITH (NOLOCK)
				ON artico.codditt = artfasi.codditt  
			AND artico.ar_codart = artfasi.af_codart   
		INNER JOIN artpro WITH (NOLOCK)
				ON artico.codditt = artpro.codditt   
			AND artico.ar_codart =  artpro.ap_codart   
			AND CASE WHEN artico.ar_gesfasi = 'S' THEN Cast(artfasi.af_fase AS VARCHAR(5)) ELSE '' END =   CASE WHEN artico.ar_gesfasi = 'S' THEN  Cast(artpro.ap_fase AS VARCHAR(5)) ELSE '' END   
		INNER JOIN tabmaga WITH (NOLOCK)
				ON artpro.codditt = tabmaga.codditt  
			AND artpro.ap_magaz = tabmaga.tb_codmaga
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
WHERE 1=1
    AND artico.codditt = @ditta
	AND ( ar_blocco != 'S')
    AND ( ar_stainv = 'S' OR ar_codart = 'D' )  
    AND ar_codtagl = 0   
    --AND ( ar_gesvar = 'N'  OR ( ar_gesvar = 'S'  AND ar_codroot <> '' ) ) 
	AND ( ar_gesvar <> 'S' OR ( ar_gesvar = 'S' AND ar_codroot <> '' ))

