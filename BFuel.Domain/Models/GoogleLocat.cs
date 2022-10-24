using System;
using System.Collections.Generic;
using System.Text;

namespace BFuel.Domain.Models
{
    public class GoogleLocat
    {
        public Plus_Code plus_code { get; set; }
        public List<Result> results { get; set; }
        public string status { get; set; }
    }

    public class Plus_Code
    {
        public string compound_code { get; set; }
        public string global_code { get; set; }
    }

    public class Result
    {
        public List<Address_Components> adress_components { get; set; }
        public string formatted_address { get; set; }
        public Geometric geometry { get; set; }
        public string place_id { get; set; }
        public Plus_Code plus_code { get; set; }
        public List<string> types { get; set; }
    }

    public class Address_Components
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public List<string> types { get; set; }
    }

    public class Geometric
    {
        public Bounds bounds { get; set; }
        public Locations location { get; set; }
        public string location_type { get; set; }
        public ViewPort viewport { get; set; }
    }

    public class Bounds
    {
        public Northeast northeast { get; set; }
        public Southeast southwest { get; set; }
    }

    public class Locations
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class ViewPort
    {
        public Northeast northeast { get; set; }
        public Southeast southwest { get; set; }
    }
    public class Northeast
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Southeast
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }
}
