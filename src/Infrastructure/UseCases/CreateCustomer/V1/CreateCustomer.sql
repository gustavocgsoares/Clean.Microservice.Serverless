SET NOCOUNT ON

INSERT INTO	
	Customer
	(
		[Name],
		[BirthDate],
		[Email],
		[Phone],
		[CreatedAt]
	)
OUTPUT 
	inserted.Id
VALUES
	(
		@Name,
		@BirthDate,
		@Email,
		@Phone,
		@CreatedAt
	)