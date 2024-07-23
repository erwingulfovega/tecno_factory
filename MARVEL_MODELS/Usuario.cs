namespace MARVEL_MODELS
{
    public class Usuario
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public Usuario()
        {
            Username=string.Empty;
            Password=string.Empty;
        }
    }
}
