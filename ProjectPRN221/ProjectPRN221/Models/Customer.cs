﻿using System;
using System.Collections.Generic;

namespace ProjectPRN221.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Accounts = new HashSet<Account>();
            Orders = new HashSet<Order>();
        }

        public string? CustomerId { get; set; }
        public string? CompanyName { get; set; }
        public string? ContactName { get; set; }
        public string? ContactTitle { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string Phone { get; set; }
        public string? Fax { get; set; }

        public virtual ICollection<Account>? Accounts { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
