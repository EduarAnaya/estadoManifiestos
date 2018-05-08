using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using estadoManifiestos.Models;
using estadoManifiestos.Models.dbSetings;
using logicaNegocio;


namespace estadoManifiestos.Controllers
{
    public class manifiestosController : Controller
    {
        //
        // GET: /manifiestos/

        [OutputCache(Duration = 60)]
        public ActionResult estados()
        {
            List<manifiesto> listaManifiestos = new List<manifiesto>();

            //db_Crud _db_Crud = new db_Crud();
            DataTable dt = new DataTable();
            //dt = _db_Crud.CargarListaRecientes();
            logicaNegocios ln = new logicaNegocios();
            dt = ln.estadoRobots(1);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    manifiesto itemManifiesto = new manifiesto();
                    itemManifiesto.nroPlanilla = row[0].ToString();
                    itemManifiesto.fechaGen = Convert.ToDateTime(row[1]);
                    itemManifiesto.oficina = row[2].ToString();
                    itemManifiesto.estMinisterio = int.Parse(row[3].ToString());
                    itemManifiesto.estDestseguro = int.Parse(row[4].ToString());
                    itemManifiesto.estOsp = int.Parse(row[5].ToString());
                    listaManifiestos.Add(itemManifiesto);
                }
            }
            /*codigos de estado;1=enviado;2:pendiente;3:error;4:norequiere*/

            //----------------------------------------------
            //manifiesto itemManifiesto1 = new manifiesto();
            //itemManifiesto1.nroPlanilla = "009467139";
            //itemManifiesto1.estMinisterio = 1;
            //itemManifiesto1.estDestseguro = 2;
            //itemManifiesto1.estOsp = 6;
            //listaManifiestos.Add(itemManifiesto1);
            ////----------------------------------------------
            //manifiesto itemManifiesto2 = new manifiesto();
            //itemManifiesto2.nroPlanilla = "0094311547";
            //itemManifiesto2.estMinisterio = 4;
            //itemManifiesto2.estDestseguro = 2;
            //itemManifiesto2.estOsp = 1;
            //listaManifiestos.Add(itemManifiesto2);
            ////----------------------------------------------
            //manifiesto itemManifiesto3 = new manifiesto();
            //itemManifiesto3.nroPlanilla = "0094213484";
            //itemManifiesto3.estMinisterio = 1;
            //itemManifiesto3.estDestseguro = 1;
            //itemManifiesto3.estOsp = 3;
            //listaManifiestos.Add(itemManifiesto3);
            ////----------------------------------------------
            //manifiesto itemManifiesto4 = new manifiesto();
            //itemManifiesto4.nroPlanilla = "0094120315";
            //itemManifiesto4.estMinisterio = 1;
            //itemManifiesto4.estDestseguro = 1;
            //itemManifiesto4.estOsp = 1;
            //listaManifiestos.Add(itemManifiesto4);

            return View(listaManifiestos);
        }

    }
}
