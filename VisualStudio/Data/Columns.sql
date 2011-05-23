SELECT 
[SC].[name], 
[SC].[system_type_id], 
[max_length], 
[is_nullable], 
[is_identity], 
CAST((CASE OBJECT_DEFINITION([default_object_id]) WHEN '(GETDATE())' THEN 1 WHEN '(NEWID())' THEN 1 WHEN '(NEWSEQUENTIALID())' THEN 1 ELSE 0 END) AS bit) AS [is_auto_generated], 
CASE OBJECT_DEFINITION([default_object_id]) WHEN '(GETDATE())' THEN NULL WHEN '(NEWID())' THEN NULL WHEN '(NEWSEQUENTIALID())' THEN NULL ELSE 
	 REPLACE(REPLACE(OBJECT_DEFINITION([default_object_id]), '(', ''), ')', '') END AS [default_value], 
ISNULL((SELECT TOP (1) [is_primary_key] 
		FROM 
		(
			[sys].[columns] JOIN [sys].[index_columns] ON [sys].[columns].[object_id] = [sys].[index_columns].[object_id] AND 
														  [sys].[columns].[column_id] = [sys].[index_columns].[column_id])
							JOIN [sys].[indexes] ON [sys].[indexes].[index_id] = [sys].[index_columns].[index_id] AND 
													[sys].[indexes].[object_id] = [sys].[index_columns].[object_id] 
			WHERE [sys].[columns].[object_id] = [SC].[object_id] AND 
				  [sys].[columns].[column_id] = [SC].[column_id] AND 
				  [is_primary_key] = 1), 0) AS [is_primary_key] 
FROM (
	SELECT [name], 
	CASE [system_type_id] WHEN 167 THEN 231 WHEN 175 THEN 239 WHEN 35 THEN 99 ELSE [system_type_id] END AS [system_type_id], 
	CASE [user_type_id] WHEN 167 THEN 231 WHEN 175 THEN 239 WHEN 35 THEN 99 ELSE [user_type_id] END AS [user_type_id] 
	FROM [sys].[columns] 
	WHERE [object_id] = OBJECT_ID(N'{0}')
) [__SubQuery__] 
	JOIN [sys].[columns] [SC] ON [__SubQuery__].[name] = [SC].[name] AND 
	                             [object_id] = OBJECT_ID(N'{0}')
ORDER BY name