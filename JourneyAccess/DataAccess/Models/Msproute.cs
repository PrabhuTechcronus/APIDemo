using System;
using System.Collections.Generic;

#nullable disable

namespace UG.Journey.JourneyAccess.DataAccess.Models 
{
    public partial class Msproute
    {
        public Msproute()
        {
            Msplegs = new HashSet<Mspleg>();
        }

        public int Id { get; set; }
        public string FromAddress { get; set; }
        public decimal? FromLatitude { get; set; }
        public decimal? FromLongitude { get; set; }
        public string ToAddress { get; set; }
        public decimal? ToLatitude { get; set; }
        public decimal? ToLongitude { get; set; }
        public decimal? Price { get; set; }
        public DateTime? Time { get; set; }
        public double? Distance { get; set; }

        public virtual ICollection<Mspleg> Msplegs { get; set; }
    }
}
