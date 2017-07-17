/* Da usare per il debug in management studio
DECLARE @ditta varchar(200)
SELECT @ditta = 'SONN'
*/

SELECT 
		case when so_codart = ' ' then '' else so_codart end + CASE WHEN so_fase <> 0 THEN '.' + cast(so_fase as varchar(5)) ELSE '' END as ar_codart, 
		tb_descscl, 
		tb_descsar, 
		so_daquant, 
		so_aquant, 
		so_conto, 
		so_clscan, 
		so_clscar, 
		so_codtpro, 
		so_datagg, 
		so_datscad, 
		so_scont1, 
		so_scont2, 
		so_scont3, 
		so_scont4, 
		so_scont5, 
		so_scont6, 
		so_ultagg, 
		CASE WHEN so_codtpro > 0 THEN 
                CASE WHEN so_conto <> 0 THEN 
                        CASE WHEN so_codart <> '' AND so_clscan = 0 AND so_clscar = 0 THEN 1
                        ELSE CASE WHEN so_codart = '' AND so_clscan = 0 AND so_clscar <> 0 THEN 3
                        ELSE CASE WHEN so_codart = '' AND so_clscan = 0 AND so_clscar = 0 THEN 9
                        END END END 
                    ELSE 
                        CASE WHEN so_codart <> '' AND so_clscan <> 0 AND so_clscar = 0 THEN 5
                        ELSE CASE WHEN so_codart = '' AND so_clscan <> 0 AND so_clscar <> 0 THEN 7
                        ELSE CASE WHEN so_codart <> '' AND so_clscan = 0 AND so_clscar = 0 THEN 11
                        END END END 
                    END 
            ELSE 
                CASE WHEN so_conto <> 0 THEN 
                        CASE WHEN so_codart <> '' AND so_clscan = 0 AND so_clscar = 0 THEN 2
                        ELSE CASE WHEN so_codart = '' AND so_clscan = 0 AND so_clscar <> 0 THEN 4
                        ELSE CASE WHEN so_codart = '' AND so_clscan = 0 AND so_clscar = 0 THEN 12 -- ex 10
                        END END END 
                    ELSE 
                        CASE WHEN so_codart <> '' AND so_clscan <> 0 AND so_clscar = 0 THEN 6
                        ELSE CASE WHEN so_codart = '' AND so_clscan <> 0 AND so_clscar <> 0 THEN 8
                        ELSE CASE WHEN so_codart <> '' AND so_clscan = 0 AND so_clscar = 0 THEN 10 -- ex 12
                        END END END 
                    END 
            END as xx_prior1,
    CASE WHEN so_codtpro > 0 THEN 
                CASE WHEN so_conto <> 0 THEN 
                        CASE WHEN so_codart <> '' AND so_clscan = 0 AND so_clscar = 0 THEN 1
                        ELSE CASE WHEN so_codart = '' AND so_clscan = 0 AND so_clscar <> 0 THEN 12
                        ELSE CASE WHEN so_codart = '' AND so_clscan = 0 AND so_clscar = 0 THEN 11
                        END END END 
                    ELSE 
                        CASE WHEN so_codart <> '' AND so_clscan <> 0 AND so_clscar = 0 THEN 3
                        ELSE CASE WHEN so_codart = '' AND so_clscan <> 0 AND so_clscar <> 0 THEN 7
                        ELSE CASE WHEN so_codart <> '' AND so_clscan = 0 AND so_clscar = 0 THEN 9
                        END END END 
                    END 
            ELSE 
                CASE WHEN so_conto <> 0 THEN 
                        CASE WHEN so_codart <> '' AND so_clscan = 0 AND so_clscar = 0 THEN 2
                        ELSE CASE WHEN so_codart = '' AND so_clscan = 0 AND so_clscar <> 0 THEN 6
                        ELSE CASE WHEN so_codart = '' AND so_clscan = 0 AND so_clscar = 0 THEN 5
                        END END END 
                    ELSE 
                        CASE WHEN so_codart <> '' AND so_clscan <> 0 AND so_clscar = 0 THEN 4
                        ELSE CASE WHEN so_codart = '' AND so_clscan <> 0 AND so_clscar <> 0 THEN 8
                        ELSE CASE WHEN so_codart <> '' AND so_clscan = 0 AND so_clscar = 0 THEN 10
                        END END END 
                    END 
            END as xx_prior2,
			getdate() as xx_ultagg  
FROM sconti WITH (NOLOCK)
	LEFT JOIN artico WITH (NOLOCK)
			ON sconti.codditt = artico.codditt 
			AND sconti.so_codart = CASE WHEN artico.ar_prevar = '1' THEN artico.ar_codroot + artico.ar_codvar1 ELSE artico.ar_codart END 
	LEFT JOIN tabcscl WITH (NOLOCK)
			ON	tabcscl.codditt = sconti.codditt AND tabcscl.tb_codcscl = sconti.so_clscan 
	LEFT JOIN tabcsar WITH (NOLOCK)
			ON	tabcsar.codditt = sconti.codditt AND tabcsar.tb_codcsar = sconti.so_clscar 
WHERE 1=1
	AND sconti.codditt = @ditta
	AND so_datagg <= getdate() AND so_datscad >= getdate() 
