using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
   
        
    public class ProyectExperience
{
    
    [Key]
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public AppProyect? Project { get; set; }

    public int ExperienceId { get; set; }
    public AppExperience? Experience { get; set; }
}

}