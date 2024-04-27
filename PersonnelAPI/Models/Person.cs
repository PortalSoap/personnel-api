namespace PersonnelAPI.Models;

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; } = "n/a";
    public string Surname { get; set; } = "n/a";
    public string Job { get; set; } = "n/a";
    public int Age { get; set; }
}