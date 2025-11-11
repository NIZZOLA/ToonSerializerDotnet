using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NizzolaToon.Examples;

public static class SampleDataBuilder
{
    public static CompanyData GenerateCompanyData()
    {
        return new CompanyData
        {
            CompanyName = "TechCorp",
            Employees = new List<Employee>
            {
                new() { Id = 1, Name = "Alice", Department = "Engineering", Skills = new() { "C#", "Azure" } },
                new() { Id = 2, Name = "Bob", Department = "Sales", Skills = new() { "Communication", "CRM" } },
                new() { Id = 3, Name = "Carol", Department = "Engineering", Skills = new() { "Python", "ML" } }
            }
        };
    }

    public static PersonData GeneratePersonData()
    {
        return new PersonData
        {
            People = new List<Person>
            {
                new() { Id = 1, Name = "Alice", Age = 30, Address = "123 Tech St", City = "Techville", State = "CA", ZipCode = "90210", PhoneNumber = "555-0123", Email = "alice@techcorp.com", Occupation = "Engineer", Skills = new() { "C#", "Azure" }, IsActive = true },
                new() { Id = 2, Name = "Bob", Age = 25, Address = "456 Sales Ave", City = "Salesburg", State = "NY", ZipCode = "10001", PhoneNumber = "555-0456", Email = "bob@salescorp.com", Occupation = "Sales Manager", Skills = new() { "Communication", "CRM" }, IsActive = true },
                new() { Id = 3, Name = "Carol", Age = 28, Address = "789 Dev Blvd", City = "Dev City", State = "TX", ZipCode = "73301", PhoneNumber = "555-0789", Email = "carol@devcorp.com", Occupation = "Developer", Skills = new() { "Python", "ML" }, IsActive = true },
                new() { Id = 4, Name = "Dave", Age = 22, Address = "321 Design Rd", City = "Design Town", State = "FL", ZipCode = "33101", PhoneNumber = "555-0987", Email = "dave@designcorp.com", Occupation = "Designer", Skills = new() { "Photoshop", "UI/UX" }, IsActive = false },
                new() { Id = 5, Name = "Eve", Age = 35, Address = "654 Marketing Pl", City = "Market City", State = "IL", ZipCode = "60601", PhoneNumber = "555-1234", Email = "eve@marketcorp.com", Occupation = "Marketing Specialist", Skills = new() { "SEO", "Content Marketing" }, IsActive = true },
                new() { Id = 6, Name = "Frank", Age = 29, Address = "987 Support Ln", City = "Support City", State = "WA", ZipCode = "98001", PhoneNumber = "555-4321", Email = "frank@supportcorp.com", Occupation = "Support Engineer", Skills = new() { "Technical Support", "Problem Solving" }, IsActive = true },
                new() { Id = 7, Name = "Grace", Age = 31, Address = "135 Research St", City = "Research City", State = "MA", ZipCode = "02101", PhoneNumber = "555-5678", Email = "grace@researchcorp.com", Occupation = "Research Analyst", Skills = new() { "Data Analysis", "Statistics" }, IsActive = true }
            }
        };
    }
}
