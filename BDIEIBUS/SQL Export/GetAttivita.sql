select
  A.codditt + '§' + cast(A.ca_codcrac as varchar) + '§' + B.cap_opcrmincpr as CHIAVE,
  A.codditt as COD_DITTA,
  cast(A.ca_codcrac as varchar)  as COD_ATTIVITA,
  A.ca_codlead  as COD_LEAD,
  A.ca_codtaco  as COD_TIPO_ATTIVITA,
  A.ca_oggetto  as DES_OGGETTO,
  A.ca_status   as COD_STATO_ATTIVITA,
  isnull(A.ca_dataprev, A.ca_dataes) as DATA_PREVISTA_ATTIVITA,
  case A.ca_oraprev when 0 then A.ca_oraes else A.ca_oraprev end  as ORA_PREVISTA_ATTIVITA,
  A.ca_note     as DES_NOTE_ATTIVITA,
  B.cap_opcrmincpr as COD_OPERATORE,
  B.cap_status as COD_STATO,
  B.cap_dataes as DATA_ESECUZIONE,
  B.cap_oraes  as ORA_ESECUZIONE,
  B.cap_note as DES_NOTE,
  A.ca_ultagg as DAT_ULT_MOD
from
  cract A,
  cractopp B
where 1=1
and A.ca_codcrac = B.cap_codcrac
and A.codditt = B.codditt
AND A.codditt = @ditta
and A.ca_codtaco <> 0
