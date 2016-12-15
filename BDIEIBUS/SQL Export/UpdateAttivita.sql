update cract 
set 
	ca_oraes=@ca_oraes@, 
	ca_dataes=@ca_dataes@, 
	ca_status=@ca_status@
where 1=1
and codditt=@codditt@ 
and ca_codcrac=@ca_codcrac@

