# Maxsys.Core.Data (Principais Features)

## Extensions (Principais)

### IQueryableExtensions
- `.LeftOuterJoin()/LeftOuterJoinList()`
	<details>
		<summary>Exemplos de uso</summary>

	```cs
	public class Location
	{
		public int Id { get; set; }
		public int CountryId { get; set; }
		public string Name { get; set; }

		public Country Country { get; set; }
	}

	public class Country
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	var a = locations.LeftOuterJoin(
		countries,
		location => location.CountryId,
		country => country.Id,
		(outer, inner) => new { Location = outer, Country = inner }); // Country nulável

	var b = countries.LeftOuterJoinList(
		location,
		country => country.Id,
		location => location.CountryId,
		(outer, innerList) => new { Country = outer, Locations = innerList }) // IEnumerable<Location>
	```
	</details>



## Features (Principais)

### RepositoryBase&lt;T&gt;

+ `RepositoryBase<T>.GetById()`
	+ Obtém uma entidade a partir de seu id ou null se não encontrada.
	⚠️ Devido a utilização do método `DbSet.FindAsync()` internamente, não é possível fazer joins (`.Include()`).
	
	<details>
		<summary>Exemplo de uso (chave única)</summary>

	```cs
	public class Location
	{
		public int Id { get; set; }        // Primary Key
		public int CountryId { get; set; } // Foreign Key
		public string Name { get; set; }

		public Country Country { get; set; }
	}

	public class Country
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	var id = Guid.Parse("E416E615-EC3F-4246-A4E8-7497C2CAE81C");
	var location = await _locationRespository.GetByIdAsync(id, cancellationToken);

	Console.WriteLine(location.Id);             // E416E615-EC3F-4246-A4E8-7497C2CAE81C
	Console.WriteLine(location.CountryId);      // algum id, ex.: C9D8D494-A044-4D7D-B4BD-C6BB13FBE2A8
	Console.WriteLine(location.Name);           // Rio de Janeiro
	Console.WriteLine(location.Country);        // null
	Console.WriteLine(location.Country?.Name);  // null
	
	```
	</details>

	<details>
		<summary>Exemplo de uso (chave múltipla)</summary>

	```cs
	public class User
	{
		public int Id { get; set; }          // Primary Key
		public int WorkspaceId { get; set; } // Primary Key

		public string Name { get; set; }
		public string Email { get; set; }
	}

	var userId = Guid.Parse("D9BC701C-A22C-459B-AF91-B858B8571E38");
	var workspaceId = Guid.Parse("01494768-6A83-401A-882F-19CB40184DBF");

	var multipleId = new object[] { userId, workspaceId };

	var user = await _userRepository.GetByIdAsync(multipleId, cancellationToken);

	Console.WriteLine(user.Id);          // D9BC701C-A22C-459B-AF91-B858B8571E38
	Console.WriteLine(user.WorkspaceId); // 01494768-6A83-401A-882F-19CB40184DBF
	Console.WriteLine(user.Name);        // Eolam Bipal
	Console.WriteLine(user.Email);       // eolam.bipal@india.com
	
	```
	</details>