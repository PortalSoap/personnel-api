using Microsoft.EntityFrameworkCore;
using PersonnelAPI.Models;

namespace PersonnelAPI.Data;

public class PersonnelDbContext : DbContext
{
    public PersonnelDbContext(DbContextOptions<PersonnelDbContext> options)
        : base(options) {}

    public DbSet<Person> Personnel { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().Property(p => p.Name)
        .HasMaxLength(80)
        .IsRequired();

        modelBuilder.Entity<Person>().Property(p => p.Surname)
        .HasMaxLength(255)
        .IsRequired(); // Embora a propriedade "Surname" possa ser omitida, na lógica de negócio, ela jamais deve ter estado null. Quando não houver entrada para a propriedade, o valor será tido por "n/a".
    }
}