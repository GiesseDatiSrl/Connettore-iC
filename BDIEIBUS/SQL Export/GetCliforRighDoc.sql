﻿-- Da usare per il debug in management studio
-- DECLARE @ditta varchar(200), @tipocf varchar(2), @conti_esclusi varchar(10), @gg_documenti varchar(10)
-- SELECT @ditta = 'CMATIC', @tipocf ='CF', @conti_esclusi= '0', @gg_documenti ='180'
-- CAST(testmag.tm_conto AS VARCHAR) + '§' + CAST(testmag.tm_numdoc AS VARCHAR) + '§' + testmag.codditt as xx_numreg,                                                    
-- CAST(testmag.tm_conto AS VARCHAR) + '§' + CAST(testmag.tm_numdoc AS VARCHAR) + '§' + testmag.codditt as xx_numreg,                                                                                                         
-- CAST(testord.td_conto AS VARCHAR) + '§' + CAST(testord.td_numord AS VARCHAR) + '§' + testord.codditt as xx_numreg,                                                                                                 

-- righe
-- fatture immediate attive e note di credito attive
-- fatture immediate PASSIVE e note di credito PASSIVE
-- altri documenti di magazzino
-- fatture differite attive (vado su movmag)
-- fatture differite PASSIVE (vado su movmag)
-- Estraggo le righe ordine (vado su movord)

SELECT   
        an_tipo,                                                          
        an_conto,     
		testmag.codditt + '§' + CAST(tm_conto AS VARCHAR)+ '§' + CAST(tm_anno as varchar) + '§' + tm_serie + '§' + CAST(tm_numdoc as varchar) + '§' + CAST(tm_numpar as varchar) as xx_numreg,
        testmag.tm_tipork,                                                
        testmag.tm_anno,                                                  
        testmag.tm_serie,                                                 
        testmag.tm_numdoc,                                                
            0 as tm_numdoc1,                                                  
        mm_riga,                                                          
        mm_codart,                                                        
        mm_fase,                                                          
        mm_descr,                                                         
        mm_desint,                                                        
        mm_unmis,                                                         
        mm_ump,                                                           
        mm_quant,                                                         
        mm_colli,                                                         
        mm_valore,
        CASE                                                              
            WHEN mm_quant <> 0 THEN Round(mm_valore / mm_quant, 4)          
            ELSE 0                                                          
        END AS xx_prezzo,
		tm_ultagg as xx_ultagg                                                  
    FROM   testmag WITH (NOLOCK)                                            
        INNER JOIN movmag b WITH (NOLOCK)                                 
                ON b.codditt = testmag.codditt                            
                AND b.mm_tipork = testmag.tm_tipork                       
                AND b.mm_anno = testmag.tm_anno                           
                AND b.mm_serie = testmag.tm_serie                         
                AND b.mm_numdoc = testmag.tm_numdoc                       
        INNER JOIN anagra WITH (NOLOCK)                                   
                ON testmag.codditt = anagra.codditt                       
                AND testmag.tm_conto = anagra.an_conto                    
    WHERE  anagra.codditt =  @ditta
        AND an_tipo <> 'S'                                                
        AND an_status = 'A'                                              
		AND ((an_tipo = 'C' and @tipocf = 'C') or (an_tipo = 'F' and @tipocf = 'F') or (an_tipo <> 'S' and @tipocf = 'CF'))              
		AND    cast((GETDATE() - testmag.tm_datdoc ) as integer) < cast(@gg_documenti as integer)   
		AND tm_tipork in ('A','N')  
UNION ALL
	SELECT   
        an_tipo,                                                          
        an_conto,     
		testmag.codditt + '§' + CAST(tm_conto AS VARCHAR)+ '§' + CAST(testmag.tm_annpar as varchar) + '§' + testmag.tm_alfpar + '§' + CAST(testmag.tm_numdoc as varchar) + '§' + CAST(testmag.tm_numpar as varchar) as xx_numreg,
        testmag.tm_tipork,                                                
        testmag.tm_anno,                                                  
        testmag.tm_serie,                                                 
        testmag.tm_numdoc,                                                
            0 as tm_numdoc1,                                                  
        mm_riga,                                                          
        mm_codart,                                                        
        mm_fase,                                                          
        mm_descr,                                                         
        mm_desint,                                                        
        mm_unmis,                                                         
        mm_ump,                                                           
        mm_quant,                                                         
        mm_colli,                                                         
        mm_valore, 
        CASE                                                              
            WHEN mm_quant <> 0 THEN Round(mm_valore / mm_quant, 4)          
            ELSE 0                                                          
        END AS xx_prezzo,
		tm_ultagg as xx_ultagg                                                  
    FROM   testmag WITH (NOLOCK)                                            
        INNER JOIN movmag b WITH (NOLOCK)                                 
                ON b.codditt = testmag.codditt                            
                AND b.mm_tipork = testmag.tm_tipork                       
                AND b.mm_anno = testmag.tm_anno                           
                AND b.mm_serie = testmag.tm_serie                         
                AND b.mm_numdoc = testmag.tm_numdoc                       
        INNER JOIN anagra WITH (NOLOCK)                                   
                ON testmag.codditt = anagra.codditt                       
                AND testmag.tm_conto = anagra.an_conto                    
    WHERE  anagra.codditt =  @ditta
        AND an_tipo <> 'S'                                                
        AND an_status = 'A'                                              
		AND ((an_tipo = 'C' and @tipocf = 'C') or (an_tipo = 'F' and @tipocf = 'F') or (an_tipo <> 'S' and @tipocf = 'CF'))            
		AND    cast((GETDATE() - testmag.tm_datdoc ) as integer) < cast(@gg_documenti as integer)   
		AND tm_tipork in ('L','J') 
		AND an_tipo = 'F' 
	    AND testmag.tm_annpar > 0 AND testmag.tm_numpar > 0
UNION ALL
	SELECT   
        an_tipo,                                                          
        an_conto,     
		testmag.codditt + '§' + CAST(tm_conto AS VARCHAR)+ '§' + CAST(tm_anno as varchar) + '§' + tm_serie + '§' + CAST(tm_numdoc as varchar) + '§' + CAST(tm_tipork as varchar) as xx_numreg,
        testmag.tm_tipork,                                                
        testmag.tm_anno,                                                  
        testmag.tm_serie,                                                 
        testmag.tm_numdoc,                                                
            0 as tm_numdoc1,                                                  
        mm_riga,                                                          
        mm_codart,                                                        
        mm_fase,                                                          
        mm_descr,                                                         
        mm_desint,                                                        
        mm_unmis,                                                         
        mm_ump,                                                           
        mm_quant,                                                         
        mm_colli,                                                         
        mm_valore,
        CASE                                                              
            WHEN mm_quant <> 0 THEN Round(mm_valore / mm_quant, 4)          
            ELSE 0                                                          
        END AS xx_prezzo,
		tm_ultagg as xx_ultagg                                                  
    FROM   testmag WITH (NOLOCK)                                            
        INNER JOIN movmag b WITH (NOLOCK)                                 
                ON b.codditt = testmag.codditt                            
                AND b.mm_tipork = testmag.tm_tipork                       
                AND b.mm_anno = testmag.tm_anno                           
                AND b.mm_serie = testmag.tm_serie                         
                AND b.mm_numdoc = testmag.tm_numdoc                       
        INNER JOIN anagra WITH (NOLOCK)                                   
                ON testmag.codditt = anagra.codditt                       
                AND testmag.tm_conto = anagra.an_conto                    
    WHERE  anagra.codditt =  @ditta
        AND an_tipo <> 'S'                                                
        AND an_status = 'A'                                              
		AND ((an_tipo = 'C' and @tipocf = 'C') or (an_tipo = 'F' and @tipocf = 'F') or (an_tipo <> 'S' and @tipocf = 'CF'))  
		AND    cast((GETDATE() - testmag.tm_datdoc ) as integer) < cast(@gg_documenti as integer)   
		AND tm_tipork not in ('L','K','J','A','D','N','Z','T','U')
UNION ALL        
	SELECT                                                                
		an_tipo,                                                          
		an_conto,    
		testmag.codditt + '§' + CAST(testmag.tm_conto AS VARCHAR)+ '§' + CAST(testmag.tm_anno as varchar) + '§' + testmag.tm_serie + '§' + CAST(testmag.tm_numdoc as varchar) + '§' + CAST(testmag.tm_numpar as varchar)  as xx_numreg, 
		testmag.tm_tipork,                                                
		testmag.tm_anno,                                                  
		testmag.tm_serie,                                                 
		testmag.tm_numdoc,                                                
		testmag_1.tm_numdoc as tm_numdoc1,                                
		mm_riga,                                                          
		mm_codart,                                                        
		mm_fase,                                                          
		mm_descr,                                                         
		mm_desint,                                                        
		mm_unmis,                                                         
		mm_ump,                                                           
		mm_quant,                                                         
		mm_colli,                                                         
		mm_valore,
		CASE                                                              
			WHEN mm_quant <> 0 THEN Round(mm_valore / mm_quant, 4)          
			ELSE 0                                                          
		END AS xx_prezzo,
	testmag.tm_ultagg as xx_ultagg                                                     
	FROM testmag WITH (NOLOCK)                                            
	INNER JOIN testmag AS testmag_1 WITH (NOLOCK)                       
				ON (testmag.tm_tipork = testmag_1.tm_tiporkfat)         
             		AND (testmag.tm_anno = testmag_1.tm_annfat)                 
             		AND (testmag.tm_serie = testmag_1.tm_alffat)                
             		AND (testmag.tm_numdoc = testmag_1.tm_numfat)               
             		AND (testmag.codditt = testmag_1.codditt)                   
		INNER JOIN movmag WITH (NOLOCK)                                   
				ON (testmag_1.tm_numdoc = movmag.mm_numdoc)             
             		AND (testmag_1.tm_serie = movmag.mm_serie)                  
             		AND (testmag_1.tm_anno = movmag.mm_anno)                    
             		AND (testmag_1.tm_tipork = movmag.mm_tipork)                
             		AND (testmag_1.codditt = movmag.codditt)                    
		INNER JOIN anagra WITH (NOLOCK)                                
				ON testmag.codditt = anagra.codditt                           
             		AND testmag.tm_conto = anagra.an_conto                      
	WHERE  anagra.codditt =  @ditta
		AND an_tipo <> 'S'                                             
		AND an_status = 'A'                                           
	AND ((an_tipo = 'C' and @tipocf = 'C') or (an_tipo = 'F' and @tipocf = 'F') or (an_tipo <> 'S' and @tipocf = 'CF'))  
		AND   cast( (GETDATE() - testmag.tm_datdoc ) as integer) < cast(@gg_documenti as integer)    
		AND testmag.tm_tipork = 'D'   
UNION ALL
    SELECT                                                                
        an_tipo,                                                          
        an_conto,    
		testmag.codditt + '§' + CAST(testmag.tm_conto AS VARCHAR)+ '§' + CAST(testmag.tm_annpar as varchar) + '§' + testmag.tm_alfpar + '§' + CAST(testmag.tm_numdoc as varchar)+ '§' + CAST(testmag.tm_numpar as varchar)  +  '§' + testmag.tm_tipork as xx_numreg,
        testmag.tm_tipork,                                                
        testmag.tm_anno,                                                  
        testmag.tm_serie,                                                 
        testmag.tm_numdoc,                                                
        testmag_1.tm_numdoc as tm_numdoc1,                                
        mm_riga,                                                          
        mm_codart,                                                        
        mm_fase,                                                          
        mm_descr,                                                         
        mm_desint,                                                        
        mm_unmis,                                                         
        mm_ump,                                                           
        mm_quant,                                                         
        mm_colli,                                                         
        mm_valore, 
        CASE                                                              
            WHEN mm_quant <> 0 THEN Round(mm_valore / mm_quant, 4)          
            ELSE 0                                                          
        END AS xx_prezzo,
	testmag.tm_ultagg as xx_ultagg                                                     
    FROM testmag WITH (NOLOCK)                                            
    INNER JOIN testmag AS testmag_1 WITH (NOLOCK)                       
             	ON (testmag.tm_tipork = testmag_1.tm_tiporkfat)         
             		AND (testmag.tm_anno = testmag_1.tm_annfat)                 
             		AND (testmag.tm_serie = testmag_1.tm_alffat)                
             		AND (testmag.tm_numdoc = testmag_1.tm_numfat)               
             		AND (testmag.codditt = testmag_1.codditt)                   
        INNER JOIN movmag WITH (NOLOCK)                                   
             	ON (testmag_1.tm_numdoc = movmag.mm_numdoc)             
             		AND (testmag_1.tm_serie = movmag.mm_serie)                  
             		AND (testmag_1.tm_anno = movmag.mm_anno)                    
             		AND (testmag_1.tm_tipork = movmag.mm_tipork)                
             		AND (testmag_1.codditt = movmag.codditt)                    
        INNER JOIN anagra WITH (NOLOCK)                                
             	ON testmag.codditt = anagra.codditt                           
             		AND testmag.tm_conto = anagra.an_conto                      
    WHERE  anagra.codditt =  @ditta
        AND an_tipo <> 'S'                                             
        AND an_status = 'A'                                           
    AND ((an_tipo = 'C' and @tipocf = 'C') or (an_tipo = 'F' and @tipocf = 'F') or (an_tipo <> 'S' and @tipocf = 'CF'))  
		AND   cast( (GETDATE() - testmag.tm_datdoc ) as integer) < cast(@gg_documenti as integer)    
        AND testmag.tm_tipork = 'K'
		AND anagra.an_tipo = 'F' 
	    AND testmag.tm_annpar > 0 AND testmag.tm_numpar > 0                       
UNION ALL 
    SELECT                                                                  
            an_tipo,                                                         
            an_conto,  
			testord.codditt + '§' + CAST(td_conto AS VARCHAR)+ '§' + CAST(td_anno as varchar) + '§' + td_serie + '§' + CAST(td_numord as varchar) + '§' + td_tipork as xx_numreg,
            testord.td_tipork,                                               
            testord.td_anno,                                                 
            testord.td_serie,                                                
            testord.td_numord,                                               
           	0 as tm_numdoc1,                                                 
            mo_riga,                                                         
            mo_codart,                                                       
            mo_fase,                                                         
            mo_descr,                                                        
            mo_desint,                                                       
            mo_unmis,                                                        
            mo_ump,                                                          
            mo_quant,                                                        
            mo_colli,                                                        
            mo_valoremm, 
            CASE                                                             
            WHEN mo_quant <> 0 THEN Round(mo_valoremm / mo_quant, 4)       
            ELSE 0                                                         
            END AS xx_prezzo,
			testord.td_ultagg as xx_ultagg                                                    
    FROM    testord WITH (NOLOCK)                                            
            INNER JOIN movord WITH (NOLOCK)                                  
                    ON movord.codditt = testord.codditt                      
                    AND movord.mo_tipork = testord.td_tipork                 
                    AND movord.mo_anno = testord.td_anno                     
                    AND movord.mo_serie = testord.td_serie                   
                    AND movord.mo_numord = testord.td_numord                 
            INNER JOIN anagra WITH (NOLOCK)                                  
                    ON testord.codditt = anagra.codditt                      
                    AND testord.td_conto = anagra.an_conto                   
    WHERE  anagra.codditt =  @ditta
            AND an_tipo <> 'S'                                               
            AND an_status = 'A'                                             
    AND ((an_tipo = 'C' and @tipocf = 'C') or (an_tipo = 'F' and @tipocf = 'F') or (an_tipo <> 'S' and @tipocf = 'CF'))  
            AND testord.td_tipork NOT IN ('X', 'H', 'Y')     
			AND    cast((GETDATE() - testord.td_datord ) as integer) < cast(@gg_documenti as integer) 