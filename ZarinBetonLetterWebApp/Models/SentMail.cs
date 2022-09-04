

namespace ZarinBetonLetterWebApp.Models
{
    public class SentMail
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Date { get; set; }
        public int Format { get; set; } //A5 = 0 & A4 = 1
        public string Receiver { get; set; }
        public string Attaches { get; set; }
        public bool HasAttach { get; set; }
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
