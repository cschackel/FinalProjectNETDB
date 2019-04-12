using System.Data.Entity;

namespace NorthwindConsole.Models
{
    public class NorthwindContext : DbContext
    {
        public NorthwindContext() : base("name=NorthwindContext") { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        public void addProduct(Product newProduct)
        {
            Products.Add(newProduct);
            SaveChanges();
        }

        public void addCategory(Category newCategory)
        {
            Categories.Add(newCategory);
            SaveChanges();
        }

        public void updateProduct(Product updatedProduct)
        {
            Product p = Products.Find(updatedProduct.ProductID);
            p.ProductName = updatedProduct.ProductName;
            p.CategoryId = updatedProduct.CategoryId;
            p.SupplierId = updatedProduct.SupplierId;
            p.UnitPrice = updatedProduct.UnitPrice;
            p.UnitsInStock = updatedProduct.UnitsInStock;
            p.UnitsOnOrder = updatedProduct.UnitsOnOrder;
            p.Discontinued = updatedProduct.Discontinued;
            p.ReorderLevel = updatedProduct.ReorderLevel;
            p.QuantityPerUnit = updatedProduct.QuantityPerUnit;
            SaveChanges();
        }

        public void updateCategory(Category updatedCategory)
        {
            Category c = Categories.Find(updatedCategory.CategoryId);
            c.CategoryName = updatedCategory.CategoryName;
            c.Description = updatedCategory.Description;
            SaveChanges();
        }
    }
}
