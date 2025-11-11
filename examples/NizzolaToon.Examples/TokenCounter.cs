using Microsoft.ML.Tokenizers;
using Nizzola.ToonConvert;
using System.Text.Json;

namespace NizzolaToon.Examples;

public static class TokenCounter
{
    public static void CounterTest()
    {
        var data = SampleDataBuilder.GenerateCompanyData();

        var jsonResult = JsonSerializer.Serialize(data);
        Console.WriteLine($"Json Sample \n\n {jsonResult} \n");

        var toonResult = ToonSerializer.Serialize(data);
        Console.WriteLine($"Toon Sample \n\n {toonResult} \n");

        TokenCountComparer(jsonResult, toonResult);
    }

    public static void CounterTest2()
    {
        var data = SampleDataBuilder.GeneratePersonData();

        var jsonResult = JsonSerializer.Serialize(data);
        Console.WriteLine($"Json Sample \n\n {jsonResult} \n");

        var toonResult = ToonSerializer.Serialize(data);
        Console.WriteLine($"Toon Sample \n\n {toonResult} \n");

        TokenCountComparer(jsonResult, toonResult);
    }

    private static void TokenCountComparer(string jsonResult, string toonResult)
    {
        Tokenizer tokenizer = TiktokenTokenizer.CreateForModel("gpt-4o");

        double jsonCounter = tokenizer.CountTokens(jsonResult);
        double toonCounter = tokenizer.CountTokens(toonResult);

        double diference = jsonCounter / toonCounter;
        Console.WriteLine($"Json Tokens:{jsonCounter} - Toon Tokens {toonCounter} - Gain: {(1 - diference) * 100}%");
    }
}