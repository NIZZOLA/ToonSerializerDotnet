using TOON;

Console.WriteLine("=== TOON.NET - Token-Oriented Object Notation ===\n");

// Definir classes de exemplo
var users = new List<User>
{
    new() { Id = 1, FirstName = "Alice", LastName = "Silva", Interests = new() { "music", "travel" }, Age = 28 },
    new() { Id = 2, FirstName = "Bob", LastName = "Santos", Interests = new() { "coding", "books" }, Age = 34 },
    new() { Id = 3, FirstName = "Carlos", LastName = "Oliveira", Interests = new() { "games", "sports" }, Age = 25 }
};

// Exemplo 1: Serializar lista de objetos
Console.WriteLine("1. SerializaÃ§Ã£o de lista de objetos:");
Console.WriteLine("=====================================");
var toon = ToonSerializer.Serialize(users);
Console.WriteLine(toon);

// Exemplo 2: Converter JSON para TOON
Console.WriteLine("\n2. Converter JSON para TOON:");
Console.WriteLine("=============================");
string json = @"{
  ""users"": [
    { ""id"": 1, ""firstName"": ""Alice"", ""interests"": [""music"", ""travel""] },
    { ""id"": 2, ""firstName"": ""Bob"", ""interests"": [""coding"", ""books""] }
  ]
}";

var toonFromJson = ToonSerializer.FromJson(json);
Console.WriteLine(toonFromJson);

// Exemplo 3: Com opÃ§Ãµes customizadas
Console.WriteLine("\n3. Com opÃ§Ãµes customizadas (snake_case):");
Console.WriteLine("=========================================");
var options = new ToonSerializerOptions
{
    PropertyNamingPolicy = ToonNamingPolicy.SnakeCase,
    IndentSize = 2
};

var customToon = ToonSerializer.Serialize(users, options);
Console.WriteLine(customToon);

// Exemplo 4: Objeto Ãºnico
Console.WriteLine("\n4. Objeto Ãºnico:");
Console.WriteLine("================");
var singleUser = users[0];
var singleToon = ToonSerializer.Serialize(singleUser);
Console.WriteLine(singleToon);

Console.WriteLine("\n=== ConcluÃ­do! ===");

// Classes de modelo
public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
    public List<string> Interests { get; set; } = new();
    
    [ToonIgnore]
    public string InternalField { get; set; } = "ignored";
}
