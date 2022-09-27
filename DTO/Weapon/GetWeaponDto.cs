using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg.DTO.Weapon
{
    public class GetWeaponDto
    {
        public string Name { get; set; } = string.Empty; 
        
        public int Damge { get; set; }
    }
}