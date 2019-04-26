using System;
using System.ComponentModel.DataAnnotations;

namespace NorthwindConsole.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        [Required(ErrorMessage = "Name Must Not Be Null")]
        [StringLength(40,ErrorMessage ="Name Must be 40 Characters or Less")]
        public string ProductName { get; set; }
        [StringLength(20,ErrorMessage ="Quantity must be 20 Characters or Less")]
        public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public Int16? UnitsInStock { get; set; }
        public Int16? UnitsOnOrder { get; set; }
        public Int16? ReorderLevel { get; set; }
        [Required(ErrorMessage = "Product Must Either be Discontinued or Not")]
        public bool Discontinued { get; set; }

       
        public int? CategoryId { get; set; }
        public int? SupplierId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Supplier Supplier { get; set; }


        //public virtual OrderDetail OrderDetails { get; set; }
    }
}
