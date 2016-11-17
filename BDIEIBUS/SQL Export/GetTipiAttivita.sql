/* Da usare per il debug in management studio
DECLARE @release varchar(200)
SELECT @release = '1.0'
*/

select 
  codditt + '§' + cast(tb_codtaco as varchar) as CHIAVE,
  tb_codtaco  as CODICE,
  tb_destaco  as DESCRIZIONE,
  tb_prevcons as FLG_PREVISIONALE
from 
  tabtaco WITH (NOLOCK)
where 1=1
and codditt = @ditta
order by 
  tb_codtaco

