using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MDBinASP.NET.Clases
{
    public class Comic
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ResourceUri { get; set; }
    }
}