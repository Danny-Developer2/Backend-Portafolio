using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class UserSkill
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public AppUser? User { get; set; }

        public int SkillId { get; set; }
        public AppSkill? Skill { get; set; }




    }
}