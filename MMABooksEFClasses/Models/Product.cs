using MMABooksEFClasses.MarisModels;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace MMABooksEFClasses.Models
{
    public partial class Product
    {
        public Product()
        {
            Invoicelineitems = new HashSet<Invoicelineitem>();
        }

        public string ProductCode { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int OnHandQuantity { get; set; }

        public virtual ICollection<Invoicelineitem> Invoicelineitems { get; set; }

        public override string ToString()
        {
            return ProductCode + ", " + Description + ", " + UnitPrice + ", " + OnHandQuantity;
        }
        //copied from Customer.cs for printing
    }
}
