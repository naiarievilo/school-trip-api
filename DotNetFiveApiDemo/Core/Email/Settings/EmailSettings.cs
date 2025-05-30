namespace DotNetFiveApiDemo.Core.Email.Settings
{
    internal class EmailSettings
    {
        public string SmtpServer { get; init; }
        public int SmtpPort { get; init; }
        public string SmtpUsername { get; init; }
        public string SmtpPassword { get; init; }
        public string SenderEmail { get; init; }
        public string SupportEmail { get; init; }
        public string SenderName { get; init; }
        public bool EnableSsl { get; init; }
    }
}