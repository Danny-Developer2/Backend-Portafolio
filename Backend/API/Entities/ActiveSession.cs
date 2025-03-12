using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class ActiveSession
    {
        public int? Id { get; set; }

        public int UserId { get; set; }

        public string? TokenHash { get; set; }

        public DateTime? Expiration { get; set; }
    }
}