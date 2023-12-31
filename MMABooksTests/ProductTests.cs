using System.Collections.Generic;
using System.Linq;
using System;

using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using MMABooksEFClasses.Models;
//deleted "using... MarisModels", was causing ambiguous reference issue

namespace MMABooksTests
{
    [TestFixture]
    public class ProductTests
    {
        
        MMABooksContext dbContext;
        Product? p;
        List<Product>? products;

        [SetUp]
        public void Setup()
        {
            dbContext = new MMABooksContext();
            dbContext.Database.ExecuteSqlRaw("call usp_testingResetData()");
        }

        [Test]
        public void GetAllTest()
        {
            products = dbContext.Products.OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(16, products.Count);
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", products[0].Description);
            PrintAll(products);
        }

        [Test]
        public void GetByPrimaryKeyTest()
        {
            p = dbContext.Products.Find("A4CS");
            Assert.IsNotNull(p);
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", p.Description);
            Assert.AreEqual("A4CS", p.ProductCode);
            Console.WriteLine(p);
        }

        [Test]
        public void GetUsingWhere()
        {
            // get a list of all of the products that have a unit price of 56.50
            products = dbContext.Products.Where(p => p.UnitPrice.Equals(56.50m)).OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(7, products.Count);
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", products[0].Description);
            PrintAll(products);
        }

        [Test]
        public void GetWithInvoiceLineItemsTest()
        {
            // getting product with code A4CS and all of the invoice line items for that product
            p = dbContext.Products.Include("Invoicelineitems").Where(p => p.ProductCode == "A4CS").SingleOrDefault();
            Assert.IsNotNull(p);
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", p.Description);
            Assert.AreEqual(4, p.Invoicelineitems.Count);
            Console.WriteLine(p);
        }

        [Test]
        public void GetWithCalculatedFieldTest()
        {
            // get a list of objects that include the productcode, unitprice, quantity and inventoryvalue
            var products = dbContext.Products.Select(
            p => new { p.ProductCode, p.UnitPrice, p.OnHandQuantity, Value = p.UnitPrice * p.OnHandQuantity }).
            OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(16, products.Count);
            foreach (var p in products)
            {
                Console.WriteLine(p);
            }
        }

        [Test]
        public void DeleteTest()
        {
            p = dbContext.Products.Find("MDOM");
            dbContext.Products.Remove(p);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.Products.Find("MDOM"));
        }

        [Test]
        public void CreateTest()
        {
            p = new Product();
            p.ProductCode = "T3ST";
            p.Description = "Test Description";
            p.UnitPrice = 10m;
            p.OnHandQuantity = 101;
            dbContext.Products.Add(p);
            dbContext.SaveChanges();
            Assert.IsNotNull(dbContext.Products.Find(p.ProductCode));
        }

        [Test]
        public void UpdateTest()
        {
            p = dbContext.Products.Find("A4CS");
            p.Description = "Test Description";
            p.UnitPrice = 10m;
            p.OnHandQuantity = 101;
            dbContext.Products.Update(p);
            dbContext.SaveChanges();
            p = dbContext.Products.Find("A4CS");
            Assert.AreEqual("Test Description", p.Description);
        }

        public void PrintAll(List<Product> products)
        {
            foreach (Product p in products)
            {
                Console.WriteLine(p);
            }
        }

    }
}