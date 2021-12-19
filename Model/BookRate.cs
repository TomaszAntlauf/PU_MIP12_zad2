namespace Model
{
    public class BookRate : Rate
    {
        public int FkBook { get; set; }
        public Book Book { get; set; }
    }
}
