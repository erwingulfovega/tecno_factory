namespace WebApi
{
    public class Usuarios
    {
        public int Id { get; set; }
        public string DocumentoIdentidad { get; set; }

        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        public string? Email { get; set; }

        public string Nombre_Usuario { get; set; }

        public string Clave { get; set; }

        public Usuarios()
        {
            Id = 0;
            DocumentoIdentidad= string.Empty;
            Nombres= string.Empty;
            Apellidos= string.Empty;
            Email= string.Empty;
            Nombre_Usuario= string.Empty;
            Clave= string.Empty;
        }

    }
}
