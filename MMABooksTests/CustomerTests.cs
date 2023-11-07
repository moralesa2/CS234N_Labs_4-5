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
    public class CustomerTests
    {
        MMABooksContext dbContext;
        Customer? c;
        List<Customer>? customers;

        [SetUp]
        public void Setup()
        {
            dbContext = new MMABooksContext();
            dbContext.Database.ExecuteSqlRaw("call usp_testingResetData()");
        }

        [Test]
        public void GetAllTest()
        {
            customers = dbContext.Customers.OrderBy(c => c.Name).ToList();
            Assert.AreEqual(696, customers.Count);
            Assert.AreEqual("Abeyatunge, Derek", customers[0].Name);
            PrintAll(customers);
        }

        [Test]
        public void GetByPrimaryKeyTest()
        {
            c = dbContext.Customers.Find(157);
            Assert.IsNotNull(c);
            Assert.AreEqual("Abeyatunge, Derek", c.Name);
            Assert.AreEqual(157, c.CustomerId);
            Console.WriteLine(c);
        }

        [Test]
        public void GetUsingWhere()
        {
            // get a list of all of the customers who live in OR
            customers = dbContext.Customers.Where(c => c.State.StartsWith("OR")).OrderBy(c => c.Name).ToList();
            Assert.AreEqual(5, customers.Count);
            Assert.AreEqual("Erpenbach, Lee", customers[0].Name);
            PrintAll(customers);
        }

        [Test]
        public void GetWithInvoicesTest()
        {
            // get the customer whose id is 20 and all of the invoices for that customer
            c = dbContext.Customers.Include("Invoices").Where(c => c.CustomerId == 20).SingleOrDefault();
            Assert.IsNotNull(c);
            Assert.AreEqual("Chamberland, Sarah", c.Name);
            Assert.AreEqual(3, c.Invoices.Count);
            Console.WriteLine(c);
        }

        [Test]
        public void GetWithJoinTest()
        {
            // get a list of objects that include the customer id, name, statecode and statename
            var customers = dbContext.Customers.Join(
               dbContext.States,
               c => c.State,
               s => s.StateCode,
               (c, s) => new { c.CustomerId, c.Name, c.State, s.StateName }).OrderBy(r => r.StateName).ToList();
            Assert.AreEqual(696, customers.Count);
            // I wouldn't normally print here but this lets you see what each object looks like
            foreach (var c in customers)
            {
                Console.WriteLine(c);
            }
        }

        [Test]
        public void DeleteTest()
        {
            c = dbContext.Customers.Find(1);
            dbContext.Customers.Remove(c);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.Customers.Find(1));
        }

        [Test]
        public void CreateTest()
        {
            c = new Customer();
            c.Name = "Test Customer";
            c.Address = "Test Address";
            c.City = "Test City";
            c.State = "OR";
            c.ZipCode = "10001";
            dbContext.Customers.Add(c);
            dbContext.SaveChanges();
            Assert.IsNotNull(dbContext.Customers.Find(c.CustomerId));
        }

        [Test]
        public void UpdateTest()
        {
            c = dbContext.Customers.Find(1);
            c.Name = "Test Customer";
            c.Address = "Test Address";
            c.City = "Test City";
            c.State = "OR";
            c.ZipCode = "10001";
            dbContext.Customers.Update(c);
            dbContext.SaveChanges();          
            dbContext.Customers.Find(c.CustomerId);
            Assert.AreEqual("Test Customer", c.Name);
        }

        public void PrintAll(List<Customer> customers)
        {
            foreach (Customer c in customers)
            {
                Console.WriteLine(c);
            }
        }     
    }
}