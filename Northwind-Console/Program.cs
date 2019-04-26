using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NLog;
using NorthwindConsole.Models;

namespace NorthwindConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");
            try
            {
                string choice;
                do
                {
                    Console.WriteLine("1) Display Categories");
                    Console.WriteLine("2) Add Category");
                    Console.WriteLine("3) Display A Category and related products");
                    Console.WriteLine("4) Display all Categories and their related products");
                    Console.WriteLine("5) Add Product");
                    Console.WriteLine("6) Display Specific Product Information");
                    Console.WriteLine("7) Edit Product");
                    Console.WriteLine("8) Edit Category");
                    Console.WriteLine("9) Display Products");
                    Console.WriteLine("10) Delete a Product");
                    Console.WriteLine("11) Delete a Category");
                    Console.WriteLine("\"q\" to quit");
                    choice = Console.ReadLine();
                    Console.Clear();
                    logger.Info($"Option {choice} selected");

                    if (choice == "1")
                    {
                        displayCategoriesAndDescription();
                    } 
                    else if (choice == "2")
                    {
                        addCategory();
                    }
                    else if (choice == "3")
                    {
                        displayCategoryProducts();
                    }
                    else if (choice == "4")
                    {
                        displayAllCategoriesAndProducts();
                    }
                    else if(choice=="5")
                    {
                        addProduct();
                    }
                    else if(choice=="6")
                    {
                        displayProductInformation();
                    }
                    else if(choice=="7")
                    {
                        editProductInfo();
                    }
                    else if (choice == "8")
                    {
                        editCategory();
                    }
                    else if (choice == "9")
                    {
                        displayProducts();
                    }
                    else if (choice == "10")
                    {
                        deleteProduct();
                    }
                    else if (choice == "11")
                    {
                        deleteCategory();
                    }
                    Console.WriteLine();

                } while (choice.ToLower() != "q");
            }
            catch (Exception ex)
            {
                logger.Error("Message: "+ex.Message + "\nInner: "  + ex.InnerException + "\nStack: " + ex.StackTrace);
            }
            logger.Info("Program ended");
        }

        //Adds a new Product to database
        public static void addProduct()
        {
            Product newProduct = getProductInfo();
            if(validateProduct(newProduct))
            {
                var db = new NorthwindContext();
                db.addProduct(newProduct);
            }
        }

        //Adds a new category to database
        public static void addCategory()
        {
            Category newCategory = getCategoryInfo();
            if(validateCategory(newCategory))
            {
                var db = new NorthwindContext();
                db.addCategory(newCategory);
            }
        }

        /*
        //Adds a new Supplier to database
        public static void addSupplier()
        {
            Supplier newSupplier = getSupplierinfo();
            if(validateSupplier(newSupplier))
            {
                
            }
        }
        */

        //Displays All Suppliers
        public static void displaySuppliers()
        {
            var db = new NorthwindContext();
            var suppliers = db.Suppliers.OrderBy(s=>s.SupplierId);
            foreach(Supplier s in suppliers)
            {
                Console.WriteLine($"ID: {s.SupplierId} Name: {s.CompanyName}");
            }
        }

        //Displays Categories
        public static void displayCategories()
        {
            var db = new NorthwindContext();
            var cats = db.Categories.OrderBy(c => c.CategoryId);
            foreach (Category c in cats)
            {
                Console.WriteLine($"ID: {c.CategoryId} Name: {c.CategoryName}");
            }
        }

        //Prompts User for Product fields, returns Product
        public static Product getProductInfo()
        {
            int value;
            String temp;

            Product newProduct = new Product();

            Console.WriteLine("Product Name:");
            newProduct.ProductName = Console.ReadLine();

            Console.WriteLine("Supplier ID (Leave Blank If Null)");
            displaySuppliers();
            String sid = Console.ReadLine();
            if (int.TryParse(sid, out value))
            {
                newProduct.SupplierId = value;
            }
            else
            {
                newProduct.SupplierId = null;
                Console.WriteLine("**Defaulted To Null**");
            }

            Console.WriteLine("Category ID (Leave Blank If Null)");
            displayCategories();
            String cid = Console.ReadLine();
            if (int.TryParse(cid, out value))
            {
                newProduct.CategoryId = value;
            }
            else
            {
                newProduct.CategoryId = null;
                Console.WriteLine("**Defaulted To Null**");
            }

            Console.WriteLine("Quantity Per Unit (Leave Blank If Null):");
            newProduct.QuantityPerUnit = Console.ReadLine();
            if (newProduct.QuantityPerUnit.Equals(""))
            {
                newProduct.QuantityPerUnit = null;
                Console.WriteLine("**Defaulted To Null**");
            }

            Console.WriteLine("Unit Price (Leave Blank If Null)");
            temp = Console.ReadLine();
            temp = temp.Replace("$", "");
            if (!temp.Equals("") && decimal.TryParse(temp, out decimal unitValue))
            {
                newProduct.UnitPrice = unitValue;
            }
            else
            {
                newProduct.UnitPrice = null;
                Console.WriteLine("**Defaulted To Null**");
            }

            Console.WriteLine("Units In Stock (Leave Blank If Null):");
            temp = Console.ReadLine();
            if (short.TryParse(temp, out short shortValue))
            {
                newProduct.UnitsInStock = shortValue;
            }
            else
            {
                newProduct.UnitsInStock = null;
                Console.WriteLine("**Defaulted To Null**");
            }

            Console.WriteLine("Units On Order (Leave Blank If Null):");
            temp = Console.ReadLine();
            if (short.TryParse(temp, out shortValue))
            {
                newProduct.UnitsOnOrder = shortValue;
            }
            else
            {
                newProduct.UnitsOnOrder = null;
                Console.WriteLine("**Defaulted To Null**");
            }

            Console.WriteLine("Reorder Level (Leave Blank If Null):");
            temp = Console.ReadLine();
            if (short.TryParse(temp, out shortValue))
            {
                newProduct.ReorderLevel = shortValue;
            }
            else
            {
                newProduct.ReorderLevel = null;
                Console.WriteLine("**Defaulted To Null**");
            }

            bool validBool = false;
            do
            {
                Console.WriteLine("Has Product been Discontinued(y/n):");
                temp = Console.ReadLine();
                if (temp.ToLower().StartsWith("y") || temp.ToLower().StartsWith("n"))
                {
                    newProduct.Discontinued = temp.ToLower().StartsWith("y") ? true : false;
                    validBool = true;
                }

            } while (!validBool);

            return newProduct;
        }

        //Validates a Product, returns boolean value
        public static bool validateProduct(Product newProduct)
        {
            ValidationContext context = new ValidationContext(newProduct, null, null);
            List<ValidationResult> results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(newProduct, context, results, true);
            if (isValid)
            {
                var db = new NorthwindContext();
                // check for unique name
                if (db.Products.Any(p => p.ProductName == newProduct.ProductName))
                {
                    // generate validation error
                    isValid = false;
                    results.Add(new ValidationResult("Name exists", new string[] { "ProductName" }));
                }
                else if (newProduct.CategoryId != null && !db.Categories.Any(c => c.CategoryId == newProduct.CategoryId))
                {
                    isValid = false;
                    results.Add(new ValidationResult("Invalid Category ID", new string[] { "CategoryID" }));
                }
                else if (newProduct.SupplierId != null && !db.Suppliers.Any(s => s.SupplierId == newProduct.SupplierId))
                {
                    isValid = false;
                    results.Add(new ValidationResult("Invalid Supplier ID", new string[] { "SupplierID" }));
                }
                else
                {
                    logger.Info("Validation passed");
                    return true;
                }
            }
            if (!isValid)
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
            return false;
        }

        //Prompts User for Supplier fields, returns Supplier
        public static Supplier getSupplierinfo()
        {
            Supplier newSupplier = new Supplier();
            String temp = "";

            Console.WriteLine("Company Name:");
            temp = Console.ReadLine();
            if (temp.Equals(""))
            {
                newSupplier.CompanyName = null;
                Console.WriteLine("**Defaulted To Null**");
            }
            else
            {
                newSupplier.CompanyName = temp;
            }

            Console.WriteLine("Contact Name:");
            temp = Console.ReadLine();
            if (temp.Equals(""))
            {
                newSupplier.ContactName = null;
                Console.WriteLine("**Defaulted To Null**");
            }
            else
            {
                newSupplier.ContactName = temp;
            }

            Console.WriteLine("Contact Title:");
            temp = Console.ReadLine();
            if (temp.Equals(""))
            {
                newSupplier.ContactTitle = null;
                Console.WriteLine("**Defaulted To Null**");
            }
            else
            {
                newSupplier.ContactTitle = temp;
            }

            Console.WriteLine("Address:");
            temp = Console.ReadLine();
            if (temp.Equals(""))
            {
                newSupplier.Address = null;
                Console.WriteLine("**Defaulted To Null**");
            }
            else
            {
                newSupplier.Address = temp;
            }

            Console.WriteLine("City:");
            temp = Console.ReadLine();
            if (temp.Equals(""))
            {
                newSupplier.City = null;
                Console.WriteLine("**Defaulted To Null**");
            }
            else
            {
                newSupplier.City = temp;
            }

            Console.WriteLine("Region:");
            temp = Console.ReadLine();
            if (temp.Equals(""))
            {
                newSupplier.Region = null;
                Console.WriteLine("**Defaulted To Null**");
            }
            else
            {
                newSupplier.Region = temp;
            }

            Console.WriteLine("Postal Code:");
            temp = Console.ReadLine();
            if (temp.Equals(""))
            {
                newSupplier.PostalCode = null;
                Console.WriteLine("**Defaulted To Null**");
            }
            else
            {
                newSupplier.PostalCode = temp;
            }

            Console.WriteLine("Country:");
            temp = Console.ReadLine();
            if (temp.Equals(""))
            {
                newSupplier.Country = null;
                Console.WriteLine("**Defaulted To Null**");
            }
            else
            {
                newSupplier.Country = temp;
            }

            Console.WriteLine("Phone:");
            temp = Console.ReadLine();
            if (temp.Equals(""))
            {
                newSupplier.Phone = null;
                Console.WriteLine("**Defaulted To Null**");
            }
            else
            {
                newSupplier.Phone = temp;
            }

            Console.WriteLine("fax:");
            temp = Console.ReadLine();
            if (temp.Equals(""))
            {
                newSupplier.Fax = null;
                Console.WriteLine("**Defaulted To Null**");
            }
            else
            {
                newSupplier.Fax = temp;
            }

            return newSupplier;
        }

        //Validates Supplier, returns boolean
        public static bool validateSupplier(Supplier newSupplier)
        {
            var db = new NorthwindContext();
            ValidationContext context = new ValidationContext(newSupplier, null, null);
            List<ValidationResult> results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(newSupplier, context, results, true);
            if (isValid)
            {
                if (db.Suppliers.Any(s => s.CompanyName == newSupplier.CompanyName))
                {
                    isValid = false;
                    results.Add(new ValidationResult("Non-Unique name", new string[] { "SupplierName" }));
                }
                else
                {
                    logger.Info("Acceptable Entry");
                    return true;
                }
            }
            if (!isValid)
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
            return false;
        }

        //Prompts user for category fields, returns category
        public static Category getCategoryInfo()
        {
            Category category = new Category();
            Console.WriteLine("Enter Category Name:");
            category.CategoryName = Console.ReadLine();
            Console.WriteLine("Enter the Category Description:");
            category.Description = Console.ReadLine();
            return category;
        }

        //Validates category, returns boolean
        public static bool validateCategory(Category category)
        {
            ValidationContext context = new ValidationContext(category, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(category, context, results, true);
            if (isValid)
            {
                var db = new NorthwindContext();
                // check for unique name
                if (db.Categories.Any(c => c.CategoryName == category.CategoryName))
                {
                    // generate validation error
                    isValid = false;
                    results.Add(new ValidationResult("Name exists", new string[] { "CategoryName" }));
                }
                else
                {
                    logger.Info("Validation passed");
                    return true;
                }
            }
            if (!isValid)
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
            return false;
        }

        //Displays all Categorys and their Products with ID
        public static void displayAllCategoriesAndProducts()
        {
            var db = new NorthwindContext();
            var query = db.Categories.Include("Products").OrderBy(p => p.CategoryId);
            foreach (var item in query)
            {
                Console.WriteLine($"{item.CategoryName}");
                foreach (Product p in item.Products)
                {
                    if(p.Discontinued==false)
                    {
                        Console.WriteLine($"\tID: {p.ProductID}\tName:{p.ProductName}");
                        Console.ResetColor();
                    }
                }
            }
        }

        public static void displayAllCategoriesAndProducts(bool all)
        {
            var db = new NorthwindContext();
            var query = db.Categories.Include("Products").OrderBy(p => p.CategoryId);
            if(!all)
            {
                foreach (var item in query)
                {
                    Console.WriteLine($"{item.CategoryName}");
                    foreach (Product p in item.Products)
                    {
                        if (p.Discontinued == false)
                        {
                            Console.WriteLine($"\tID: {p.ProductID}\tName:{p.ProductName}");
                            Console.ResetColor();
                        }
                    }
                }
            }
            else
            {
                foreach (var item in query)
                {
                    Console.WriteLine($"{item.CategoryName}");
                    foreach (Product p in item.Products)
                    {
                        if(p.Discontinued==true)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        Console.WriteLine($"\tID: {p.ProductID}\tName:{p.ProductName}");
                        Console.ResetColor();
                    }
                }
            }
        }

        //DPrompts user for category, then displays all products
        public static void displayCategoryProducts()
        {
            int id = getTargetCategory();
            var db = new NorthwindContext();
            Console.Clear();
            logger.Info($"CategoryId {id} selected");
            Category category = db.Categories.FirstOrDefault(c => c.CategoryId == id);
            Console.WriteLine($"{category.CategoryName} - {category.Description}");
            foreach (Product p in category.Products)
            {
                if (p.Discontinued == false)
                {
                    Console.WriteLine(p.ProductName);
                }
            }
        }

        //Prompts user for valid Category ID, returns ID
        public static int getTargetCategory()
        {
            bool validId = false;
            var db = new NorthwindContext();
            int targetID = -1;
            do
            {
                Console.WriteLine("Select Target Category ID: ");
                displayCategories();
                String userInput = Console.ReadLine();
                if (int.TryParse(userInput, out targetID) && db.Categories.Any(c => c.CategoryId == targetID))
                {
                    validId = true;
                }
                else
                {
                    Console.WriteLine("Invalid Category ID");
                }

            } while (!validId);

            return targetID;
        }

        //Prompts User for valid Product ID, returns ID
        public static int getTargetProduct()
        {
            bool validId = false;
            var db = new NorthwindContext();
            int targetID = -1;
            do
            {
                Console.WriteLine("Select Target Product ID: ");
                displayAllCategoriesAndProducts(true);
                String userInput = Console.ReadLine();
                if (int.TryParse(userInput, out targetID) && db.Products.Any(p => p.ProductID == targetID))
                {
                    validId = true;
                }
                else
                {
                    Console.WriteLine("Invalid Product ID");
                }

            } while (!validId);

            return targetID;

        }

        //Selects Product, gets new Fields and Updates DataBase
        public static void editProductInfo()
        {
            int targetProduct = getTargetProduct();
            Product newProduct = getProductInfo();
            newProduct.ProductID = targetProduct;
            if (validateProduct(newProduct))
            {
                var db = new NorthwindContext();
                db.updateProduct(newProduct);
            }
        }

        //Selects category, gets new fields, and updates Database
        public static void editCategory()
        {
            int targetCategory = getTargetCategory();
            Category newCategory = getCategoryInfo();
            newCategory.CategoryId = targetCategory;
            if(validateCategory(newCategory))
            {
                var db = new NorthwindContext();
                db.updateCategory(newCategory);
            }
        }

        //Displays all Information on a Specific Product
        public static void displayProductInformation()
        {
            int productID = getTargetProduct();
            var db = new NorthwindContext();
            Product product = db.Products.Find(productID);
            if(product.Discontinued==true)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine($"\tID: {product.ProductID}\n\tName: {product.ProductName}\n\tCategory: {product.Category.CategoryName}\n\tSupplier: {product.Supplier.CompanyName}\n\tUnits In Stock: {product.UnitsInStock}\n\tUnits On Order: {product.UnitsOnOrder}\n\tReorder Level: {product.ReorderLevel}\n\tQuantity Per Unit: {product.QuantityPerUnit}\n\tUnit Price: {product.UnitPrice}\n\tDiscontinued: {product.Discontinued.ToString()}");
            Console.ResetColor();
        }

        //Displays all categories and their Description
        public static void displayCategoriesAndDescription()
        {
            var db = new NorthwindContext();
            var query = db.Categories.OrderBy(p => p.CategoryName);
            foreach (var item in query)
            {
                Console.WriteLine($"\tName: {item.CategoryName}\n\tDescription: {item.Description}\n");
            }
        }

        //Display All Products, Discontinued, or Active
        public static void displayProducts()
        {
            int option = -1;
            bool validChoice = false;
            do
            {
                Console.Clear();
                Console.WriteLine("1) Display All Products");
                Console.WriteLine("2) Display Active Products");
                Console.WriteLine("3) Display Discontinued Products");
                String userInput = Console.ReadLine();
                if(int.TryParse(userInput,out option)&&option>=1&&option<=3)
                {
                    validChoice = true;
                }
            } while (!validChoice);
            var db = new NorthwindContext();
            switch(option)
            {
                case 1:  //All Products
                    var allProducts = db.Products.OrderBy(p=>p.ProductName);
                    foreach(Product p in allProducts)
                    {
                        if(p.Discontinued==true)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        Console.WriteLine($"{p.ProductName}");
                        Console.ResetColor();
                    }
                    break;
                case 2:  //Active Products
                    var activeProducts = db.Products.Where(p=>p.Discontinued==false).OrderBy(p => p.ProductName);
                    foreach (Product p in activeProducts)
                    {
                        Console.WriteLine($"{p.ProductName}");
                        Console.ResetColor();
                    }
                    break;
                case 3:  //Discontinued Products
                    var discontinuedProducts = db.Products.Where(p=>p.Discontinued==true).OrderBy(p => p.ProductName);
                    Console.ForegroundColor = ConsoleColor.Red;
                    foreach (Product p in discontinuedProducts)
                    {
                        Console.WriteLine($"{p.ProductName}");
                    }
                    Console.ResetColor();
                    break;
                default:
                    break;
            }
        }

        public static void deleteCategory()
        {
            int categoryID = getTargetCategory();
            var db = new NorthwindContext();
            Category targetCategory = db.Categories.Find(categoryID);
            bool validResponce = false;
            bool delete = false;
            do
            {
                Console.WriteLine($"Are You Sure You Want To Delete {targetCategory.CategoryName}?(Y/N)\nAll Products in {targetCategory.CategoryName} will become Unassigned.");
                String yOrN = Console.ReadLine().ToUpper();
                if (yOrN.Equals("Y") || yOrN.Equals("N"))
                {
                    delete = yOrN.Equals("Y") ? true : false;
                    validResponce = true;
                }
                else
                {
                    Console.WriteLine("Invalid Choice");
                }
            } while (!validResponce);
            if (delete)
            {
                logger.Info($"Deleting Category {targetCategory.CategoryName}");
                Console.WriteLine($"Deleting {targetCategory.CategoryName}");
                db.deleteCategory(targetCategory);
            }
            else
            {
                Console.WriteLine("Delete Aborted");
            }
            Console.WriteLine("\n");
        }

        public static void deleteProduct()
        {
            int productID = getTargetProduct();
            var db = new NorthwindContext();
            Product targetProduct = db.Products.Find(productID);
            bool validResponce = false;
            int action = -1;
            do
            {
                Console.WriteLine($"Are You Sure You Want To Delete {targetProduct.ProductName}?\nAll Orders Of {targetProduct.ProductName} will be deleted.");
                Console.WriteLine("Consider Discontinuing Product. No Orders will be lost.");
                Console.WriteLine("1) Delete");
                Console.WriteLine("2) Discontinue");
                Console.WriteLine("3) Abort");
                String choice = Console.ReadLine();
                if(int.TryParse(choice, out action)&&action>=1&&action<=3)
                {
                    validResponce = true;
                }
                else
                {
                    Console.WriteLine("Invalid Choice");
                }

            } while (!validResponce);

            switch(action)
            {
                case 1:
                    db.deleteProduct(targetProduct);
                    Console.WriteLine("Product Deleted");
                    logger.Info($"Product {targetProduct.ProductName} Deleted");
                    break;
                case 2:
                    targetProduct.Discontinued = true;
                    db.updateProduct(targetProduct);
                    Console.WriteLine("Product Discontinued");
                    logger.Info($"Product {targetProduct.ProductName} Discontinued");
                    break;
                case 3:
                    Console.WriteLine("Delete Aborted");
                    break;
                default:

                    break;
            }
            Console.WriteLine("\n");
        }

    }
}
