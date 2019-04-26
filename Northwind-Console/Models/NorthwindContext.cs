using System;
using System.Data.Entity;

namespace NorthwindConsole.Models
{
    public class NorthwindContext : DbContext
    {
        public NorthwindContext() : base("name=NorthwindContext") { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        //public DbSet<OrderDetail> OrderDetails { get; set; }

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

        public void deleteCategory(Category targetCategory)
        {
            var productList = targetCategory.Products;
            int unassignedID = -1;
            foreach(Category c in Categories)
            {
                if(c.CategoryName=="Not Assigned")
                {
                    unassignedID = c.CategoryId;
                }
            }
            if(unassignedID==-1)
            {
                Category unnasignedCategory = new Category();
                unnasignedCategory.CategoryName="Not Assigned";
                unnasignedCategory.Description = "Products With No Category";
                addCategory(unnasignedCategory);
                SaveChanges();
                foreach (Category c in Categories)
                {
                    if (c.CategoryName == "Not Assigned")
                    {
                        unassignedID = c.CategoryId;
                    }
                }
            }

            foreach(Product p in productList)
            {
                p.CategoryId = unassignedID;
            }
            SaveChanges();
            if(targetCategory.CategoryId!=unassignedID)
            {
                Categories.Remove(targetCategory);
                SaveChanges();
            }
        }
        
        public void deleteProduct(Product targetProduct)
        {
            try
            {
                //var orders = targetProduct.OrderDetails;
                Product p = Products.Find(targetProduct.ProductID);
                if (targetProduct != null)
                {
                    Products.Remove(targetProduct);
                    SaveChanges();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message + " " + e.StackTrace +" " + e.InnerException);
            }
        }
        


    }
}
