using System.ComponentModel.DataAnnotations;


namespace CustomerManagement.Models
{
    public class State
    {
        public int Id { get; set; }

        [StringLength(2)]
        public string Abbreviation { get; set; }
        [StringLength(25)]
        public string Name { get; set; }
    }
}
