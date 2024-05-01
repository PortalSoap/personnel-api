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

    /// <summary>
    /// Obtém todos os itens Person existentes no banco de dados, já mapeados. [Obtains all existent Person items on the database, already mapped]
    /// </summary>
    /// <returns>
    /// Uma lista de itens Person mapeados. [A list of mapped Person items]
    /// </returns>
    public async Task<IEnumerable<PersonVO>> GetAll()
    {
        List<Person> personnel = await _context.Personnel.ToListAsync();
        return _mapper.Map<List<PersonVO>>(personnel);
    }

    /// <summary>
    /// Obtém um item Person, baseando-se em seu id. [Obtains a Person item based on its id]
    /// </summary>
    /// <param name="id">Um parâmetro id de tipo inteiro. [An integer id parameter]</param>
    /// <returns>
    /// O item Person desejado, já mapeado, ou um valor nulo, caso nenhum item correspondente seja encontrado. [The wished Person item, already mapped, or a null value if no corresponding Person item is found]
    /// </returns>
    public async Task<PersonVO?> GetById(int id)
    {
        var person = await _context.Personnel.Where(p => p.Id == id).FirstOrDefaultAsync();
        return _mapper.Map<PersonVO>(person);
    }

    /// <summary>
    /// Adiciona um item Person no banco de dados e retorna sua versão mapeada. [Adds a Person item on the database and returns its mapped version]
    /// </summary>
    /// <param name="personVO">O item Person a ser adicionado ao banco de dados. [The Person item to be added to the database]</param>
    /// <returns>
    /// O item Person adicionado, já mapeado, ou um valor nulo, caso o parâmetro personVO seja nulo ou tenha o valor de sua propriedade age inferior a 0. [The added Person item, already mapped, or a null value if the personVO parameter is null or the value of its age property is below 0]
    /// </returns>
    public async Task<PersonVO?> Create(PersonVO personVO)
    {
        var person = _mapper.Map<Person>(personVO);

        if (person == null || personVO.Age < 0)
            return null;

        await _context.Personnel.AddAsync(person);
        await _context.SaveChangesAsync();

        return _mapper.Map<PersonVO>(person);
    }

    /// <summary>
    /// Atualiza um item Person existente e retorna sua versão mapeada. [Updates an existing Person item and returns its mapped version]
    /// </summary>
    /// <param name="personVO">O item Person contendo as alterações desejadas. [The Person item containing the wished changes]</param>
    /// <returns>
    /// O item Person atualizado, já mapeado, ou um valor nulo, caso o parâmetro personVO seja nulo. [The updated Person item, already mapped, or a null value if the personVO parameter is null]
    /// </returns>
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

    /// <summary>
    /// Remove um item Person, baseando-se em seu id. [Removes a Person item based on its id]
    /// </summary>
    /// <param name="id">Um parâmetro id de tipo inteiro. [An integer id parameter]</param>
    /// <returns>
    /// Um valor true, caso o item seja encontrado e deletado ou um valor false, caso o item não seja encontrado e, portanto, não deletado. [A true value if the Person item is sucessfully found and removed or a false value if no Person item is found and, thereby, not removed]
    /// </returns>
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