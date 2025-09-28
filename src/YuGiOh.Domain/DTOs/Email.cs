/* src/Domain/DTOs/Email.cs */

namespace YuGiOh.Domain.DTOs
{
    public class Email
    {
        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHTML { get; set; }
        public string PlainTextBody { get; set; }
    }
}

