using System.ComponentModel.DataAnnotations.Schema;

namespace EmailerTestApp.Models
{
    public class EmailModel
    {
        public string? From { get; set; } = "example@email.com";
        public string? ToEmail { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public bool IsHtml { get; set; } = true;
    }
}
