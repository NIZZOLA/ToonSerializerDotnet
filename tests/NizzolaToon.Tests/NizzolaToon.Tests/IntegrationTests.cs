using Nizzola.ToonConvert;
using Shouldly;

namespace NizzolaToon.Tests;

public class IntegrationTests
{
    [Fact]
    public void CompleteWorkflow_SerializeAndCompare_ProducesExpectedOutput()
    {
        // Arrange
        var data = new CompanyData
        {
            CompanyName = "TechCorp",
            Employees = new List<Employee>
            {
                new() { Id = 1, Name = "Alice", Department = "Engineering", Skills = new() { "C#", "Azure" } },
                new() { Id = 2, Name = "Bob", Department = "Sales", Skills = new() { "Communication", "CRM" } },
                new() { Id = 3, Name = "Carol", Department = "Engineering", Skills = new() { "Python", "ML" } }
            }
        };

        // Act
        var result = ToonSerializer.Serialize(data);

        // Assert
        result.ShouldContain("companydata{CompanyName,Employees}:");
        result.ShouldContain("TechCorp");
        result.ShouldContain("employees[3]{Department,Id,Name,Skills}:");
        result.ShouldContain("1,Alice");
        result.ShouldContain("[C#,Azure]");
        result.ShouldContain("Bob");
        result.ShouldContain("Sales");
    }

    [Fact]
    public void RealWorldScenario_LargeDataset_ProcessesEfficiently()
    {
        // Arrange
        var orders = Enumerable.Range(1, 1000)
            .Select(i => new Order
            {
                OrderId = i,
                CustomerName = $"Customer{i}",
                Amount = i * 10.5m,
                Items = new List<string> { $"Item{i}A", $"Item{i}B" }
            })
            .ToList();

        // Act
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var result = ToonSerializer.Serialize(orders);
        stopwatch.Stop();

        // Assert
        result.ShouldContain("orders[1000]{Amount,CustomerName,Items,OrderId}:");
        stopwatch.ElapsedMilliseconds.ShouldBeLessThan(1000);
        result.Length.ShouldBeLessThan(100000);
    }

    [Fact]
    public void CompareWithJson_TokenCount_ToonIsSmaller()
    {
        // Arrange
        var users = Enumerable.Range(1, 100)
            .Select(i => new TestUser { Id = i, Name = $"User{i}" })
            .ToList();

        // Act
        var toonResult = ToonSerializer.Serialize(users);
        var jsonResult = System.Text.Json.JsonSerializer.Serialize(users);

        // Assert
        toonResult.Length.ShouldBeLessThan(jsonResult.Length);

        // TOON deve ser pelo menos 30% menor
        var reduction = (jsonResult.Length - toonResult.Length) / (double)jsonResult.Length;
        reduction.ShouldBeGreaterThan(0.3);
    }
}

