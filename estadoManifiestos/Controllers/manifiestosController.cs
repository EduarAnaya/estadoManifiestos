﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using estadoManifiestos.Models;
using logicaNegocio;
using System.Diagnostics;


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
        [OutputCache(Duration = 60)]
        public ActionResult estados()
        {
            Stopwatch monitor = new Stopwatch();
            monitor.Start();
            try
            {
                List<manifiesto> listaManifiestos = new List<manifiesto>();
                DataTable dt = new DataTable();
                dt = ln.estadoRobots(1);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        manifiesto itemManifiesto = new manifiesto();
                        itemManifiesto.nroPlanilla = row[0].ToString();
                        itemManifiesto.fechaGen = String.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Parse(row[1].ToString()));
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
                ViewBag.T_tiempo = monitor.ElapsedMilliseconds;
                return View(listaManifiestos);
            }
            catch (Exception)
            {
                return View("Error");
                throw;
            }
        }
        #endregion

        #region Consulta de estados
        /*ACCION: acción que permite consultar el estado de un manifiesto en cualquiera de las entidades a las que se le reporta
         *ESTADOS: aplica para los estados (enviados  y rechazados)
         *ENTIDADES: 1:mint;2:destSeguro;3:Osp
        */

        [HttpPost]
        public JsonResult estadomManifiesto(string planilla, int entidad)
        {
            DataTable dt = new DataTable();
            List<estManifiesto> ListaestadoManif = new List<estManifiesto>();
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
                return Json(ListaestadoManif);
            }
            else
            {
                return Json(ListaestadoManif);
            }
        }
        #endregion

        #region consulta por demanda
        /*ACCION: Retorna la información del manifiesto indicado
         * 
         */
        [HttpPost]
        public JsonResult demanda(string planilla)//1:mint;2:destSeguro;3:Osp
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
                        itemManifiesto.nroPlanilla = row[0].ToString();
                        itemManifiesto.fechaGen = String.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Parse(row[1].ToString()));
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
        #endregion

    }
}
