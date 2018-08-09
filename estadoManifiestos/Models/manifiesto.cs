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
        public string estMinisterio{ get; set; }
        public string estDestseguro { get; set; }
        public string estOsp { get; set; }
        public string estBavaria { get; set; }
        
        
    }
}