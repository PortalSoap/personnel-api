using Microsoft.AspNetCore.Mvc;
using PersonnelAPI.Models.ValueObjects;
using PersonnelAPI.Repository.Interfaces;

namespace PersonnelAPI.Controllers;

[ApiController]
[Route("personnel")]
public class PersonnelController : ControllerBase
{
    private IPersonnelRepository _personnelRepository;

    public PersonnelController(IPersonnelRepository personnelRepository)
    {
        _personnelRepository = personnelRepository;
    }

    /// <summary>
    /// Retorna todos os itens Person existentes. [Returns all existent Person items]
    /// </summary>
    /// <returns>
    /// Um IEnumerable que contém todos os items Person existentes no banco de dados. [An IEnumerable which contains all existing Person items in the databasee]
    /// </returns>
    /// <response code="200">Retorna a coleção de todos os itens Person existentes. [Returns the collection of all existing Person items]</response>
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var personnel = await _personnelRepository.GetAll();
        return Ok(personnel);
    }

    /// <summary>
    /// Retorna um item Person com base em um id de tipo inteiro. [Returns a Person item based on an integer id parameter]
    /// </summary>
    /// <param name="id">O id do item Person a ser obtido. [The id of the Person item to be obtained]</param>
    /// <returns>
    /// O item Person de mesmo id. [The Person item of same id]
    /// </returns>
    /// <response code="200">Retorna o item Person desejado. [Successfully returns the wished Person item]</response>
    /// <response code="400">Se o parâmetro id não for um inteiro. [If the id parameter is not an integer]</response>
    /// <response code="404">Se nenhum item Person correspondente ao parâmetro id informado for encontrado. [If no Person item matching the id parameter is found]</response>
    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(string id)
    {
        if (int.TryParse(id, out int convertedId))
        {
            var person = await _personnelRepository.GetById(convertedId);

            if (person == null)
                return NotFound();

            return Ok(person);
        }
        
        return BadRequest();
    }

    /// <summary>
    /// Adiciona um item Person no banco de dados. [Adds a Person item in the database]
    /// </summary>
    /// <param name="personVO">O item Person a ser adicionado no banco de dados. [The Person item to be added to the database]</param>
    /// <returns>
    /// O item Person adicionado. [The added Person item]
    /// </returns>
    /// <remarks>
    /// Request Example:
    ///     
    ///     POST /personnel
    ///     {
    ///         "name": "Alex",
    ///         "surname": "Green",
    ///         "job": "Mechanical Engineer",
    ///         "age": "32"
    ///     }
    /// </remarks>
    /// <response code="200">Adiciona e retorna com sucesso o item Person passado como parâmetro. [Successfully adds and returns the Person item passed as a parameter]</response>
    /// <response code="400">Se o item Person não foi adicionado ao banco de dadoas. [If the Person item was not added to the database]</response>
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] PersonVO personVO)
    {
        var person = await _personnelRepository.Create(personVO);

        if (person == null)
        {
            return BadRequest();
        }

        return Ok(person);
    }

    /// <summary>
    /// Atualiza um item Person existente. [Updates an existing Person item]
    /// </summary>
    /// <param name="id">O id do item Person a ser atualizado. [The id of the Person item to be updated]</param>
    /// <param name="personVO">O item Person contendo as alterações desejadas. [The Person item containing the wished changes]</param>
    /// <returns>
    /// O item Person atualizado. [The updated Person item]
    /// </returns>
    /// <remarks>
    /// Request Example:
    ///     
    ///     PUT /personnel/1
    ///     {
    ///         "id": 1,
    ///         "name": "Alex",
    ///         "surname": "Marshall Green",
    ///         "job": "Mechanical Engineer",
    ///         "age": "34"
    ///     }
    /// </remarks>
    /// <response code="200">Atualiza e retorna com sucesso o item Person atualizado. [Successfully updates and returns the updated Person item]</response>
    /// <response code="400">Se o parâmetro id for diferente do id do item Person passado como body parameter. [If the id parameter differs from the Person item's id passed as body parameter]</response>
    /// <response code="404">Se nenhum item Person correspondente ao parâmetro id informado for encontrado. [If no Person item matching the id parameter is found]</response>
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, [FromBody] PersonVO personVO)
    {
        if (int.TryParse(id, out int convertedId))
        {
            if (convertedId != personVO.Id)
                return BadRequest();

            var updatedPerson = await _personnelRepository.Update(personVO);

            if (updatedPerson == null)
                return NotFound();

            return Ok(updatedPerson);
        }

        return BadRequest();
    }

    /// <summary>
    /// Remove um item Person do banco de dados com base em um id de tipo inteiro. [Removes a Person item based on an integer id parameter]
    /// </summary>
    /// <param name="id">O id do item Person a ser removido. [The id of the Person item to be removed]</param>
    /// <returns></returns>
    /// <response code="204">Remove com sucesso o item Person correspondente, mas sem nenhum payload. [Successfully removes the corresponding Person item, but without any payload.</response>
    /// <response code="404">Se nenhum item Person correspondente ao parâmetro id informado for encontrado. [If no Person item matching the id parameter is found]</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        if (int.TryParse(id, out int convertedId))
        {
            if (await _personnelRepository.Delete(convertedId))
                return NoContent();

            return NotFound();
        }

        return BadRequest();
    }
}