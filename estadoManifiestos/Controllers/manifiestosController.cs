using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using estadoManifiestos.Models;
using logicaNegocio;
using System.Diagnostics;
using Oracle.DataAccess.Client;


namespace estadoManifiestos.Controllers
{
    public class manifiestosController : Controller
    {
        #region libreria Externa
        /*
         * Instancia de la librería que maneja la conexión y tratamiento de datos
         */
        public logicaNegocios ln = new logicaNegocios();
        #endregion

        #region Estados General
        /*
         *ACCION: Retorna el estado en las diferentes entidades de los manifiestos generados en las últimas 24 horas
         */
        //[OutputCache(Duration = 60)]
        public ActionResult estados()
        {
            Stopwatch monitor = new Stopwatch();
            monitor.Start();
            try
            {
                List<manifiesto> listaManifiestos = new List<manifiesto>();
                DataTable dt = new DataTable();
                dt = ln.estadoRobots(1);
                int enviados = 0;
                int pendientes = 0;
                int error = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        manifiesto itemManifiesto = new manifiesto();
                        itemManifiesto.nroPlanilla = row["Planilla"].ToString();
                        itemManifiesto.fechaGen = String.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Parse(row["Fecha"].ToString()));
                        itemManifiesto.oficina = row["Oficina"].ToString();
                        try
                        {
                            itemManifiesto.estMinisterio = row["Ministerio"].ToString();
                            switch (itemManifiesto.estMinisterio)
                            {
                                case "E"://ENVIADO
                                    enviados++;
                                    break;
                                case "P"://PENDIENTE
                                    pendientes++;
                                    break;
                                case "R"://RECHAZADO
                                    error++;
                                    break;
                                case ""://RECHAZADO
                                    itemManifiesto.estMinisterio = "NC";//NO CATALOGADO
                                    break;
                                default:
                                    itemManifiesto.estMinisterio = "NA";//NO APLICA
                                    break;
                            }
                        }
                        catch (Exception)
                        {
                            itemManifiesto.estMinisterio = "NC";
                        }
                        try
                        {
                            itemManifiesto.estDestseguro = row["Deseguro"].ToString();
                            switch (itemManifiesto.estDestseguro)
                            {
                                case "E":
                                    enviados++;
                                    break;
                                case "P":
                                    pendientes++;
                                    break;
                                case "R":
                                    error++;
                                    break;
                                case ""://RECHAZADO
                                    itemManifiesto.estDestseguro = "NC";//NO CATALOGADO
                                    break;
                                default:
                                    itemManifiesto.estDestseguro = "NA";
                                    break;
                            }
                        }
                        catch (Exception)
                        {
                            itemManifiesto.estDestseguro = "NC";
                        }

                        try
                        {
                            itemManifiesto.estOsp = row["Osp"].ToString();
                            switch (itemManifiesto.estOsp)
                            {
                                case "E":
                                    enviados++;
                                    break;
                                case "P":
                                    pendientes++;
                                    break;
                                case "R":
                                    error++;
                                    break;
                                case ""://RECHAZADO
                                    itemManifiesto.estOsp = "NC";//NO CATALOGADO
                                    break;
                                default:
                                    itemManifiesto.estOsp = "NA";
                                    break;
                            }
                        }
                        catch (Exception)
                        {
                            itemManifiesto.estOsp = "NC";
                        }

                        try
                        {
                            itemManifiesto.estBavaria = row["Bavaria"].ToString();
                            switch (itemManifiesto.estBavaria)
                            {
                                case "E":
                                    enviados++;
                                    break;
                                case "P":
                                    pendientes++;
                                    break;
                                case "R":
                                    error++;
                                    break;
                                case ""://RECHAZADO
                                    itemManifiesto.estBavaria = "NC";//NO CATALOGADO
                                    break;
                                default:
                                    itemManifiesto.estBavaria = "NA";
                                    break;
                            }
                        }
                        catch (Exception)
                        {
                            itemManifiesto.estBavaria = "NC";
                        }
                        listaManifiestos.Add(itemManifiesto);
                    }
                }
                ViewBag.mEnviados = enviados;
                ViewBag.mPendientes = pendientes;
                ViewBag.mError = error;
                ViewBag.T_tiempo = monitor.ElapsedMilliseconds;
                return View(listaManifiestos);
            }
            catch (Exception Ex)
            {
                ModelState.AddModelError("ErrorLoad", Ex.Message);
                return View("Error");
            }
        }
        #endregion

        #region Consulta de estados
        /*ACCION: acción que permite consultar el estado de un manifiesto en cualquiera de las entidades a las que se le reporta
         *ESTADOS: aplica para los estados (enviados  y rechazados)
         *ENTIDADES: 1:mint;2:destSeguro;3:Osp
        */

        [HttpPost]
        public JsonResult estadomManifiesto(string planilla, int entidad)//1:mint;2:destSeguro;3:Osp;4:Bavaria
        {
            DataTable dt = new DataTable();
            List<estManifiesto> ListaestadoManif = new List<estManifiesto>();
            try
            {
                switch (entidad)
                {
                    case 1:
                        dt = ln.respuestaMinisterio(planilla);
                        break;
                    case 2:
                        dt = ln.respuestaDeseguro(planilla);
                        break;
                    case 3:
                        dt = ln.respuestaOsp(planilla);
                        break;
                    case 4:
                        dt = ln.respuestaBavaria(planilla);
                        break;
                    default:
                        break;
                }
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        estManifiesto itemEstado = new estManifiesto();
                        string oficina = row[0].ToString();
                        string fecha = String.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Parse(row[1].ToString()));
                        string idMin = row[2].ToString();
                        string respuesta = row[3].ToString();
                        itemEstado.oficina = oficina;
                        itemEstado.fecha = fecha;
                        itemEstado.idMin = idMin;
                        itemEstado.respuesta = respuesta;
                        ListaestadoManif.Add(itemEstado);
                    }

                }
                return Json(ListaestadoManif);

            }
            catch (OracleException oraEx)
            {
                Response.StatusCode = 500;
                return Json(Response.StatusDescription = oraEx.Message);
            }

            catch (Exception Ex)
            {
                Response.StatusCode = 500;
                return Json(Response.StatusDescription = Ex.Message);
            }


        }
        #endregion

        #region consulta por demanda
        /*ACCION: Retorna la información del manifiesto indicado
         * 
         */
        [HttpPost]
        public JsonResult demanda(string planilla)
        {
            try
            {
                List<manifiesto> listaManifiestos = new List<manifiesto>();
                DataTable dt = new DataTable();
                dt = ln.estadoRobots(planilla);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        manifiesto itemManifiesto = new manifiesto();
                        itemManifiesto.nroPlanilla = row["Planilla"].ToString();
                        itemManifiesto.fechaGen = String.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Parse(row["Fecha"].ToString()));
                        itemManifiesto.oficina = row["Oficina"].ToString();
                        try
                        {
                            itemManifiesto.estMinisterio = row["Ministerio"].ToString();
                        }
                        catch (Exception)
                        {

                            itemManifiesto.estMinisterio = "NC";
                        }
                        try
                        {
                            itemManifiesto.estDestseguro = row["Deseguro"].ToString();
                        }
                        catch (Exception)
                        {

                            itemManifiesto.estDestseguro = "NC";
                        }

                        try
                        {
                            itemManifiesto.estOsp = row["Osp"].ToString();
                        }
                        catch (Exception)
                        {
                            itemManifiesto.estOsp = "NC";
                        }
                        try
                        {
                            itemManifiesto.estBavaria = row["Bavaria"].ToString();
                        }
                        catch (Exception)
                        {
                            itemManifiesto.estBavaria = "NC";
                        }
                        listaManifiestos.Add(itemManifiesto);
                    }
                }
                return Json(listaManifiestos);
            }
            catch (OracleException oraEx)
            {
                Response.StatusCode = 500;
                return Json(Response.StatusDescription = oraEx.Message);
            }
            catch (Exception Ex)
            {
                Response.StatusCode = 500;
                return Json(Response.StatusDescription = Ex.Message);
            }
        }
        #endregion

    }
}
