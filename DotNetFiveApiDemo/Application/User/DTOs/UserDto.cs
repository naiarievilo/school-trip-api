namespace DotNetFiveApiDemo.Application.User.DTOs
{
    public class UserDto
    {
        public string Email { get; init; }

        public bool IsEmailConfirmed { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }

        public string Street { get; init; }

        public string Number { get; init; }

        public string City { get; init; }

        public string State { get; init; }

        public string Country { get; init; }

        public string ZipCode { get; init; }
    }
}