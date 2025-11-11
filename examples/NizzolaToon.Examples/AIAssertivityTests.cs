using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using Nizzola.ToonConvert;
using OpenAI;
using System.Text.Json;

namespace NizzolaToon.Examples;

public class AIAssertivityTests
{
    private static string aiUrl = "<your token here>";
    private static string aiModel = "gpt-4o";

    internal AIAgent agent = new AzureOpenAIClient(
        new Uri(aiUrl),
        new DefaultAzureCredential())
        .GetChatClient(aiModel)
        .CreateAIAgent(instructions: "You are an analyst checking data and anwering.");

    public async Task ValidateAIUnderstanding1()
    {
        var data = SampleDataBuilder.GenerateCompanyData();

        var jsonString = JsonSerializer.Serialize(data);

        var toonString = ToonSerializer.Serialize(data);

        var prompt = "Based on data, what is the role and skills of Bob, your response need to be objective and only need to return role and skills, here are my data:";

        var jsonPrompt = prompt + jsonString;
        var toonPrompt = prompt + toonString;

        await ExecuteComparison(jsonPrompt, toonPrompt);
    }


    public async Task ValidateAIUnderstanding2()
    {
        var data = SampleDataBuilder.GeneratePersonData();

        var jsonString = JsonSerializer.Serialize(data);

        var toonString = ToonSerializer.Serialize(data);

        var prompt = "Based on data, what is the address and phone of Dave ? your response need to be objective and only need to return information requested, here are my data:";

        var jsonPrompt = prompt + jsonString;
        var toonPrompt = prompt + toonString;

        await ExecuteComparison(jsonPrompt, toonPrompt);
    }

    private async Task ExecuteComparison(string jsonPrompt, string toonPrompt)
    {
        var jsonResponse = await agent.RunAsync(jsonPrompt);
        Console.WriteLine("\nThis is the response for json:\n");
        Console.WriteLine(jsonResponse);
        Console.WriteLine($"\nCounters - Input:{jsonResponse.Usage.InputTokenCount} - {jsonResponse.Usage.TotalTokenCount}");

        var toonResponse = await agent.RunAsync(toonPrompt);
        Console.WriteLine("\nThis is the response for toon:\n");
        Console.WriteLine(toonResponse);
        Console.WriteLine($"\nCounters - Input:{toonResponse.Usage.InputTokenCount} - {toonResponse.Usage.TotalTokenCount}");

        var diference = Convert.ToDouble(jsonResponse.Usage.TotalTokenCount - toonResponse.Usage.TotalTokenCount);
        var percent = diference / Convert.ToDouble(jsonResponse.Usage.TotalTokenCount) * 100;

        Console.WriteLine($"\nYou consume {(jsonResponse.Usage.TotalTokenCount - toonResponse.Usage.TotalTokenCount)} less tokens - {percent}% reduction");
    }
}
