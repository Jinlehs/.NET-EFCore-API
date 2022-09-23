using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.DTO.Character;

namespace dotnet_rpg
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //creates map from one type to another 
            CreateMap<Character, GetCharacterDto>(); 
            CreateMap<AddCharacterDto, Character>(); 
            CreateMap<UpdateCharacterDto, Character>(); 
        }
    }
}