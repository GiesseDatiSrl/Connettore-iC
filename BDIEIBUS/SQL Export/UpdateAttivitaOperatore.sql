
update cractopp 
set 
	cap_note=@cap_note@, 
	cap_status=@cap_status@, 
	cap_dataes=@cap_dataes@, 
	cap_oraes=@cap_oraes@
where 1=1
and codditt=@codditt@ 
and cap_codcrac=@cap_codcrac@
and cap_opcrmincpr=@cap_opcrmincpr@


-- Scommentare per aggiornare l'attivita principale
/*
update cract 
set 
	ca_note=@cap_note@
where 1=1
and codditt=@codditt@ 
and ca_codcrac=@cap_codcrac@
*/