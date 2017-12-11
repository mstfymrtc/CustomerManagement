using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagement.Models
{
    public class State
    {
        public int StateID { get; set; }

        [StringLength(2)]
        public string Abbreviation { get; set; }

        [StringLength(2)]
        public string Name { get; set; }
    }
}
