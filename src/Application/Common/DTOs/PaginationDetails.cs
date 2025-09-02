using System.ComponentModel.DataAnnotations;

namespace SchoolTripApi.Application.Common.DTOs;

public sealed class PaginationDetails
{
    [Range(1, int.MaxValue, ErrorMessage = "Page number must be a positive integer.")]
    public int PageNumber { get; set; } = 1;

    [Range(1, 100, ErrorMessage = "Page size must range between 1 and 100.")]
    public int PageSize { get; set; } = 10;
}