namespace SchoolTripApi.Domain.Common.Exceptions;

public class ValueObjectException : Exception
{
    public ValueObjectException(string message) : base(message)
    {
    }

    private ValueObjectException(string message, string? propertyName) : base(message)
    {
        PropertyName = propertyName;
    }

    public ValueObjectException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public string? PropertyName { get; set; }

    public ValueObjectException WithPropertyContext(string? propertyName)
    {
        return new ValueObjectException(Message, propertyName);
    }
}