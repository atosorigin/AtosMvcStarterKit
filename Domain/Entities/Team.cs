using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Customer.Project.Domain.Entities
{
    public class Team
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        // virtual for lazy loading
        [JsonIgnore]
        public virtual IList<Person> Members { get; set; }

        // Always override ToString to display the name of the instance 
        public override string ToString()
        {
            return Name;
        }
    }
}
