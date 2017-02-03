/* Da usare per il debug in management studio
DECLARE @release varchar(200)
SELECT @release = '1.0'
*/

SELECT ar_codart + CASE WHEN ar_gesfasi = 'S' THEN '.' + Cast(af_fase AS VARCHAR (5)) ELSE '' END AS ar_codart,     
        ar_descr,                                                                                                    
        ar_desint,    				                                                                                               
        CASE ISNULL(ar_gif1,'') 
		   WHEN '' THEN 'empty-image.jpg' 
		   ELSE ar_gif1
		END
		    as ar_gif1,  
        af_descr,                                                                                                    
        tb_desgmer     AS xx_l1,                                                                                         
        tb_dessgme     AS xx_l2,                                                                                         
        cast(NULL as varchar)   AS xx_l3,                                                                                        
        cast(NULL as varchar)   AS xx_l4,
		tabcfam.tb_descfam AS tb_descfam,
        tabmarc.tb_desmarc AS tb_desmarc, 
		artico.ar_gruppo   as ar_gruppo,
		getdate() as xx_ultagg
    FROM   artico WITH (NOLOCK)                                                                                                     
        LEFT JOIN artfasi WITH (NOLOCK)                                                                                           
                ON artico.codditt = artfasi.codditt                                                                   
                    AND artico.ar_codart = artfasi.af_codart                                                           
        LEFT JOIN tabgmer WITH (NOLOCK)                                                                                           
                ON artico.codditt = tabgmer.codditt                                                                   
                    AND artico.ar_gruppo = tabgmer.tb_codgmer                                                          
        LEFT JOIN tabsgme WITH (NOLOCK)                                                                                           
                ON artico.codditt = tabsgme.codditt                                                                   
                    AND artico.ar_sotgru = tabsgme.tb_codsgme                                                          
        LEFT JOIN tabcfam WITH (NOLOCK)                                                                                             
                ON artico.codditt = tabcfam.codditt                                                                     
                 	AND artico.ar_famprod = tabcfam.tb_codcfam                                                               
        LEFT JOIN tabmarc WITH (NOLOCK)                                                                                             
                ON artico.codditt = tabmarc.codditt                                                                     
                 	AND artico.ar_codmarc = tabmarc.tb_codmarc                                                             
    WHERE  1=1                                                                                                          
        AND ( ar_stainv = 'S' OR ar_codart = 'D' )                                                                   
        AND ar_codtagl = 0                                                                                           
        AND (ar_blocco != 'S')                                                                                       
        --AND ( ar_gesvar = 'N' OR ( ar_gesvar = 'S' AND ar_codroot <> '' ) )        
        AND ( ar_gesvar <> 'S' OR ( ar_gesvar = 'S' AND ar_codroot <> '' ))   
		AND artico.codditt = @ditta                              
