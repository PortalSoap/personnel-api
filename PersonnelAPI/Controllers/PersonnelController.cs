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

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var personnel = await _personnelRepository.GetAll();
        return Ok(personnel);
    }

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