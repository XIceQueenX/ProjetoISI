namespace Trabalho_ISI.Data
{
    /// <summary>
    /// Represents a movie entity stored in the database.
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// Unique identifier for the movie.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Movie title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Original title of the movie (if different).
        /// </summary>
        public string OriginalTitle { get; set; }

        /// <summary>
        /// Short synopsis or overview of the movie.
        /// </summary>
        public string Overview { get; set; }

        /// <summary>
        /// Movie release date (stored as string, e.g. YYYY-MM-DD).
        /// </summary>
        public string ReleaseDate { get; set; }

        /// <summary>
        /// Path or URL to the movie poster image.
        /// </summary>
        public string? PosterPath { get; set; }

        /// <summary>
        /// Path or URL to the movie backdrop image.
        /// </summary>
        public string? BackdropPath { get; set; }
    }
}
