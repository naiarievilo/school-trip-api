using System;
using DotNetFiveApiDemo.Core.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DotNetFiveApiDemo.Core.User.Entities
{
    public class AppUser : IdentityUser<int>, IAggregateRoot
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public Address Address { get; init; }
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public bool UnlockMessageSent { get; set; }
    }

    public class Address
    {
        public string Street { get; init; }
        public string Number { get; init; }
        public string City { get; init; }
        public string State { get; init; }
        public string Country { get; init; }
        public string PostalCode { get; init; }
    }
}