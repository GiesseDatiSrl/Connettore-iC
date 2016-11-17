select
  A.codditt + '§' + cast(A.ca_codcrac as varchar) + '§' + B.cap_opcrmincpr as CHIAVE,
  A.codditt as COD_DITTA,
  cast(A.ca_codcrac as varchar)  as COD_ATTIVITA,
  A.ca_codlead as COD_LEAD,
  A.ca_codtaco as COD_TIPO_ATTIVITA,
  A.ca_oggetto as DES_OGGETTO,
  A.ca_dataprev as DATA_PREVISTA,
  A.ca_oraprev  as ORA_PREVISTA,
  A.ca_note     as DES_NOTE_ATTIVITA,
  cast(B.cap_codcrac as varchar) +  '-' + B.cap_opcrmincpr  as COD_ATTIVITA_OPERATORE,
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
