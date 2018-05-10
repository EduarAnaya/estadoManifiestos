using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace estadoManifiestos.Models.dbSetings
{
    public class db_Param
    {
        public string servidor { get; set; }
        public string dbName { get; set; }
        public string usuario { get; set; }
        public string constraseña { get; set; }
        public db_Param()
        {
            this.servidor = "192.168.30.6";
            this.dbName = "MILEBOG1";
            this.usuario = "SOPORTE";
            this.constraseña = "SOPORTE";

        }
    }
}