using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class AppProyect
    {

        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }

        public required string Technology { get; set; }

        public required string Url { get; set; }

        public required string ImgUrl { get; set; }

        public List<AppUser> Users { get; set; } = new List<AppUser>();

        public ICollection<UserProyect>? UserProyects { get; set; }

        // Relación con UserSkills
        public ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();

        // Relación con ProyectExperience
        public ICollection<ProyectExperience>? ProyectExperience { get; set; }

        public ICollection<ProyectSkill>? ProyectSkills { get; set; }





       

    }
}