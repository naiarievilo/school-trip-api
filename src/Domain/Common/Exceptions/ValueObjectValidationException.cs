namespace SchoolTripApi.Domain.Common.Exceptions;

public class ValueObjectValidationException(string message) : Exception(message);