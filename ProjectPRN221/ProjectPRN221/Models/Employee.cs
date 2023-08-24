using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectPRN221.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Accounts = new HashSet<Account>();
            InverseReportsToNavigation = new HashSet<Employee>();
            Orders = new HashSet<Order>();
        }

        public int EmployeeId { get; set; }
        [Required(ErrorMessage ="You need to input Last Name")]

        public string? LastName { get; set; }
        [Required(ErrorMessage = "You need to input First Name")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "You need to input Title")]
        public string? Title { get; set; }
        public string? TitleOfCourtesy { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }
        [Required(ErrorMessage = "You need to input Address")]
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? HomePhone { get; set; }
        public string? Extension { get; set; }
        public byte[]?    Photo { get; set; }
        public string? Notes { get; set; }
        public int? ReportsTo { get; set; }
        public string? PhotoPath { get; set; }

        public virtual Employee? ReportsToNavigation { get; set; }
        public virtual ICollection<Account>? Accounts { get; set; }
        public virtual ICollection<Employee>? InverseReportsToNavigation { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
