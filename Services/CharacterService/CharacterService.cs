using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.DTO.Character;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
    
        //add a static mock character
        public readonly IMapper _mapper;
        public readonly DataContext _context;
        
        public CharacterService(IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper; 
        }
        
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
          
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            Character character = _mapper.Map<Character>(newCharacter);
            _context.Characters.Add(character);
            await _context.SaveChangesAsync(); 
            serviceResponse.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> response = new ServiceResponse<List<GetCharacterDto>>(); 
            
            try
            { 
                Character character = await _context.Characters.FirstAsync(c => c.Id == id);
                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();
                response.Data = _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            
            } catch(Exception ex) { 
                response.Success = false; 
                response.Message = ex.Message; 
            } 
            
            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters(int userId)
        {
            var response = new ServiceResponse<List<GetCharacterDto>>(); 
            var dbCharacters = await _context.Characters
            .Where(c => c.User.Id == userId).ToListAsync(); 
            response.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return response;
            
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>(); 
            var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
            //FirstOrDefault function = return the 1st elemt of a sequence or a default value if element isnt there 
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter); 
            return serviceResponse;
        }
        
        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>(); 
            
            try{ 
            
                Character character = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
                
                character.Name = updatedCharacter.Name; 
                character.HitPoints = updatedCharacter.HitPoints; 
                character.Strength = updatedCharacter.Strength; 
                character.Defense = updatedCharacter.Defense;
                character.Intelligence = updatedCharacter.Intelligence; 
                character.Class = updatedCharacter.Class; 
                
                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<GetCharacterDto>(character);
            
            } catch(Exception ex) { 
                response.Success = false; 
                response.Message = ex.Message; 
            } 
            
            return response; 
        }
    }
}