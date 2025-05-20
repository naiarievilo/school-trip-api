using System.Collections.Generic;
using DotNetFiveApiDemo.Domain.Order.Entities;
using Microsoft.AspNetCore.Identity;

namespace DotNetFiveApiDemo.Application.User.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public Address Address { get; set; }
        public List<Order> Orders { get; set; }
    }

    public class Address
    {
        public string Street { get; init; }
        public string Number { get; init; }
        public string City { get; init; }
        public string State { get; init; }
        public string Country { get; init; }
        public string ZipCode { get; init; }
    }
}