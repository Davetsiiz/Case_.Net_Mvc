using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_Futboligs.Entity
{
    public class Player
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public int Age { get; set; }
        public int TeamId { get; set; }
        public virtual Teams Teams { get; set; }

        
    }
}