using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.DTO.Character;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
    
        //add a static mock character
        private static List<Character> characters = new List<Character> {
            new Character(),
            new Character{ Id = 1, Name = "Sam" }
        };
        public readonly IMapper _mapper;
        
        public CharacterService(IMapper mapper)
        {
            _mapper = mapper; 
        }
        
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
          
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            Character character = _mapper.Map<Character>(newCharacter);
            //take max character Id and increase by one when adding new character 
            character.Id = characters.Max(c => c.Id) + 1; 
            characters.Add(character);
            serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> response = new ServiceResponse<List<GetCharacterDto>>(); 
            
            try
            { 
                Character character = characters.First(c => c.Id == id);
                characters.Remove(character);
                response.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            
            } catch(Exception ex) { 
                response.Success = false; 
                response.Message = ex.Message; 
            } 
            
            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            
            return new ServiceResponse<List<GetCharacterDto>> 
            { 
                Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList() 
            };
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>(); 
            var character = characters.FirstOrDefault(c => c.Id == id);
            //FirstOrDefault function = return the 1st elemt of a sequence or a default value if element isnt there 
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character); 
            return serviceResponse;
        }
        
        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>(); 
            
            try{ 
            
                Character character = characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);
                
                character.Name = updatedCharacter.Name; 
                character.HitPoints = updatedCharacter.HitPoints; 
                character.Strength = updatedCharacter.Strength; 
                character.Defense = updatedCharacter.Defense;
                character.Intelligence = updatedCharacter.Intelligence; 
                character.Class = updatedCharacter.Class; 
                
                response.Data = _mapper.Map<GetCharacterDto>(character);
            
            } catch(Exception ex) { 
                response.Success = false; 
                response.Message = ex.Message; 
            } 
            
            return response; 
        }
    }
}