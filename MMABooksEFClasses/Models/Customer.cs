﻿using System;
using System.Collections.Generic;

namespace MMABooksEFClasses.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Invoices = new HashSet<Invoice>();
        }

        public int CustomerId { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string ZipCode { get; set; } = null!;

        public virtual State? StateNavigation { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }

        public override string ToString()
        {
            return CustomerId + ", " + Name + ", " + Address + ", " + City + ", " + State + ", " + ZipCode;
        }
        //added string override from MarisModels to Customers.cs so customers print properly

    }
}
