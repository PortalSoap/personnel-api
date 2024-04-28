using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PersonnelAPI.Data;
using PersonnelAPI.Models;
using PersonnelAPI.Models.ValueObjects;
using PersonnelAPI.Repository.Interfaces;

namespace PersonnelAPI.Repository;

public class PersonnelRepository : IPersonnelRepository
{
    private readonly PersonnelDbContext _context;
    private IMapper _mapper;

    public PersonnelRepository(PersonnelDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PersonVO>> GetAll()
    {
        List<Person> personnel = await _context.Personnel.ToListAsync();
        return _mapper.Map<List<PersonVO>>(personnel);
    }

    public async Task<PersonVO?> GetById(int id)
    {
        var person = await _context.Personnel.Where(p => p.Id == id).FirstOrDefaultAsync();
        return _mapper.Map<PersonVO>(person);
    }

    public async Task<PersonVO?> Create(PersonVO personVO)
    {
        var person = _mapper.Map<Person>(personVO);

        if (person == null || personVO.Age < 0)
            return null;

        await _context.Personnel.AddAsync(person);
        await _context.SaveChangesAsync();

        return _mapper.Map<PersonVO>(person);
    }

    public async Task<PersonVO?> Update(PersonVO personVO)
    {
        var person = await _context.Personnel.Where(p => p.Id == personVO.Id).FirstOrDefaultAsync();

        if (person == null)
            return null;

        if
        (
            person.Name != personVO.Name ||
            person.Surname != personVO.Surname ||
            person.Job != personVO.Job ||
            person.Age != personVO.Age
        )
        {
            if (!string.IsNullOrEmpty(personVO.Name))
                person.Name = personVO.Name;

            if (!string.IsNullOrEmpty(personVO.Surname))
                person.Surname = personVO.Surname;

            if (!string.IsNullOrEmpty(personVO.Job))
                person.Job = personVO.Job;

            if (personVO.Age >= 0)
                person.Age = personVO.Age;

            await _context.SaveChangesAsync();
        }

        return _mapper.Map<PersonVO>(person);
    }

    public async Task<bool> Delete(int id)
    {
        var person = await _context.Personnel.Where(p => p.Id == id).FirstOrDefaultAsync();
        
        if (person == null)
            return false;

        _context.Personnel.Remove(person);
        await _context.SaveChangesAsync();
        return true;
    }
}