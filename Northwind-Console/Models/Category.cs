using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NorthwindConsole.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Category Must Have a Name")]
        [StringLength(15,ErrorMessage = "Name must be 15 Characters or Less")]
        public string CategoryName { get; set; }
        public string Description { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}
