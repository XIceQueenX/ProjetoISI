namespace Trabalho_ISI.Data
{
    /// <summary>
    /// Represents a book entity stored in the database.
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Unique identifier for the book (e.g., external API ID or ISBN).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Main title of the book.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Subtitle of the book (optional).
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// Author or authors of the book.
        /// Stored as a string (e.g., comma-separated).
        /// </summary>
        public string Authors { get; set; }

        /// <summary>
        /// Publisher of the book.
        /// </summary>
        public string Publisher { get; set; }

        /// <summary>
        /// Publication date of the book.
        /// Stored as a string (e.g., YYYY-MM-DD).
        /// </summary>
        public string PublishedDate { get; set; }

        /// <summary>
        /// Short description or synopsis of the book.
        /// </summary>
        public string Description { get; set; }
    }
}
