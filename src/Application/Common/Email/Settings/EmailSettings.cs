namespace SchoolTripApi.Application.Common.Email.Settings;

public class EmailSettings
{
    public required string SmtpServer { get; init; }
    public required int SmtpPort { get; init; }
    public required string SmtpUsername { get; init; }
    public required string SmtpPassword { get; init; }
    public required string SenderEmail { get; init; }
    public required string SupportEmail { get; init; }
    public required string SenderName { get; init; }
    public required bool EnableSsl { get; init; }
}