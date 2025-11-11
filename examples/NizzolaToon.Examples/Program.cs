using Nizzola.ToonConvert;
using NizzolaToon.Examples;
/*
var serv = new AIAssertivityTests();

await serv.ValidateAIUnderstanding1();
await serv.ValidateAIUnderstanding2();


TokenCounter.CounterTest();
Console.Clear();
TokenCounter.CounterTest2();
*/

// Exemplo 1: Serializar objeto direto
var users = new List<User>
{
    new() { Id = 1, Name = "Alice" },
    new() { Id = 2, Name = "Bob" }
};

var json1 = System.Text.Json.JsonSerializer.Serialize(users);
Console.WriteLine(json1);

var toon = ToonSerializer.Serialize(users);
Console.WriteLine("\n \n" + toon);

var users2 = new List<User2>
{
    new() { Id = 1, FirstName = "Alice", Interests = new() { "music", "travel" } },
    new() { Id = 2, FirstName = "Bob", Interests = new() { "coding", "books" } }
};

var toon2 = ToonSerializer.Serialize(users2);
Console.WriteLine(toon2);

// Saída:
// 2
// Id  FirstName  Interests
// 1   Alice      music, travel
// 2   Bob        coding, books

// Exemplo 2: Converter de JSON para TOON
string json = @"{
  ""users"": [
    { ""id"": 1, ""firstName"": ""Alice"", ""interests"": [""music"", ""travel""] },
    { ""id"": 2, ""firstName"": ""Bob"", ""interests"": [""coding"", ""books""] }
  ]
}";

var toonFromJson = ToonSerializer.FromJson(json);
Console.WriteLine(toonFromJson);

// Exemplo 3: Com opções personalizadas
var options = new ToonSerializerOptions
{
    PropertyNamingPolicy = ToonNamingPolicy.SnakeCase,
    IndentSize = 4,
    IgnoreNullValues = true
};

var customToon = ToonSerializer.Serialize(users, options);
Console.WriteLine(customToon);

// Exemplo 4: Async para streams
await using var stream = File.Create("output.toon");
await ToonSerializer.SerializeAsync(stream, users);

Console.ReadKey();