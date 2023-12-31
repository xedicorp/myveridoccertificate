create trigger Cofoundry.PageVersion_CascadeDelete
   on  Cofoundry.PageVersion
   instead of delete
AS 
begin
	set nocount on;
	if not exists (select * from deleted) return
	declare @DefinitionCode char(6) = 'COFPGV'
	
	-- Dependencies
	
    delete Cofoundry.PageVersionBlock
	from Cofoundry.PageVersionBlock e
	inner join deleted d on e.PageVersionId = d.PageVersionId

    delete Cofoundry.PagePublishStatusQuery
	from Cofoundry.PagePublishStatusQuery e
	inner join deleted d on e.PageVersionId = d.PageVersionId

	-- NB: related entity cascade constraints are enforced at the domain layer, so here we just need to clear everything
	delete from Cofoundry.UnstructuredDataDependency
	from Cofoundry.UnstructuredDataDependency e
	inner join deleted d on (e.RootEntityId = d.PageVersionId and RootEntityDefinitionCode = @DefinitionCode) or (e.RelatedEntityId = d.PageVersionId and RelatedEntityDefinitionCode = @DefinitionCode)
	
	-- Main Table
    delete Cofoundry.PageVersion
	from Cofoundry.PageVersion e
	inner join deleted d on e.PageVersionId = d.PageVersionId

end