SET NOCOUNT ON

SELECT
	[Id],
	[Name],
	[BirthDate],
	[Email],
	[Phone]
  FROM Customer
 WHERE Id = @Id
