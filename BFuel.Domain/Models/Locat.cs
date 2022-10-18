using System;
using System.Collections.Generic;
using System.Text;

namespace BFuel.Domain.Models
{
    public class Locat
    {
        public Summary summary { get; set; }

        public Results results { get; set; }
    }

    public class Summary
    {
        public string query { get; set; }
        public string queryType { get; set; }
        public int queryTime { get; set; }
        public int numResults { get; set; }
        public int offset { get; set; }
        public int totalResults { get; set; }
        public int fuzzyLevel { get; set; }
    }

    public class Results
    {
        public string type { get; set; }
        public string id { get; set; }
        public double score { get; set; }
        public string entityType { get; set; }
        public MatchConfidence matchConfidence { get; set; }
        public Address address { get; set; }
        public Position position { get; set; }
        public Viewport viewport { get; set; }
        public Boundingbox boundingBox { get; set; }
        public DataSources dataSources { get; set; }
    }

    public class MatchConfidence
    {
        public int score { get; set; }
    }

    public class Address
    {
        public string municipalitySubdivision { get; set; }
        public string municipality { get; set; }
        public string countrySubdivision { get; set; }
        public string countryCode { get; set; }
        public string country { get; set; }
        public string countryCodeISO3 { get; set; }
        public string freeformAddress { get; set; }
    }

    public class Position
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }

    public class Viewport
    {
        public TopLeftPoint topLeftPoint { get; set; }
        public BtmRightPoint btmRightPoint { get; set; }
    }

    public class Boundingbox
    {
        public TopLeftPoint topLeftPoint { get; set; }
        public BtmRightPoint btmRightPoint { get; set; }
    }

    public class TopLeftPoint
    {
        public double lat { get; set; }
        public double lont { get; set; }
    }

    public class BtmRightPoint
    {
        public double lat { get; set; }
        public double lont { get; set; }
    }

    public class DataSources
    {
        public Geometry geometry { get; set; }
    }

    public class Geometry
    {
        public string id { get; set; }
    }
}
