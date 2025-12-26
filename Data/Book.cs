namespace Trabalho_ISI.Data
{
    public class Book
    {
        public string Id { get; set; }       // Google Books ID
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Authors { get; set; }  // Comma-separated
        public string Publisher { get; set; }
        public string PublishedDate { get; set; }
        public string Description { get; set; }
    }
}
