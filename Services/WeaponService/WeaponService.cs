using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.DTO.Character;
using dotnet_rpg.DTO.Weapon;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.WeaponService
{
    public class WeaponService : IWeaponService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IMapper _mapper;
    
        public WeaponService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            
        }
        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>(); 
            try { 
                Character character = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId &&
                    c.User.Id == int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                if(character == null) { 
                    response.Success = false;
                    response.Message = "Character not found";
                    return response; 
                }
                Weapon weapon = new Weapon { 
                    Name = newWeapon.Name,
                    Damage = newWeapon.Damage, 
                    Character = character
                };
                //adds given entity to the context underlying the set in the added state such that it will be inserted into the db when savechanges is called 
                _context.Weapons.Add(weapon);
                await _context.SaveChangesAsync(); 
                response.Data = _mapper.Map<GetCharacterDto>(character);
            }
            
            catch(Exception ex) { 
                response.Success = false; 
                response.Message = ex.Message; 
            }
            return response; 
        }
    }
}