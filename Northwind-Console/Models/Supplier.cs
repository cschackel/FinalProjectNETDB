using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NorthwindConsole.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        [Required(ErrorMessage ="Company Must Have a Name")]
        [StringLength(40,ErrorMessage ="Company Name cannot Excede 40 Characters")]
        public string CompanyName { get; set; }
        [StringLength(30, ErrorMessage = "Company Contact Name cannot Excede 30 Characters")]
        public string ContactName { get; set; }
        [StringLength(30, ErrorMessage = "Company Contact Title cannot Excede 30 Characters")]
        public string ContactTitle { get; set; }
        [StringLength(60, ErrorMessage = "Company Address cannot Excede 60 Characters")]
        public string Address { get; set; }
        [StringLength(15, ErrorMessage = "City Name cannot Excede 15 Characters")]
        public string City { get; set; }
        [StringLength(15, ErrorMessage = "Region Name cannot Excede 15 Characters")]
        public string Region { get; set; }
        [StringLength(10, ErrorMessage = "Postal Code cannot Excede 10 Characters")]
        public string PostalCode { get; set; }
        [StringLength(15, ErrorMessage = "Country Name cannot Excede 15 Characters")]
        public string Country { get; set; }
        [Phone]
        [StringLength(24, ErrorMessage = "Phone Number cannot Excede 24 Characters")]
        public string Phone { get; set; }
        [Phone]
        [StringLength(24, ErrorMessage = "Fax Number cannot Excede 24 Characters")]
        public string Fax { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}
