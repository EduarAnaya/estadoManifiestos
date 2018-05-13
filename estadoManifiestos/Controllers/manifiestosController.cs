using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using estadoManifiestos.Models;
//using estadoManifiestos.Models.dbSetings;
using logicaNegocio;


namespace estadoManifiestos.Controllers
{
    public class manifiestosController : Controller
    {
        //
        // GET: /manifiestos/
        logicaNegocios ln = new logicaNegocios();

        [OutputCache(Duration = 60)]
        public ActionResult estados()
        {
            try
            {
                List<manifiesto> listaManifiestos = new List<manifiesto>();

                //db_Crud _db_Crud = new db_Crud();
                DataTable dt = new DataTable();
                //dt = _db_Crud.CargarListaRecientes();
                dt = ln.estadoRobots(1);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        manifiesto itemManifiesto = new manifiesto();
                        itemManifiesto.nroPlanilla = row[0].ToString();
                        itemManifiesto.fechaGen = row[1].ToString();
                        itemManifiesto.oficina = row[2].ToString();
                        try
                        {
                            itemManifiesto.estMinisterio = int.Parse(row[3].ToString());
                        }
                        catch (Exception)
                        {

                            itemManifiesto.estMinisterio = 4;
                        }
                        try
                        {
                            itemManifiesto.estDestseguro = int.Parse(row[4].ToString());
                        }
                        catch (Exception)
                        {

                            itemManifiesto.estDestseguro = 4;
                        }

                        try
                        {
                            itemManifiesto.estOsp = int.Parse(row[5].ToString());
                        }
                        catch (Exception)
                        {

                            itemManifiesto.estOsp = 4;
                        }

                        listaManifiestos.Add(itemManifiesto);
                    }
                }
                /*codigos de estado;1=enviado;2:pendiente;3:error;4:norequiere*/

                //----------------------------------------------
                //manifiesto itemManifiesto1 = new manifiesto();
                //itemManifiesto1.nroPlanilla = "009455252";
                //itemManifiesto1.estMinisterio = 3;
                //itemManifiesto1.estDestseguro = 2;
                //itemManifiesto1.estOsp = 6;
                //listaManifiestos.Add(itemManifiesto1);
                //----------------------------------------------
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
            catch (Exception)
            {
                return View("Error");
                throw;
            }

        }
        /*estadoministario*/
        [HttpPost]
        public JsonResult estadoministerio(string planilla, int entidad)//1:mint;2:destSeguro;3:Osp
        {
            DataTable dt = new DataTable();
            List<estManifiesto> ListaestadoMint = new List<estManifiesto>();

            if (entidad == 1)
            {
                dt = ln.respuestaMinisterio(planilla);
            }
            else if (entidad == 2)
            {
                dt = ln.respuestaDeseguro(planilla);
            }
            else if (entidad == 3)
            {
                dt = ln.respuestaOsp(planilla);
            }

            foreach (DataRow row in dt.Rows)
            {
                estManifiesto itemEstado = new estManifiesto();
                string oficina = row[0].ToString();
                string fecha = row[1].ToString();
                string idMin = row[2].ToString();
                string respuesta = row[3].ToString();
                itemEstado.oficina = oficina;
                itemEstado.fecha = fecha;
                itemEstado.idMin = idMin;
                itemEstado.respuesta = respuesta;
                ListaestadoMint.Add(itemEstado);
            }
            return Json(ListaestadoMint);
        }


        [HttpPost]
        public JsonResult demanda(string planilla)//1:mint;2:destSeguro;3:Osp
        {
            try
            {
                List<manifiesto> listaManifiestos = new List<manifiesto>();

                //db_Crud _db_Crud = new db_Crud();
                DataTable dt = new DataTable();
                //dt = _db_Crud.CargarListaRecientes();
                dt = ln.estadoRobots(planilla);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        manifiesto itemManifiesto = new manifiesto();
                        itemManifiesto.nroPlanilla = row[0].ToString();
                        itemManifiesto.fechaGen = row[1].ToString();
                        itemManifiesto.oficina = row[2].ToString();
                        try
                        {
                            itemManifiesto.estMinisterio = int.Parse(row[3].ToString());
                        }
                        catch (Exception)
                        {

                            itemManifiesto.estMinisterio = 4;
                        }
                        try
                        {
                            itemManifiesto.estDestseguro = int.Parse(row[4].ToString());
                        }
                        catch (Exception)
                        {

                            itemManifiesto.estDestseguro = 4;
                        }

                        try
                        {
                            itemManifiesto.estOsp = int.Parse(row[5].ToString());
                        }
                        catch (Exception)
                        {

                            itemManifiesto.estOsp = 4;
                        }

                        listaManifiestos.Add(itemManifiesto);
                    }
                }


                return Json(listaManifiestos);
            }
            catch (Exception)
            {
                return Json(0);
            }
        }

    }
}
