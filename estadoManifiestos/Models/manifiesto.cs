using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace estadoManifiestos.Models
{
    public class manifiesto
    {
        public string nroPlanilla { get; set; }
        public string fechaGen{get;set;}
        public string oficina{get;set;}
        public int estMinisterio{ get; set; }
        public int estDestseguro { get; set; }
        public int estOsp { get; set; }
    }
}