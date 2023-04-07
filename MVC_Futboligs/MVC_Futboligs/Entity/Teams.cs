using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVC_Futboligs.Entity
{
    public class Teams
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Coach { get; set; }
        public virtual ICollection<Player> Player { get; set; }
    }
}