using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class ProyectSkill
    {   [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public AppProyect Project { get; set; } = null!;

        public int SkillId { get; set; }

         public required AppSkill Skill { get; set; } = null!;
        
    }
}