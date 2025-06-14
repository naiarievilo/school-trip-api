#nullable disable

using System.ComponentModel.DataAnnotations;

namespace SchoolTripApi.Domain.Common.DTOs;

public abstract class Error(string code, string description) : IEquatable<Error>
{
    [Required(ErrorMessage = "Error description is required.")]
    public string Description { get; init; } =
        description ?? throw new ArgumentNullException(nameof(description));

    [Required(ErrorMessage = "Error code is required.")]
    public string Code { get; init; } =
        code ?? throw new ArgumentNullException(nameof(code));

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