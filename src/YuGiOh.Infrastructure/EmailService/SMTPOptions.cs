namespace YuGiOh.Infrastructure.EmailService
{
    public class SMTPOptions

    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FromAddress { get; set; }
        public string FromDisplayName { get; set; }
    }
}