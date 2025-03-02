using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class UserProyect
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public AppUser? User { get; set; }

    public int ProjectId { get; set; }
    public AppProyect? Project { get; set; }
}


}