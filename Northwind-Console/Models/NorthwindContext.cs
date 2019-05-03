using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Configuration;
//using Microsoft.EntityFrameworkCore.ModelBuilder;

//using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NorthwindConsole.Models
{
    public class NorthwindContext : DbContext
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public NorthwindContext() : base("name=NorthwindContext") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>()
                .HasKey(c => new { c.OrderID, c.ProductID });
            modelBuilder.Entity<OrderDetail>()
            .ToTable("Order Details");
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public void addProduct(Product newProduct)
        {
            try
            {
                Products.Add(newProduct);
                SaveChanges();
            }
            catch(Exception e)
            {
                logger.Error($"Error Adding Product {newProduct.ProductName}: {e.Message}");
            }
        }

        public void addCategory(Category newCategory)
        {
            try
            {
                Categories.Add(newCategory);
                SaveChanges();
            }
            catch (Exception e)
            {
                logger.Error($"Error Adding Category {newCategory.CategoryName}: {e.Message}");
            }
        }

        public void updateProduct(Product updatedProduct)
        {
            try
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
            catch (Exception e)
            {
                logger.Error($"Error Updating Product {updatedProduct.ProductName}: {e.Message}");
            }
        }

        public void updateCategory(Category updatedCategory)
        {
            try
            {
                Category c = Categories.Find(updatedCategory.CategoryId);
                c.CategoryName = updatedCategory.CategoryName;
                c.Description = updatedCategory.Description;
                SaveChanges();
            }
            catch (Exception e)
            {
                logger.Error($"Error Updating Category {updatedCategory.CategoryName}: {e.Message}");
            }
        }

        public void deleteCategory(Category targetCategory)
        {
            try
            {
                var productList = targetCategory.Products;
                int unassignedID = -1;
                foreach (Category c in Categories)
                {
                    if (c.CategoryName == "Not Assigned")
                    {
                        unassignedID = c.CategoryId;
                    }
                }
                if (unassignedID == -1)
                {
                    Category unnasignedCategory = new Category();
                    unnasignedCategory.CategoryName = "Not Assigned";
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

                foreach (Product p in productList)
                {
                    p.CategoryId = unassignedID;
                }
                SaveChanges();
                if (targetCategory.CategoryId != unassignedID)
                {
                    Categories.Remove(targetCategory);
                    SaveChanges();
                }
            }
            catch (Exception e)
            {
                logger.Error($"Error Removing Category {targetCategory.CategoryName}: {e.Message}");
            }
        }

        /*
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
        */

        public void deleteProduct(Product targetProduct)
        {
            try
            {
                //var orders = targetProduct.OrderDetails;
                Product p = Products.Find(targetProduct.ProductID);
                //var prods = Products.Include(QueryableExtensions => QueryableExtensions.OrderDetails);
                //var a = Products.Find(targetProduct.ProductID

                if (p != null)
                {
                    logger.Info($"Product {p.ProductName} Selected");
                    logger.Info($"OD Count { p.OrderDetails.Count}");
                    if(p.OrderDetails.Count!=0)
                    {
                        OrderDetail od = null;
                        do
                        {
                            od = p.OrderDetails.FirstOrDefault();
                            if(od!=null)
                            {
                                logger.Info($"Order Detail OID:{od.OrderID } & PID:{od.ProductID}");
                                deleteOrderDetails(od);
                            }

                        } while (od != null);

                    }
                    foreach (OrderDetail od in p.OrderDetails)
                    {
                        deleteOrderDetails(od);
                    }
                    //SaveChanges();
                    Products.Remove(p);
                    SaveChanges();
                    logger.Info($"{p.ProductName} Deleted");
                }
            }
            catch (Exception e)
            {
                logger.Error($"Error deleting Product {targetProduct.ProductName}: {e.Message}");
            }


        }

        public void deleteOrderDetails(OrderDetail targetOD)
        {
            try
            {
                if (targetOD != null)
                {
                    OrderDetails.Remove(targetOD);
                    SaveChanges();
                    logger.Info($"Order Detail OID:{targetOD.OrderID } & PID:{targetOD.ProductID} Deleted");
                }
            } catch(Exception e)
            {
                logger.Error($"Error Deleting Order Detail with Order ID {targetOD.OrderID} and Product ID {targetOD.ProductID}: {e.Message}");
            }
        }
    }
}
