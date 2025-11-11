using Nizzola.ToonConvert;

namespace NizzolaToon.Tests;

public class TestUser
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class TestUserWithArray
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<string> Interests { get; set; } = new();
}

public class TestUserWithIgnore
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    [ToonIgnore]
    public string Secret { get; set; } = string.Empty;

    [ToonIgnore]
    public int InternalId { get; set; }
}

public class TestUserComplex
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
}

public class TestPrimitives
{
    public int IntValue { get; set; }
    public string StringValue { get; set; } = string.Empty;
    public bool BoolValue { get; set; }
    public double DoubleValue { get; set; }
    public DateTime DateValue { get; set; }
}

public class CompanyData
{
    public string CompanyName { get; set; } = string.Empty;
    public List<Employee> Employees { get; set; } = new();
}

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public List<string> Skills { get; set; } = new();
}

public class Order
{
    public int OrderId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public List<string> Items { get; set; } = new();
}