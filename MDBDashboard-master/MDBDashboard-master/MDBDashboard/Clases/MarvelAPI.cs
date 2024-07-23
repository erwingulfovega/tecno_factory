using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MDBinASP.NET.Clases
{
    public class MarvelAPI
    {

        public class Comic
        {
            public int Id { get; set; } // Assuming "id" property represents the comic ID
            public string Title { get; set; }
            public string Description { get; set; }
            public string ResourceURI { get; set; }
            public Url[] Urls { get; set; }
            public Series Series { get; set; }
            public Image Thumbnail { get; set; }
            // ... other properties (creators, characters, stories, etc.) based on your needs
        }

        public class Url
        {
            public string Type { get; set; }
            public string url { get; set; }
        }

        public class Series
        {
            public string ResourceURI { get; set; }
            public string Name { get; set; }
        }

        public class Image
        {
            public string Path { get; set; }
            public string Extension { get; set; }
        }

        public class MarvelData
        {
            public int Offset { get; set; }
            public int Limit { get; set; }
            public int Total { get; set; }
            public int Count { get; set; }
            public Comic[] Results { get; set; }
        }

        public class RootObject
        {
            public int Code { get; set; }
            public string Status { get; set; }
            public string Copyright { get; set; }
            public string AttributionText { get; set; }
            public string AttributionHTML { get; set; }
            public string Etag { get; set; }
            public MarvelData Data { get; set; }
        }

    }

}