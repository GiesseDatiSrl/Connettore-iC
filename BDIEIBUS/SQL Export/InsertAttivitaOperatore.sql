INSERT INTO [cractopp]
           ([codditt]
           ,[cap_codcrac]
           ,[cap_opcrmincpr]
           ,[cap_status]
           ,[cap_dataes]
           ,[cap_oraes]
           ,[cap_tempes]
           ,[cap_note])
     VALUES
           (
            @codditt@
           ,@cap_codcrac@
           ,@cap_opcrmincpr@
           ,@cap_status@
           ,@cap_dataes@
           ,@cap_oraes@
           ,@cap_tempes@
           ,@cap_note@
           )


-- Scommentare il seguente esempio per aggiornare le note di testata.
/*
update cract 
set ca_note=@cap_note@
where 1=1
and codditt = @codditt@ 
and ca_codcrac = @cap_codcrac@
*/