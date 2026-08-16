using System;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAppWithLogin.Models
{
    public class Todos
    {
        [Key]
        public int Id { get; set; }

       public string Description { get; set; }

       public bool IsComplete { get; set; } = false;

        // Foreign key
        public string  UserId { get; set; }

        //Navigation properties
        public Users User { get; set; }
    }
}
