using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using dotnet_rpg.DTO.Character;
using dotnet_rpg.Services.CharacterService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace dotnet_rpg.Controllers
{
    //anything in a [] for cs are attributes -> declarative tag that is used to convey info to runtime about the behaviors of various elements 
    //like classes, methods, structures, enumerators 

    //applies inference rules for the default data sources of action parameters 
    //these rules save you from having to ID binding sources manually by applying attributes to the action params 
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    //controllerbase class provides many properties and methods that are useful  for handling Http requests 
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;
    
        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
            
        }
        //this allows us to override the authentication and allows for anyone to access this functionality 
        //[AllowAnonymous]
        //retrieves the info of the given server using a url 
        //url path: /api/Character/GetAll
        [HttpGet("GetAll")]
        //Action result is a data type -> result of action when executed -> in this case the result is character type 
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get()
        {
           return Ok(await _characterService.GetAllCharacters()); 
    
        }

        //url path: /api/Character/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingle(int id)
        {

            //creates an OkResult that produces an empty Status200OK response 
            //FirstOrDefault function = return the 1st elemt of a sequence or a default value if element isnt there 
            return Ok(await _characterService.GetCharacterById(id));
        }
        
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto newCharacter)
        { 
            return Ok(await _characterService.AddCharacter(newCharacter));
        }
        
        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        { 
            var response = await _characterService.UpdateCharacter(updatedCharacter); 
            if(response.Data == null) { 
                return NotFound(response);
            }
            
            return Ok(response);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Delete(int id)
        {

            //creates an OkResult that produces an empty Status200OK response 
            //FirstOrDefault function = return the 1st elemt of a sequence or a default value if element isnt there 
            var response = await _characterService.DeleteCharacter(id); 
            if(response.Data == null) { 
                return NotFound(response);
            }
            
            return Ok(response);
        }
        
        [HttpPost("Skill")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill) {
            return Ok(await _characterService.AddCharacterSkill(newCharacterSkill));
        }
    }
}