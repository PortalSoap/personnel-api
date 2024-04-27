using AutoMapper;
using PersonnelAPI.Models;
using PersonnelAPI.Models.ValueObjects;

namespace PersonnelAPI.Config;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config => {
            config.CreateMap<PersonVO, Person>();
            config.CreateMap<Person, PersonVO>();
        });

        return mappingConfig;
    }
}