namespace Trabalho_ISI.Data
{
    public class Movie
    {
        public int Id { get; set; }           // TMDB movie ID
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string Overview { get; set; }
        public string ReleaseDate { get; set; }
        public string PosterPath { get; set; }
        public string BackdropPath { get; set; }
    }
}
