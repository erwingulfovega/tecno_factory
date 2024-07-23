namespace WebApi
{
    public class Comics
    {
        public int Id { get; set; }
        public int IdComics { get; set; }
        public int IdUsuario { get; set; }
      
        public Comics()
        {
            Id = 0;
            IdComics = 0;
            IdUsuario = 0;
        }

    }
}
