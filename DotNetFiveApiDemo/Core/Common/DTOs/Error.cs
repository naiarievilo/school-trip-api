using System;
using System.ComponentModel.DataAnnotations;

namespace DotNetFiveApiDemo.Core.Common.DTOs
{
    public abstract class Error : IEquatable<Error>
    {
        protected Error(string code, string description)
        {
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Code = code ?? throw new ArgumentNullException(nameof(code));
        }

        [Required(ErrorMessage = "Error description is required.")]
        public string Description { get; }

        [Required(ErrorMessage = "Error code is required.")]
        public string Code { get; }

        public bool Equals(Error other)
        {
            if (other is null) return false;
            return Description == other.Description && Code == other.Code;
        }

        public override bool Equals(object obj)
        {
            return obj is Error error && Equals(error);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Description, Code);
        }

        public static bool operator ==(Error left, Error right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(Error left, Error right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return $"{Code}: {Description}";
        }

        public static implicit operator string(Error error)
        {
            return error.ToString();
        }
    }
}