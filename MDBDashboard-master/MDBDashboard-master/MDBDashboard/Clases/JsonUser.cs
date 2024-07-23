using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MDBinASP.NET.Clases
{
    public class JsonUser
    {
        public int Id { get; set; }
        public string DocumentoIdentidad { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Nombre_Usuario { get; set; }

    }
}