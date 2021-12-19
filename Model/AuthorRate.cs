namespace Model
{
    public class AuthorRate : Rate
    {
        public int FkAuthor { get; set; }
        public Author Author { get; set; }
    }
}
