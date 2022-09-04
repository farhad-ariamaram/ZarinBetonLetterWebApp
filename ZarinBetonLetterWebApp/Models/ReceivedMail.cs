

namespace ZarinBetonLetterWebApp.Models
{
    public class ReceivedMail
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Date { get; set; }
        public string Sender { get; set; }
        public string Attaches { get; set; }
        public bool HasAttach { get; set; }
        public string LetterImage { get; set; }
    }
}
