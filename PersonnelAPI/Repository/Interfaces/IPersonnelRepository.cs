using PersonnelAPI.Models.ValueObjects;

namespace PersonnelAPI.Repository.Interfaces;

public interface IPersonnelRepository
{
    Task<IEnumerable<PersonVO>> GetAll();
    Task<PersonVO?> GetById(int id);
    Task<PersonVO?> Create(PersonVO personVO);
    Task<PersonVO?> Update(PersonVO personVO);
    Task<bool> Delete(int id);
}