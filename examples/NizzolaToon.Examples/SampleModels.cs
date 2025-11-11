namespace NizzolaToon.Examples;

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

public class PersonData
{
    public List<Person> People { get; set; } = new();
}

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Occupation { get; set; } = string.Empty;
    public List<string> Skills { get; set; } = new();
    public bool IsActive { get; set; }
}
