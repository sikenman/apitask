using WebApi.Model;

namespace WebApi.Model
{
    public class Coordinates
    {
        public long? latitude { get; set; }
        public long? longitude { get; set; }
    }

    public class Animal
    {
        public string? id { get; set; }
        public string? genus { get; set; }
        public string? species { get; set; }
        public string? family { get; set; }
        public string? collectors { get; set; }
        public string? location { get; set; }
        public string? country { get; set; }
        public Coordinates? coordinates { get; set; }
        public string? collectionDate { get; set; }
        public bool isType { get; set; }
        public string? kindOfType { get; set; }
        public bool identified { get; set; }
        public string? identifier { get; set; }
        public string? identificationDate { get; set; }
    }
}
