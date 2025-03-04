using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;

namespace API.Entities
{
    public class AppUser
    {

        public int Id { get; set; }

        public required string Name { get; set; }  = string.Empty;

        public string? Email { get; set; }

        public string? PasswordHash { get; set; }

        public UserRole Role { get; set; } = UserRole.User;

        public string? Token { get; set; }

        public List<AppSkill> Skills { get; set; } = new();

        public List<AppExperience> Experiences { get; set; } = new List<AppExperience>();

        public List<AppProyect> Projects { get; set; } = new List<AppProyect>();

         public ICollection<UserProyect>? UserProyects { get; set; }

         public ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();

         

     

        

         


       




    }
}