namespace WebApi
{
    public class Login
    {
        public string Usuario { get; set; }

        public string Password { get; set; }

     
        public Login()
        {
            Usuario = string.Empty;
            Password = string.Empty;
        }

    }
}
