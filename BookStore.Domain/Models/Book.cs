namespace BookStore.Domain.Models
{
    public class Book : Entity
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public string CoverPhoto { get; set; }

        public int PublicationYear { get; set; }

        public Publisher Publisher { get; set; }
    }
}
