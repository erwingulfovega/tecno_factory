namespace WebApi
{
    public class Login
    {
        public string Nombre_Usuario { get; set; }

        public string Clave { get; set; }

     
        public Login()
        {
            Nombre_Usuario = string.Empty;
            Clave = string.Empty;
        }

    }
}
