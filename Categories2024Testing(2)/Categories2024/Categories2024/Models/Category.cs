using System.ComponentModel.DataAnnotations;

namespace Categories2024.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int CategoryOrder { get; set; }
    }
}
