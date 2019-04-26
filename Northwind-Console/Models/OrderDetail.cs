using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindConsole.Models
{
    [Table("Order Details")]
    public class OrderDetail
    {
        //[Key, Column(Order = 0)]
        public int OrderID { get; set; }
        //[Key, Column(Order = 1)]
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public Int16 Quantity { get; set; }
        public decimal Discount { get; set; }

        public virtual Product Product { get; set; }
        
    }
    
}
