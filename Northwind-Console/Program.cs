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
                    Console.WriteLine("3) Display Category and related products");
                    Console.WriteLine("4) Display all Categories and their related products");
                    Console.WriteLine("5) Add Product");
                    Console.WriteLine("6) Add Supplier");
                    Console.WriteLine("\"q\" to quit");
                    choice = Console.ReadLine();
                    Console.Clear();
                    logger.Info($"Option {choice} selected");
                    if (choice == "1")
                    {
                        displayCategories();
                    } 
                    else if (choice == "2")
                    {
                        Category newCategory = getCategoryInfo();
                        submitCategory(newCategory);

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
                        Product newProduct = getProductInfo();
                        submitProduct(newProduct);

                    }
                    else if(choice=="6")
                    {
                        Supplier newSupplier = getSupplierinfo();
                        submitSupplier(newSupplier);

                    }
                    Console.WriteLine();

                } while (choice.ToLower() != "q");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }




        public static void displaySuppliers()
        {
            var db = new NorthwindContext();
            var suppliers = db.Suppliers.OrderBy(s=>s.SupplierId);
            foreach(Supplier s in suppliers)
            {
                Console.WriteLine($"ID: {s.SupplierId} Name: {s.CompanyName}");
            }
        }

        public static void displayCategories()
        {
            var db = new NorthwindContext();
            var cats = db.Categories.OrderBy(c => c.CategoryId);
            foreach (Category c in cats)
            {
                Console.WriteLine($"ID: {c.CategoryId} Name: {c.CategoryName}");
            }
        }

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

        public static void submitProduct(Product newProduct)
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
                    // TODO: save category to db
                }
            }
            if (!isValid)
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
        }

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

        public static void submitSupplier(Supplier newSupplier)
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
                }
            }
            if (!isValid)
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
        }

        public static Category getCategoryInfo()
        {
            Category category = new Category();
            Console.WriteLine("Enter Category Name:");
            category.CategoryName = Console.ReadLine();
            Console.WriteLine("Enter the Category Description:");
            category.Description = Console.ReadLine();
            return category;
        }

        public static void submitCategory(Category category)
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
                    // TODO: save category to db
                }
            }
            if (!isValid)
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
        }

        public static void displayAllCategoriesAndProducts()
        {
            var db = new NorthwindContext();
            var query = db.Categories.Include("Products").OrderBy(p => p.CategoryId);
            foreach (var item in query)
            {
                Console.WriteLine($"{item.CategoryName}");
                foreach (Product p in item.Products)
                {
                    Console.WriteLine($"\t{p.ProductName}");
                }
            }
        }

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
                Console.WriteLine(p.ProductName);
            }
        }

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

    }
}
